using UnityEngine;
using UnityEngine.UI;

// Paleta do menu de Sword Art Online (Aincrad).
public static class SaoColors
{
    public static readonly Color Panel = new Color(0.97f, 0.97f, 0.97f, 0.95f);
    public static readonly Color PanelGrey = new Color(0.84f, 0.84f, 0.85f, 0.96f);
    public static readonly Color Text = new Color(0.27f, 0.27f, 0.29f);
    public static readonly Color TextSoft = new Color(0.5f, 0.5f, 0.53f);
    public static readonly Color Line = new Color(0.42f, 0.42f, 0.45f, 0.9f);
    public static readonly Color Orange = new Color(0.96f, 0.65f, 0.1f);
    public static readonly Color OrangeLight = new Color(1f, 0.8f, 0.42f);
    public static readonly Color Bubble = new Color(1f, 1f, 1f, 0.7f);
    public static readonly Color BubbleHover = new Color(1f, 1f, 1f, 0.97f);
    public static readonly Color Icon = new Color(0.42f, 0.42f, 0.45f);
    public static readonly Color Silhouette = new Color(0.42f, 0.42f, 0.45f);
    public static readonly Color SlotGrey = new Color(0.56f, 0.56f, 0.58f);
    public static readonly Color SlotGreyHover = new Color(0.7f, 0.7f, 0.72f);
    public static readonly Color SlotDark = new Color(0.3f, 0.3f, 0.32f);
    public static readonly Color SlotFilled = new Color(0.86f, 0.83f, 0.78f);
    public static readonly Color ButtonGrey = new Color(0.9f, 0.9f, 0.91f);
    public static readonly Color Blue = new Color(0.22f, 0.55f, 0.95f);
    public static readonly Color Red = new Color(0.92f, 0.27f, 0.27f);
}

// Atalhos para montar a UI por código. Unidades do canvas = milímetros (antes da escala do menu).
public static class SaoUI
{
    private static Font font;

    public static Font Font
    {
        get
        {
            if (font == null) font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            return font;
        }
    }

    public static RectTransform Rect(string name, Transform parent, Vector2 pos, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.layer = parent != null ? parent.gameObject.layer : 0;
        RectTransform rt = (RectTransform)go.transform;
        rt.SetParent(parent, false);
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        return rt;
    }

    public static CanvasGroup Group(string name, Transform parent, Vector2 pos, Vector2 size)
    {
        RectTransform rt = Rect(name, parent, pos, size);
        return rt.gameObject.AddComponent<CanvasGroup>();
    }

    public static Image Image(string name, Transform parent, Sprite sprite, Color color, Vector2 pos, Vector2 size)
    {
        RectTransform rt = Rect(name, parent, pos, size);
        Image img = rt.gameObject.AddComponent<Image>();
        img.sprite = sprite;
        img.color = color;
        img.raycastTarget = false;
        img.preserveAspect = false;
        return img;
    }

    public static Image Panel(string name, Transform parent, Color color, Vector2 pos, Vector2 size)
    {
        Image img = Image(name, parent, SaoArt.RoundedRect, color, pos, size);
        img.type = UnityEngine.UI.Image.Type.Sliced;
        return img;
    }

    public static Image Line(Transform parent, Vector2 pos, float width, float thickness = 2f)
    {
        return Image("Line", parent, null, SaoColors.Line, pos, new Vector2(width, thickness));
    }

    public static Text Label(string name, Transform parent, string text, int fontSize, Color color,
        TextAnchor anchor, Vector2 pos, Vector2 size, FontStyle style = FontStyle.Normal, bool wrap = false)
    {
        RectTransform rt = Rect(name, parent, pos, size);
        Text t = rt.gameObject.AddComponent<Text>();
        t.font = Font;
        t.text = text;
        t.fontSize = fontSize;
        t.fontStyle = style;
        t.color = color;
        t.alignment = anchor;
        t.horizontalOverflow = wrap ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        t.raycastTarget = false;
        return t;
    }

    public static SaoTouchButton CircleButton(string name, Transform parent, Vector2 pos, float diameter,
        Sprite icon, float iconFraction, Color normal, Color hover, Color selected, Color iconNormal, Color iconSelected)
    {
        Image bg = Image(name, parent, SaoArt.Circle, normal, pos, new Vector2(diameter, diameter));
        SaoTouchButton b = bg.gameObject.AddComponent<SaoTouchButton>();
        b.Circular = true;
        b.Background = bg;
        b.NormalColor = normal;
        b.HoverColor = hover;
        b.SelectedColor = selected;
        b.ContentNormal = iconNormal;
        b.ContentSelected = iconSelected;

        if (icon != null)
        {
            float s = diameter * iconFraction;
            Image img = Image("Icon", bg.transform, icon, iconNormal, Vector2.zero, new Vector2(s, s));
            b.Content = new Graphic[] { img };
        }
        return b;
    }

    public static SaoTouchButton BarButton(string name, Transform parent, Vector2 pos, Vector2 size, string text,
        int fontSize, Color normal, Color hover, Color selected, Color textNormal, Color textSelected)
    {
        Image bg = Panel(name, parent, normal, pos, size);
        SaoTouchButton b = bg.gameObject.AddComponent<SaoTouchButton>();
        b.Background = bg;
        b.NormalColor = normal;
        b.HoverColor = hover;
        b.SelectedColor = selected;
        b.ContentNormal = textNormal;
        b.ContentSelected = textSelected;
        b.HoverScale = 1.03f;

        Text label = Label("Text", bg.transform, text, fontSize, textNormal, TextAnchor.MiddleCenter, Vector2.zero, size);
        b.Content = new Graphic[] { label };
        return b;
    }
}
