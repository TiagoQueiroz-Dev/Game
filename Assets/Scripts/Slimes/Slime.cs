using System;
using System.Collections;
using UnityEngine;

// Vida, movimento por pulos (estilo Terraria) e feedback de dano do slime.
public class Slime : MonoBehaviour
{
    private enum JumpState { Idle, Charging, Airborne, Landing }

    [Header("Vida")]
    [SerializeField] private int maxHealth = 3;
    [Tooltip("Tempo mínimo entre dois golpes, evita que um único soco conte várias vezes.")]
    [SerializeField] private float hitCooldown = 0.3f;

    [Header("Alvo")]
    [Tooltip("Distância em que o slime percebe o jogador e começa a pular na direção dele.")]
    [SerializeField] private float detectionRange = 15f;
    [Tooltip("Distância (horizontal) em que o slime para de avançar e só pula no lugar.")]
    [SerializeField] private float stopDistance = 0.6f;
    [SerializeField] private float turnSpeed = 6f;

    [Header("Pulo")]
    [Tooltip("Tempo parado no chão entre pulos (mín/máx) quando está seguindo o jogador.")]
    [SerializeField] private Vector2 idleTime = new Vector2(0.6f, 1.4f);
    [Tooltip("Tempo agachando antes de pular.")]
    [SerializeField] private float chargeTime = 0.25f;
    [SerializeField] private float jumpDuration = 0.55f;
    [SerializeField] private float jumpHeight = 0.6f;
    [Tooltip("Distância horizontal máxima de cada pulo.")]
    [SerializeField] private float jumpDistance = 1.6f;
    [Tooltip("Tempo do 'esmagamento' ao aterrissar.")]
    [SerializeField] private float landTime = 0.15f;

    [Header("Deformação do corpo")]
    [SerializeField] private Vector3 chargeSquash = new Vector3(1.25f, 0.65f, 1.25f);
    [SerializeField] private Vector3 jumpStretch = new Vector3(0.85f, 1.25f, 0.85f);
    [SerializeField] private Vector3 landSquash = new Vector3(1.3f, 0.6f, 1.3f);

    [Header("Feedback de dano")]
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float hitFlashTime = 0.12f;
    [SerializeField] private Vector3 hitSquash = new Vector3(1.25f, 0.7f, 1.25f);
    [SerializeField] private float knockbackSpeed = 4f;
    [SerializeField] private float knockbackDamping = 20f;
    [SerializeField] private float deathShrinkTime = 0.3f;

    public event Action<Slime> Died;

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    private Transform target;
    private Renderer[] renderers;
    private Material[][] materials;
    private Color[][] baseColors;
    private Vector3 baseScale;
    private float groundOffset;
    private float lastHitTime = -999f;
    private Vector3 knockbackVelocity;
    private Coroutine flashRoutine;
    private bool hitSquashActive;

    private JumpState state = JumpState.Idle;
    private float stateTimer;
    private float idleDuration;
    private Vector3 jumpVelocity;
    private float currentJumpHeight;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        baseScale = transform.localScale;
        CacheMaterials();
        groundOffset = ComputeGroundOffset();
        idleDuration = UnityEngine.Random.Range(0f, idleTime.y); // dessincroniza os slimes
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (IsDead) return;

        float dt = Time.deltaTime;
        Vector3 pos = transform.position;
        float previousTimer = stateTimer;
        stateTimer += dt;

        // Situação do jogador.
        Vector3 toTarget = Vector3.zero;
        float dist = float.MaxValue;
        bool hasTarget = false;
        if (target != null)
        {
            toTarget = target.position - pos;
            toTarget.y = 0f;
            dist = toTarget.magnitude;
            hasTarget = dist <= detectionRange;
        }

        // No chão, vira para o jogador.
        if (hasTarget && state != JumpState.Airborne && dist > 0.01f)
        {
            Quaternion look = Quaternion.LookRotation(toTarget / dist, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * dt);
        }

        Vector3 stateScale = Vector3.one;
        float airHeight = 0f;

        switch (state)
        {
            case JumpState.Idle:
                if (stateTimer >= idleDuration)
                    EnterState(JumpState.Charging);
                break;

            case JumpState.Charging:
                stateScale = Vector3.Lerp(Vector3.one, chargeSquash, stateTimer / chargeTime);
                if (stateTimer >= chargeTime)
                    StartJump(hasTarget, toTarget, dist);
                break;

            case JumpState.Airborne:
            {
                // Avança só o tempo que ainda resta do pulo (evita passar do ponto no último frame).
                float moveTime = Mathf.Min(stateTimer, jumpDuration) - Mathf.Min(previousTimer, jumpDuration);
                pos += jumpVelocity * moveTime;

                float t = Mathf.Clamp01(stateTimer / jumpDuration);
                airHeight = 4f * currentJumpHeight * t * (1f - t); // arco parabólico
                // Esticado na subida e na descida, normal no topo.
                stateScale = Vector3.Lerp(jumpStretch, Vector3.one, Mathf.Sin(t * Mathf.PI));

                if (t >= 1f)
                    EnterState(JumpState.Landing);
                break;
            }

            case JumpState.Landing:
                stateScale = Vector3.Lerp(landSquash, Vector3.one, stateTimer / landTime);
                if (stateTimer >= landTime)
                {
                    // Sem jogador por perto: fica mais tempo parado entre pulos.
                    idleDuration = hasTarget
                        ? UnityEngine.Random.Range(idleTime.x, idleTime.y)
                        : UnityEngine.Random.Range(idleTime.y, idleTime.y * 2.5f);
                    EnterState(JumpState.Idle);
                }
                break;
        }

