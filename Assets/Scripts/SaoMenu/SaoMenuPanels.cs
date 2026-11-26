using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Painel da esquerda: nome, silhueta com os equipamentos em volta e status.
public class SaoCharacterPanel
{
    public const float Width = 340f;
    public const float Height = 600f;

    public readonly CanvasGroup Root;

    private readonly SaoMenu menu;
    private readonly Text nameText;
    private readonly Dictionary<EquipSlot, SlotView> slots = new Dictionary<EquipSlot, SlotView>();
    private readonly Text lv, hp, exp, atk, def, str, agi, vit, col;

    private struct SlotView
    {
        public SaoTouchButton Button;
        public Image Fill;
        public Image Icon;
    }

    // Ângulo (graus) de cada slot na elipse em volta da silhueta.
    private static readonly (EquipSlot slot, float angle)[] Layout =
    {
        (EquipSlot.Head, 90f), (EquipSlot.Chest, 130f), (EquipSlot.OffHand, 170f), (EquipSlot.Hands, 210f),
        (EquipSlot.Legs, 250f), (EquipSlot.Feet, 290f), (EquipSlot.Ring, 330f), (EquipSlot.MainHand, 10f),
        (EquipSlot.Necklace, 50f),
    };

    public SaoCharacterPanel(SaoMenu menu, Transform parent, Vector2 position, float arrowY)
    {
        this.menu = menu;
        Root = SaoUI.Group("Character Panel", parent, position, new Vector2(Width, Height));
        Transform t = Root.transform;

        // Fundo branco + parte de baixo cinza (status).
        SaoUI.Panel("Background", t, SaoColors.Panel, Vector2.zero, new Vector2(Width, Height));
        SaoUI.Panel("Stats Background", t, SaoColors.PanelGrey, new Vector2(0, -180), new Vector2(Width, 240));
        SaoUI.Image("Stats Top", t, null, SaoColors.PanelGrey, new Vector2(0, -80), new Vector2(Width, 40));
        Image notch = SaoUI.Image("Notch", t, SaoArt.Triangle, SaoColors.Panel, new Vector2(-120, -66), new Vector2(18, 22));
        notch.rectTransform.localEulerAngles = new Vector3(0, 0, -90);

        // Setinha apontando para o botão do menu.
        SaoUI.Image("Arrow", t, SaoArt.Triangle, SaoColors.Panel, new Vector2(Width * 0.5f + 13f, arrowY), new Vector2(28, 38));

        nameText = SaoUI.Label("Name", t, "", 34, SaoColors.Text, TextAnchor.MiddleCenter, new Vector2(0, 262), new Vector2(300, 46));
        SaoUI.Line(t, new Vector2(0, 236), 290f);

        // Silhueta + slots.
        Vector2 center = new Vector2(0, 80);
        SaoUI.Image("Silhouette", t, SaoArt.Silhouette, SaoColors.Silhouette, center, new Vector2(100, 200));
        SaoUI.Image("Ground", t, SaoArt.Circle, new Color(0.4f, 0.4f, 0.42f, 0.35f), center + new Vector2(0, -104), new Vector2(90, 12));

        foreach ((EquipSlot slot, float angle) in Layout)
        {
            float a = angle * Mathf.Deg2Rad;
            Vector2 pos = center + new Vector2(Mathf.Cos(a) * 122f, Mathf.Sin(a) * 120f);
            slots[slot] = CreateSlot(t, slot, pos);
        }

        // Status em duas colunas, com linhas embaixo (igual ao print).
        const float leftX = -80f, rightX = 80f, topY = -96f, rowH = 38f;
        lv = Stat(t, leftX, topY);
        hp = Stat(t, leftX, topY - rowH);
        exp = Stat(t, leftX, topY - rowH * 2);
        atk = Stat(t, leftX, topY - rowH * 3);
        def = Stat(t, leftX, topY - rowH * 4);
        str = Stat(t, rightX, topY);
        agi = Stat(t, rightX, topY - rowH);
        vit = Stat(t, rightX, topY - rowH * 2);
        Stat(t, rightX, topY - rowH * 4).text = "";
        col = Stat(t, rightX, topY - rowH * 3);
        SaoUI.Image("Coin", t, SaoArt.MenuIcon(SaoMenuIcon.Coin), new Color(0.98f, 0.78f, 0.25f),
            new Vector2(rightX - 58f, topY - rowH * 3), new Vector2(20, 20));
        col.rectTransform.anchoredPosition += new Vector2(26f, 0f);
    }

