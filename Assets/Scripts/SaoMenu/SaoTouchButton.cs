using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Botão do menu SAO acionado tocando com a ponta do dedo (o SaoMenu faz o teste de toque).
public class SaoTouchButton : MonoBehaviour
{
    public Action OnClick;

    [Tooltip("Teste de toque circular (botões redondos) em vez de retangular.")]
    public bool Circular;
    [Tooltip("Margem extra da área de toque, em unidades do canvas (mm).")]
    public float HitPadding = 4f;
    [Tooltip("Aumenta um pouco o botão quando o dedo está em cima.")]
    public float HoverScale = 1.06f;

    public Image Background;
    public Graphic[] Content = Array.Empty<Graphic>();

    public Color NormalColor = Color.white;
    public Color HoverColor = Color.white;
    public Color SelectedColor = SaoColors.Orange;
    public Color ContentNormal = SaoColors.Icon;
    public Color ContentSelected = Color.white;

    public bool Interactable = true;
    public bool Selected;

    [NonSerialized] public bool Hovered;

    private float flash;
    private readonly List<CanvasGroup> groups = new List<CanvasGroup>();

    public RectTransform Rect => (RectTransform)transform;

    private void Awake()
    {
        CacheGroups();
    }

    public void CacheGroups()
    {
        groups.Clear();
        GetComponentsInParent(true, groups);
    }

    // Pode receber toque agora? (ativo, habilitado e sem painel pai sendo escondido).
    public bool IsTouchable
    {
        get
        {
            if (!Interactable || !isActiveAndEnabled) return false;
            foreach (CanvasGroup g in groups)
                if (g != null && (!g.interactable || g.alpha < 0.5f)) return false;
            return true;
        }
    }

    public bool Contains(Vector3 worldPoint)
    {
        RectTransform rt = Rect;
        Vector2 local = rt.InverseTransformPoint(worldPoint);
        Rect r = rt.rect;
        if (Circular)
        {
            float radius = Mathf.Min(r.width, r.height) * 0.5f + HitPadding;
            return (local - r.center).sqrMagnitude <= radius * radius;
        }
        r.xMin -= HitPadding;
        r.xMax += HitPadding;
        r.yMin -= HitPadding;
        r.yMax += HitPadding;
        return r.Contains(local);
    }

    public void Press()
    {
        flash = 1f;
        OnClick?.Invoke();
    }

    private void LateUpdate()
    {
        float dt = Time.unscaledDeltaTime;
        flash = Mathf.MoveTowards(flash, 0f, dt * 5f);

        Color bg = !Interactable ? new Color(NormalColor.r, NormalColor.g, NormalColor.b, NormalColor.a * 0.45f)
            : Selected ? SelectedColor
            : Hovered ? HoverColor
            : NormalColor;
        bg = Color.Lerp(bg, Color.white, flash * 0.8f);
        if (Background != null) Background.color = bg;

        Color content = Selected ? ContentSelected : ContentNormal;
        if (!Interactable) content.a *= 0.45f;
        foreach (Graphic g in Content)
            if (g != null) g.color = content;

        float targetScale = Hovered && Interactable ? HoverScale : 1f;
        float s = Mathf.MoveTowards(transform.localScale.x, targetScale, dt * 1.5f);
        transform.localScale = new Vector3(s, s, 1f);
    }
}