        // Empurrão ao levar soco.
        pos += knockbackVelocity * dt;
        knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, knockbackDamping * dt);

        // Escala final = base * deformação do pulo * deformação do golpe.
        Vector3 deform = hitSquashActive ? Vector3.Scale(stateScale, hitSquash) : stateScale;
        transform.localScale = Vector3.Scale(baseScale, deform);

        // Mantém a base do slime encostada no chão mesmo achatado.
        if (SlimeGround.TryGetHeight(pos, out float groundY))
            pos.y = groundY + groundOffset * deform.y + airHeight;

        transform.position = pos;
    }

    private void StartJump(bool hasTarget, Vector3 toTarget, float dist)
    {
        Vector3 dir = Vector3.zero;
        float distance = 0f;
        currentJumpHeight = jumpHeight;

        if (hasTarget)
        {
            float remaining = dist - stopDistance;
            if (remaining > 0.05f)
            {
                dir = toTarget / dist;
                distance = Mathf.Min(jumpDistance, remaining);
            }
            // Já está perto: pula no lugar.
        }
        else
        {
            // Sem jogador por perto: pulinho curto em direção aleatória.
            Vector2 rnd = UnityEngine.Random.insideUnitCircle.normalized;
            dir = new Vector3(rnd.x, 0f, rnd.y);
            distance = jumpDistance * 0.5f;
            currentJumpHeight = jumpHeight * 0.6f;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        jumpVelocity = dir * (distance / jumpDuration);
        EnterState(JumpState.Airborne);
    }

    private void EnterState(JumpState newState)
    {
        state = newState;
        stateTimer = 0f;
    }

    /// <summary>Aplica dano fixo. Retorna true se o golpe foi aceito.</summary>
    public bool TakeHit(int damage, Vector3 hitDirection)
    {
        if (IsDead || Time.time - lastHitTime < hitCooldown) return false;

        lastHitTime = Time.time;
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

        hitDirection.y = 0f;
        if (hitDirection.sqrMagnitude > 0.0001f)
            knockbackVelocity = hitDirection.normalized * knockbackSpeed;

        if (IsDead)
        {
            Die();
            return true;
        }

        // Se estava se preparando para pular, cancela (fica atordoado um pouco).
        if (state == JumpState.Charging || state == JumpState.Idle)
        {
            idleDuration = idleTime.y;
            EnterState(JumpState.Idle);
        }

        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(HitFlash());

        return true;
    }

    private void Die()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        Died?.Invoke(this);
        StopAllCoroutines();
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator HitFlash()
    {
        SetColor(hitColor);
        hitSquashActive = true;

        yield return new WaitForSeconds(hitFlashTime);

        RestoreColors();
        hitSquashActive = false;
        flashRoutine = null;
    }

    private IEnumerator DeathRoutine()
    {
        SetColor(hitColor);
        Vector3 start = transform.localScale;
        float t = 0f;

        while (t < deathShrinkTime)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, Vector3.zero, t / deathShrinkTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    private float ComputeGroundOffset()
    {
        Bounds bounds;
        Renderer rend = GetComponentInChildren<Renderer>();
        Collider col = GetComponentInChildren<Collider>();

        if (rend != null) bounds = rend.bounds;
        else if (col != null) bounds = col.bounds;
        else return 0f;

        return transform.position.y - bounds.min.y;
    }

    // ---- Cores (pisca ao levar dano) ----

    private void CacheMaterials()
    {
        renderers = GetComponentsInChildren<Renderer>();
        materials = new Material[renderers.Length][];
        baseColors = new Color[renderers.Length][];

        for (int r = 0; r < renderers.Length; r++)
        {
            materials[r] = renderers[r].materials; // instancia os materiais deste slime
            baseColors[r] = new Color[materials[r].Length];
            for (int m = 0; m < materials[r].Length; m++)
                baseColors[r][m] = GetColor(materials[r][m]);
        }
    }

    private void SetColor(Color color)
    {
        foreach (Material[] mats in materials)
            foreach (Material mat in mats)
                ApplyColor(mat, color);
    }

    private void RestoreColors()
    {
        for (int r = 0; r < materials.Length; r++)
            for (int m = 0; m < materials[r].Length; m++)
                ApplyColor(materials[r][m], baseColors[r][m]);
    }

    private static Color GetColor(Material mat)
    {
        if (mat.HasProperty("_BaseColor")) return mat.GetColor("_BaseColor");
        if (mat.HasProperty("_Color")) return mat.GetColor("_Color");
        return Color.white;
    }

    private static void ApplyColor(Material mat, Color color)
    {
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
    }

    private void OnDestroy()
    {
        if (materials == null) return;
        foreach (Material[] mats in materials)
            foreach (Material mat in mats)
                if (mat != null) Destroy(mat);
    }
}