    private SlotView CreateSlot(Transform parent, EquipSlot slot, Vector2 pos)
    {
        const float d = 54f;
        Image ring = SaoUI.Image(slot + " Slot", parent, SaoArt.SlotRing, Color.white, pos, new Vector2(d, d));
        Image fill = SaoUI.Image("Fill", ring.transform, SaoArt.Circle, SaoColors.SlotDark, Vector2.zero, new Vector2(d * 0.74f, d * 0.74f));
        Image icon = SaoUI.Image("Icon", ring.transform, null, Color.white, Vector2.zero, new Vector2(d * 0.56f, d * 0.56f));

        SaoTouchButton b = ring.gameObject.AddComponent<SaoTouchButton>();
        b.Circular = true;
        b.Background = ring;
        b.NormalColor = Color.white;
        b.HoverColor = SaoColors.OrangeLight;
        b.SelectedColor = SaoColors.Orange;
        b.HoverScale = 1.1f;
        b.OnClick = () => menu.OnEquipSlotPressed(slot);

        return new SlotView { Button = b, Fill = fill, Icon = icon };
    }

    private static Text Stat(Transform parent, float x, float y)
    {
        SaoUI.Line(parent, new Vector2(x, y - 16f), 140f, 1.5f);
        return SaoUI.Label("Stat", parent, "", 22, SaoColors.Text, TextAnchor.MiddleLeft, new Vector2(x, y), new Vector2(140, 30));
    }

    public void SetSelectedSlot(EquipSlot selected)
    {
        foreach (var pair in slots) pair.Value.Button.Selected = pair.Key == selected;
    }

    public void Refresh(PlayerStats s, Inventory inv)
    {
        nameText.text = s.PlayerName;
        lv.text = "Lv: " + s.Level;
        hp.text = "HP: " + s.CurrentHp + "/" + s.MaxHp;
        exp.text = "EXP: " + s.Experience + "/" + s.ExpToNextLevel;
        atk.text = "ATK: " + s.Attack;
        def.text = "DEF: " + s.Defense;
        str.text = "STR: " + s.Strength;
        agi.text = "AGI: " + s.Agility;
        vit.text = "VIT: " + s.Vitality;
        col.text = s.Col + " Col";

        foreach (var pair in slots)
        {
            ItemData item = inv.GetEquipped(pair.Key);
            SlotView v = pair.Value;
            v.Fill.color = item != null ? SaoColors.SlotFilled : SaoColors.SlotDark;
            v.Icon.enabled = item != null;
            if (item != null)
            {
                v.Icon.sprite = SaoArt.ItemIcon(item);
                v.Icon.color = item.icon != null ? Color.white : item.iconColor;
            }
        }
    }
}

// Painel da direita: "ITENS" com a grade 5x5.
public class SaoInventoryPanel
{
    public const float Width = 360f;
    public const float Height = 450f;
    private const int Columns = 5;
    private const float Cell = 64f;
    private const float SlotSize = 54f;

    public readonly CanvasGroup Root;

    private readonly SaoMenu menu;
    private readonly List<SlotView> slots = new List<SlotView>();
    private readonly Text footer;

    private struct SlotView
    {
        public SaoTouchButton Button;
        public Image Icon;
        public Text Count;
    }

    public SaoInventoryPanel(SaoMenu menu, Transform parent, Vector2 position, float arrowY, int capacity)
    {
        this.menu = menu;
        Root = SaoUI.Group("Inventory Panel", parent, position, new Vector2(Width, Height));
        Transform t = Root.transform;

        SaoUI.Panel("Background", t, SaoColors.Panel, Vector2.zero, new Vector2(Width, Height));
        Image arrow = SaoUI.Image("Arrow", t, SaoArt.Triangle, SaoColors.Panel, new Vector2(-Width * 0.5f - 13f, arrowY), new Vector2(28, 38));
        arrow.rectTransform.localEulerAngles = new Vector3(0, 0, 180);

        SaoUI.Label("Title", t, "ITENS", 30, SaoColors.Text, TextAnchor.MiddleCenter, new Vector2(0, 190), new Vector2(300, 40));
        SaoUI.Line(t, new Vector2(0, 165), 300f);

        int rows = Mathf.CeilToInt(capacity / (float)Columns);
        for (int i = 0; i < capacity; i++)
        {
            int c = i % Columns, r = i / Columns;
            Vector2 pos = new Vector2((c - (Columns - 1) * 0.5f) * Cell, 110f - r * Cell);
            slots.Add(CreateSlot(t, i, pos));
        }

        footer = SaoUI.Label("Footer", t, "", 20, SaoColors.TextSoft, TextAnchor.MiddleCenter,
            new Vector2(0, 110f - rows * Cell - 2f), new Vector2(300, 28));
    }

