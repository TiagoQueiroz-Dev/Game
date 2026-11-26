using System.Collections.Generic;
using UnityEngine;

// Cria slimes periodicamente em uma área circular ao redor deste objeto.
public class SlimeSpawner : MonoBehaviour
{
    [Header("Slime")]
    [Tooltip("Prefab do slime. Se vazio, é gerada uma esfera verde simples (bom para testar).")]
    [SerializeField] private GameObject slimePrefab;
    [Tooltip("Alvo que os slimes seguem. Se vazio, usa a Main Camera (cabeça do jogador).")]
    [SerializeField] private Transform player;

    [Header("Spawn")]
    [SerializeField] private int maxAlive = 5;
    [SerializeField] private int spawnOnStart = 2;
    [Tooltip("Segundos entre cada tentativa de spawn.")]
    [SerializeField] private float spawnInterval = 5f;
    [Tooltip("Raio da área de spawn ao redor deste objeto.")]
    [SerializeField] private float spawnRadius = 20f;
    [Tooltip("Distância mínima do jogador para o slime não nascer colado nele.")]
    [SerializeField] private float minDistanceFromPlayer = 6f;

    [Header("Slime gerado (quando não há prefab)")]
    [SerializeField] private Vector3 fallbackScale = new Vector3(0.5f, 0.35f, 0.5f);
    [SerializeField] private Color fallbackColor = new Color(0.3f, 0.9f, 0.35f);

    private readonly List<Slime> alive = new List<Slime>();
    private float timer;

    private void Start()
    {
        if (player == null && Camera.main != null)
            player = Camera.main.transform;

        for (int i = 0; i < spawnOnStart; i++)
            TrySpawn();
    }

    private void Update()
    {
        alive.RemoveAll(s => s == null);

        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        if (alive.Count < maxAlive)
            TrySpawn();
    }

    public bool TrySpawn()
    {
        if (alive.Count >= maxAlive) return false;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 point = transform.position + new Vector3(offset.x, 0f, offset.y);

            if (player != null)
            {
                Vector3 diff = point - player.position;
                diff.y = 0f;
                if (diff.magnitude < minDistanceFromPlayer) continue;
            }

            if (!SlimeGround.TryGetHeight(point, out float groundY)) continue;
            point.y = groundY;

            Spawn(point);
            return true;
        }

        return false;
    }

    private void Spawn(Vector3 position)
    {
        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        GameObject go = slimePrefab != null
            ? Instantiate(slimePrefab, position, rotation)
            : CreateFallbackSlime(position, rotation);

        Slime slime = go.GetComponent<Slime>();
        if (slime == null) slime = go.AddComponent<Slime>();

        slime.SetTarget(player);
        slime.Died += OnSlimeDied;
        alive.Add(slime);
    }

    private GameObject CreateFallbackSlime(Vector3 position, Quaternion rotation)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "Slime";
        go.transform.SetPositionAndRotation(position, rotation);
        go.transform.localScale = fallbackScale;

        Renderer rend = go.GetComponent<Renderer>();
        Material mat = rend.material;
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", fallbackColor);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", fallbackColor);

        return go;
    }

    private void OnSlimeDied(Slime slime)
    {
        slime.Died -= OnSlimeDied;
        alive.Remove(slime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 0.9f, 0.35f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
