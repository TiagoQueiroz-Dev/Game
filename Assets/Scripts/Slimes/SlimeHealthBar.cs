using UnityEngine;
using UnityEngine.UI;

// Barra de vida em cima da cabeça do slime, sempre virada para o jogador.
// A barra é um objeto separado (não é filho do slime) para não ser esticada/achatada pelos pulos.
[RequireComponent(typeof(Slime))]
public class SlimeHealthBar : MonoBehaviour
{
    [Header("Tamanho e posição")]
    [Tooltip("Largura e altura da barra, em metros.")]
    [SerializeField] private Vector2 size = new Vector2(0.4f, 0.06f);
    [Tooltip("Distância acima do topo do slime.")]
    [SerializeField] private float heightAboveHead = 0.15f;

    [Header("Cores")]
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.75f);
    [SerializeField] private Color fullColor = new Color(0.2f, 0.9f, 0.25f);
    [SerializeField] private Color emptyColor = new Color(0.9f, 0.15f, 0.15f);

    [Header("Animação")]
    [Tooltip("Velocidade com que a barra desce até a vida atual.")]
    [SerializeField] private float drainSpeed = 3f;

    private const float PixelsPerMeter = 100f;

    private Slime slime;
    private Renderer body;
    private Transform bar;
    private RectTransform fill;
    private Image fillImage;
    private float shownFraction = 1f;

    private void Start()
    {
        slime = GetComponent<Slime>();
        body = GetComponentInChildren<Renderer>();
        CreateBar();
        UpdateBar(true);
    }

    private void LateUpdate()
    {
        if (bar == null) return;

        if (slime.IsDead)
        {
            Destroy(bar.gameObject);
            bar = null;
            return;
        }

        UpdateBar(false);
    }

    private void UpdateBar(bool instant)
    {
        float target = (float)slime.CurrentHealth / Mathf.Max(1, slime.MaxHealth);
        shownFraction = instant ? target : Mathf.MoveTowards(shownFraction, target, drainSpeed * Time.deltaTime);

        fill.anchorMax = new Vector2(shownFraction, 1f);
        fillImage.color = Color.Lerp(emptyColor, fullColor, shownFraction);

        // Fica sempre acima do topo do corpo, acompanhando os pulos.
        float top = body != null ? body.bounds.max.y : transform.position.y;
        Vector3 pos = transform.position;
        bar.position = new Vector3(pos.x, top + heightAboveHead, pos.z);

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 away = bar.position - cam.transform.position;
            if (away.sqrMagnitude > 0.0001f)
                bar.rotation = Quaternion.LookRotation(away, Vector3.up);
        }
    }

    private void CreateBar()
    {
        GameObject root = new GameObject(name + " HealthBar", typeof(RectTransform), typeof(Canvas));
        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        RectTransform rootRect = (RectTransform)root.transform;
        rootRect.sizeDelta = size * PixelsPerMeter;
        rootRect.localScale = Vector3.one / PixelsPerMeter;
        bar = root.transform;

        CreateImage("Background", rootRect, backgroundColor);
        fillImage = CreateImage("Fill", rootRect, fullColor);
        fill = fillImage.rectTransform;
    }

    private static Image CreateImage(string objectName, RectTransform parent, Color color)
    {
        GameObject go = new GameObject(objectName, typeof(RectTransform), typeof(Image));
        RectTransform rect = (RectTransform)go.transform;
        rect.SetParent(parent, false);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = go.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    private void OnDestroy()
    {
        if (bar != null) Destroy(bar.gameObject);
    }
}