    private SlotView CreateSlot(Transform parent, int index, Vector2 pos)
    {
        SaoTouchButton b = SaoUI.CircleButton("Slot " + index, parent, pos, SlotSize, null, 0f,
            SaoColors.SlotGrey, SaoColors.SlotGreyHover, SaoColors.Orange, Color.white, Color.white);
        b.HoverScale = 1.1f;
        b.OnClick = () => menu.OnInventorySlotPressed(index);

        Image icon = SaoUI.Image("Icon", b.transform, null, Color.white, Vector2.zero, new Vector2(SlotSize * 0.68f, SlotSize * 0.68f));
        Text count = SaoUI.Label("Count", b.transform, "", 16, Color.white, TextAnchor.LowerRight,
            new Vector2(4, -4), new Vector2(SlotSize, SlotSize), FontStyle.Bold);
        Shadow shadow = count.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.7f);
        shadow.effectDistance = new Vector2(1.2f, -1.2f);

        return new SlotView { Button = b, Icon = icon, Count = count };
    }

    public void SetSelected(int index)
    {
        for (int i = 0; i < slots.Count; i++) slots[i].Button.Selected = i == index;
    }

    public void Refresh(Inventory inv)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            ItemStack s = inv.GetSlot(i);
            SlotView v = slots[i];
            v.Icon.enabled = s != null;
            v.Count.text = s != null && s.count > 1 ? s.count.ToString() : "";
            if (s != null)
            {
                v.Icon.sprite = SaoArt.ItemIcon(s.item);
                v.Icon.color = s.item.icon != null ? Color.white : s.item.iconColor;
            }
        }
        footer.text = inv.UsedSlots + "/" + inv.Capacity;
    }
}

// Janela de detalhes do item (modal), com Equipar/Usar/Descartar e a confirmação O / X do anime.
public class SaoItemPopup
{
    public const float Width = 330f;
    public const float Height = 340f;

    public readonly CanvasGroup Root;

    private readonly Image dim;
    private readonly RectTransform window;
    private readonly Image iconFill, icon;
    private readonly Text title, subtitle, description, bonus, question;
    private readonly GameObject detailsGroup, confirmGroup;
    private readonly SaoTouchButton primary, discard, close, yes, no;
    private readonly Text primaryText;

    public SaoItemPopup(SaoMenu menu, Transform parent)
    {
        Root = SaoUI.Group("Item Popup", parent, Vector2.zero, new Vector2(Width, Height));
        Transform t = Root.transform;

        dim = SaoUI.Panel("Dim", t, new Color(0.1f, 0.1f, 0.12f, 0.35f), Vector2.zero, new Vector2(Width, Height));
        window = SaoUI.Panel("Window", t, new Color(1f, 1f, 1f, 0.98f), Vector2.zero, new Vector2(Width, Height)).rectTransform;
        Transform w = window;

        // ---- Detalhes ----
        detailsGroup = SaoUI.Rect("Details", w, Vector2.zero, new Vector2(Width, Height)).gameObject;
        Transform d = detailsGroup.transform;
        iconFill = SaoUI.Image("Icon Fill", d, SaoArt.Circle, SaoColors.SlotGrey, new Vector2(-112, 112), new Vector2(72, 72));
        icon = SaoUI.Image("Icon", d, null, Color.white, new Vector2(-112, 112), new Vector2(50, 50));
        title = SaoUI.Label("Title", d, "", 24, SaoColors.Text, TextAnchor.MiddleLeft, new Vector2(40, 124), new Vector2(220, 34), FontStyle.Bold);
        subtitle = SaoUI.Label("Subtitle", d, "", 17, SaoColors.TextSoft, TextAnchor.MiddleLeft, new Vector2(40, 96), new Vector2(220, 26));
        SaoUI.Line(d, new Vector2(0, 66), 290f);
        description = SaoUI.Label("Description", d, "", 18, SaoColors.Text, TextAnchor.UpperLeft, new Vector2(0, 18), new Vector2(290, 80), wrap: true);
        bonus = SaoUI.Label("Bonus", d, "", 19, SaoColors.Orange, TextAnchor.MiddleLeft, new Vector2(0, -50), new Vector2(290, 30), FontStyle.Bold);

        primary = SaoUI.BarButton("Primary", d, new Vector2(-102, -122), new Vector2(98, 48), "Equipar", 19,
            SaoColors.Orange, SaoColors.OrangeLight, SaoColors.Orange, Color.white, Color.white);
        primaryText = (Text)primary.Content[0];
        discard = SaoUI.BarButton("Discard", d, new Vector2(0, -122), new Vector2(98, 48), "Descartar", 19,
            SaoColors.ButtonGrey, SaoColors.OrangeLight, SaoColors.Orange, SaoColors.Text, Color.white);
        close = SaoUI.BarButton("Close", d, new Vector2(102, -122), new Vector2(98, 48), "Fechar", 19,
            SaoColors.ButtonGrey, SaoColors.OrangeLight, SaoColors.Orange, SaoColors.Text, Color.white);

        primary.OnClick = menu.OnPopupPrimary;
        discard.OnClick = menu.OnPopupDiscard;
        close.OnClick = menu.ClosePopup;

        // ---- Confirmação (O azul / X vermelho, igual ao anime) ----
        confirmGroup = SaoUI.Rect("Confirm", w, Vector2.zero, new Vector2(Width, Height)).gameObject;
        Transform c = confirmGroup.transform;
        question = SaoUI.Label("Question", c, "", 22, SaoColors.Text, TextAnchor.MiddleCenter, new Vector2(0, 60), new Vector2(290, 120), wrap: true);
        yes = SaoUI.CircleButton("Yes", c, new Vector2(-62, -80), 76, SaoArt.MenuIcon(SaoMenuIcon.Confirm), 0.6f,
            SaoColors.Blue, new Color(0.45f, 0.7f, 1f), SaoColors.Blue, Color.white, Color.white);
        no = SaoUI.CircleButton("No", c, new Vector2(62, -80), 76, SaoArt.MenuIcon(SaoMenuIcon.Close), 0.5f,
            SaoColors.Red, new Color(1f, 0.5f, 0.5f), SaoColors.Red, Color.white, Color.white);
        yes.OnClick = menu.OnPopupConfirmYes;
        no.OnClick = menu.OnPopupConfirmNo;

        Root.gameObject.SetActive(false);
    }

    // Posiciona sobre um painel (cobre o painel com o "dim").
    public void PlaceOver(RectTransform panel)
    {
        RectTransform rt = (RectTransform)Root.transform;
        rt.anchoredPosition = panel.anchoredPosition;
        dim.rectTransform.sizeDelta = panel.sizeDelta;
        window.anchoredPosition = Vector2.zero;
    }

    public void ShowDetails(ItemData item, int count, string primaryLabel, bool canDiscard)
    {
        detailsGroup.SetActive(true);
        confirmGroup.SetActive(false);

        icon.sprite = SaoArt.ItemIcon(item);
        icon.color = item.icon != null ? Color.white : item.iconColor;
        iconFill.color = SaoColors.SlotFilled;
        title.text = count > 1 ? item.itemName + "  x" + count : item.itemName;
        subtitle.text = item.CategoryLabel;
        description.text = item.description;

        string b = item.bonus.Describe();
        if (item.healAmount > 0) b = item.healAmount >= 9999 ? "Recupera todo o HP" : "HP +" + item.healAmount;
        bonus.text = b;

        primary.gameObject.SetActive(!string.IsNullOrEmpty(primaryLabel));
        primaryText.text = primaryLabel ?? "";
        discard.gameObject.SetActive(canDiscard);
        foreach (SaoTouchButton btn in new[] { primary, discard, close, yes, no }) btn.Hovered = false;
    }

    public void ShowConfirm(string text)
    {
        detailsGroup.SetActive(false);
        confirmGroup.SetActive(true);
        question.text = text;
    }
}
