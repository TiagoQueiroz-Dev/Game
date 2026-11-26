using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

// Menu principal estilo Sword Art Online (Aincrad) para VR.
//
// Abrir/fechar: segure o GATILHO direito e deslize a mão para BAIXO (como no anime),
// ou aperte B (controle direito) / Menu (controle esquerdo). No editor, a tecla M também funciona.
// Os botões são acionados tocando com a ponta do dedo indicador, sem raio.
//
// Se não houver nenhum SaoMenu na cena, ele é adicionado sozinho no XR Origin ao dar Play.
[DisallowMultipleComponent]
public class SaoMenu : MonoBehaviour
{
    [Header("Referências (achadas automaticamente se vazias)")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [Tooltip("Ponta do indicador. Se vazio, procura o osso 'index_ignore' (ou 'index3') dentro da mão.")]
    [SerializeField] private Transform rightFingertip;
    [SerializeField] private Transform leftFingertip;
    [SerializeField] private Inventory inventory;

    [Header("Abrir / fechar")]
    [Tooltip("Mão que faz o gesto de deslizar para baixo.")]
    [SerializeField] private XRNode gestureHand = XRNode.RightHand;
    [Tooltip("Exige segurar o gatilho durante o gesto (evita abrir sem querer ao socar slimes).")]
    [SerializeField] private bool requireTrigger = true;
    [Tooltip("Quanto a mão precisa descer (m).")]
    [SerializeField] private float swipeDistance = 0.15f;
    [Tooltip("Tempo máximo para descer essa distância (s).")]
    [SerializeField] private float swipeMaxTime = 0.35f;
    [SerializeField] private bool toggleWithButtonB = true;
    [SerializeField] private bool toggleWithLeftMenuButton = true;
    [SerializeField] private bool toggleWithKeyboardM = true;

    [Header("Posição")]
    [Tooltip("Distância à frente da cabeça (m).")]
    [SerializeField] private float distance = 0.5f;
    [Tooltip("Altura em relação aos olhos (m).")]
    [SerializeField] private float heightOffset = -0.15f;
    [Tooltip("Deslocamento lateral (m). Positivo = direita.")]
    [SerializeField] private float sideOffset = 0f;
    [Tooltip("Escala geral. 1 = cada unidade do canvas vale 1 mm.")]
    [SerializeField] private float menuScale = 0.6f;

    [Header("Toque")]
    [Tooltip("Distância do dedo ao painel em que o botão já fica destacado (m).")]
    [SerializeField] private float hoverDistance = 0.06f;
    [Tooltip("Quanto o dedo precisa recuar para poder tocar de novo (m).")]
    [SerializeField] private float releaseDistance = 0.015f;
    [SerializeField] private bool showFingerCursor = true;
    [Range(0f, 1f)] [SerializeField] private float hapticAmplitude = 0.35f;
    [SerializeField] private float hapticDuration = 0.04f;

    [Header("Sons (vazio = sons gerados por código)")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip equipSound;
    [SerializeField] private AudioClip errorSound;
    [Range(0f, 1f)] [SerializeField] private float volume = 0.7f;

    // Layout (unidades do canvas = mm, antes da escala).
    private const float ButtonSize = 64f;
    private const float ButtonSpacing = 80f;
    private const float PanelGap = 52f;
    private static readonly Vector2 BarSize = new Vector2(230f, 56f);
    private const float BarLeft = 56f;
    private const float InventoryLeft = BarLeft + 230f + 40f;

    public PlayerStats Stats => stats;
    public Inventory Inventory => inventory;
    public bool IsOpen => isOpen;

    private PlayerStats stats;
    private Transform rig;

    // UI
    private RectTransform canvasRect;
    private CanvasGroup canvasGroup;
    private RectTransform content;
    private AudioSource audioSource;
    private readonly List<SaoTouchButton> mainButtons = new List<SaoTouchButton>();
    private readonly List<Vector2> mainButtonPositions = new List<Vector2>();
    private CanvasGroup submenu;
    private SaoTouchButton itemsButton;
    private SaoCharacterPanel characterPanel;
    private SaoInventoryPanel inventoryPanel;
    private SaoItemPopup popup;
    private CanvasGroup toast;
    private Text toastText;
    private float toastTimer;
    private readonly List<SaoTouchButton> buttons = new List<SaoTouchButton>();

    // Estado
    private bool built;
    private bool isOpen;
    private int selectedMain = -1;
    private bool inventoryOpen;
    private Transform modal;
    private float inputUnlockTime;
    private bool soundPlayedThisClick;

    private enum PopupSource { None, Inventory, Equipment }
    private PopupSource popupSource;
    private int popupIndex = -1;
    private EquipSlot popupSlot = EquipSlot.None;

    // Gesto
    private struct Sample { public float Time; public Vector3 Position; }
    private readonly List<Sample> samples = new List<Sample>();
    private float gestureCooldownUntil;
    private bool lastButtonB, lastMenuButton;

    // Toque
    private class Finger
    {
        public XRNode Node;
        public Transform Hand;
        public Transform Tip;
        public float NextTipSearch;
        public bool Pressed;
        public bool WasInFront;
        public SaoTouchButton Hovered;
        public Image Cursor;
    }

    private Finger right, left;
    private readonly Dictionary<UnityEngine.Object, Coroutine> tweens = new Dictionary<UnityEngine.Object, Coroutine>();

    // ======================================================================================
    // Ciclo de vida
    // ======================================================================================

    private void Awake()
    {
        XROrigin origin = GetComponentInParent<XROrigin>();
        if (origin == null) origin = FindAnyObjectByType<XROrigin>();
        rig = origin != null ? origin.transform : transform;

        if (head == null)
        {
            if (origin != null && origin.Camera != null) head = origin.Camera.transform;
            else if (Camera.main != null) head = Camera.main.transform;
        }
        if (rightHand == null) rightHand = FindChild(rig, "Right Hand");
        if (leftHand == null) leftHand = FindChild(rig, "Left Hand");

        if (inventory == null) inventory = GetComponent<Inventory>();
        if (inventory == null) inventory = FindAnyObjectByType<Inventory>();
        if (inventory == null)
        {
            // Cria o PlayerStats antes, para o Inventory já encontrá-lo no Awake.
            if (GetComponent<PlayerStats>() == null) gameObject.AddComponent<PlayerStats>();
            inventory = gameObject.AddComponent<Inventory>();
        }
        stats = inventory.GetComponent<PlayerStats>();
        if (stats == null) stats = inventory.gameObject.AddComponent<PlayerStats>();

        right = new Finger { Node = XRNode.RightHand, Hand = rightHand, Tip = rightFingertip };
        left = new Finger { Node = XRNode.LeftHand, Hand = leftHand, Tip = leftFingertip };
    }

    private void Start()
    {
        BuildUI();
        stats.Changed += OnDataChanged;
        inventory.Changed += OnDataChanged;
    }

    private void OnDestroy()
    {
        if (stats != null) stats.Changed -= OnDataChanged;
        if (inventory != null) inventory.Changed -= OnDataChanged;
        if (canvasRect != null) Destroy(canvasRect.gameObject);
    }

    private void Update()
    {
        if (!built) return;

        UpdateToggleInput();

        if (isOpen && canvasRect.gameObject.activeSelf)
        {
            UpdateFinger(right);
            UpdateFinger(left);
            foreach (SaoTouchButton b in buttons)
                b.Hovered = b == right.Hovered || b == left.Hovered;
        }

        if (toastTimer > 0f)
        {
            toastTimer -= Time.unscaledDeltaTime;
            toast.alpha = Mathf.Clamp01(toastTimer / 0.3f);
            if (toastTimer <= 0f) toast.gameObject.SetActive(false);
        }
    }

    // ======================================================================================
    // Abrir / fechar
    // ======================================================================================

    public void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }

    public void Open()
    {
        if (!built || isOpen || head == null) return;
        isOpen = true;

        PlaceInFrontOfPlayer();
        StopTween(canvasGroup);
        canvasRect.gameObject.SetActive(true);
        canvasGroup.interactable = true;
        ResetPanels();
        RefreshAll();
        inputUnlockTime = Time.unscaledTime + 0.35f;
        right.Pressed = left.Pressed = false;

        Tween(canvasGroup, 0.12f, t => canvasGroup.alpha = t);
        for (int i = 0; i < mainButtons.Count; i++)
        {
            SaoTouchButton b = mainButtons[i];
            RectTransform rt = b.Rect;
            CanvasGroup g = b.GetComponent<CanvasGroup>();
            Vector2 target = mainButtonPositions[i];
            float delay = i * 0.045f;
            Tween(g, 0.22f + delay, t =>
            {
                float k = EaseOut(Mathf.Clamp01((t * (0.22f + delay) - delay) / 0.22f));
                g.alpha = k;
                rt.anchoredPosition = target + new Vector2(0f, 60f * (1f - k));
            });
        }

        PlaySound(openSound != null ? openSound : SaoSounds.Open);
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        canvasGroup.interactable = false;
        right.Hovered = left.Hovered = null;
        if (right.Cursor != null) right.Cursor.enabled = false;
        if (left.Cursor != null) left.Cursor.enabled = false;

        float start = canvasGroup.alpha;
        Tween(canvasGroup, 0.12f, t => canvasGroup.alpha = Mathf.Lerp(start, 0f, t),
            () => canvasRect.gameObject.SetActive(false));
        PlaySound(closeSound != null ? closeSound : SaoSounds.Close);
    }

    private void PlaceInFrontOfPlayer()
    {
        Vector3 forward = head.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.001f) forward = rig.forward;
        forward.Normalize();
        Vector3 side = Vector3.Cross(Vector3.up, forward);

        Vector3 pos = head.position + forward * distance + Vector3.up * heightOffset + side * sideOffset;
        // Frente do canvas virada para os olhos (inclina junto quando fica abaixo da linha dos olhos).
        canvasRect.SetPositionAndRotation(pos, Quaternion.LookRotation(pos - head.position, Vector3.up));
    }

    private void UpdateToggleInput()
    {
        bool toggle = false;

        if (toggleWithButtonB)
        {
            bool b = ReadButton(XRNode.RightHand, CommonUsages.secondaryButton);
            if (b && !lastButtonB) toggle = true;
            lastButtonB = b;
        }
        if (toggleWithLeftMenuButton)
        {
            bool m = ReadButton(XRNode.LeftHand, CommonUsages.menuButton);
            if (m && !lastMenuButton) toggle = true;
            lastMenuButton = m;
        }
#if ENABLE_INPUT_SYSTEM
        if (toggleWithKeyboardM && UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.mKey.wasPressedThisFrame)
            toggle = true;
#endif
        if (DetectSwipeDown()) toggle = true;

        if (toggle) Toggle();
    }

    // Gesto do anime: mão desce rápido (com o gatilho seguro).
    private bool DetectSwipeDown()
    {
        Transform hand = gestureHand == XRNode.LeftHand ? leftHand : rightHand;
        if (hand == null) return false;

        bool armed = !requireTrigger || ReadButton(gestureHand, CommonUsages.triggerButton);
        if (!armed)
        {
            samples.Clear();
            return false;
        }

        float now = Time.unscaledTime;
        // Posição no espaço do rig: andar/girar com o joystick não conta como gesto.
        Vector3 p = rig.InverseTransformPoint(hand.position);
        samples.Add(new Sample { Time = now, Position = p });
        while (samples.Count > 0 && now - samples[0].Time > swipeMaxTime) samples.RemoveAt(0);
        if (now < gestureCooldownUntil) return false;

        Sample top = samples[0];
        foreach (Sample s in samples)
            if (s.Position.y > top.Position.y) top = s;

        float drop = top.Position.y - p.y;
        Vector3 drift = p - top.Position;
        drift.y = 0f;
        if (drop < swipeDistance || drift.magnitude > drop) return false;

        samples.Clear();
        gestureCooldownUntil = now + 0.6f;
        return true;
    }

    // ======================================================================================
    // Toque com a ponta do dedo
    // ======================================================================================

    private void UpdateFinger(Finger f)
    {
        f.Hovered = null;
        if (!TryGetTip(f, out Vector3 tip))
        {
            if (f.Cursor != null) f.Cursor.enabled = false;
            return;
        }

        // < 0 = dedo na frente do painel (lado do jogador), > 0 = atravessou.
        float depth = Vector3.Dot(tip - canvasRect.position, canvasRect.forward);
        if (f.Pressed && depth < -releaseDistance) f.Pressed = false;

        bool near = depth > -hoverDistance && depth < 0.1f;
        bool unlocked = Time.unscaledTime >= inputUnlockTime;
        if (near && unlocked)
        {
            f.Hovered = FindButtonAt(tip);
            if (!f.Pressed && f.WasInFront && depth >= 0f)
            {
                f.Pressed = true;
                if (f.Hovered != null) Click(f, f.Hovered);
            }
        }
        f.WasInFront = depth < 0f;

        if (f.Cursor != null)
        {
            bool show = showFingerCursor && depth > -hoverDistance && depth < 0.02f;
            f.Cursor.enabled = show;
            if (show)
            {
                Vector3 local = canvasRect.InverseTransformPoint(tip);
                f.Cursor.rectTransform.localPosition = new Vector3(local.x, local.y, 0f);
                float k = Mathf.Clamp01(-depth / hoverDistance); // 1 = longe, 0 = encostando
                float size = Mathf.Lerp(10f, 26f, k);
                f.Cursor.rectTransform.sizeDelta = new Vector2(size, size);
                Color c = f.Hovered != null ? SaoColors.Orange : Color.white;
                c.a = Mathf.Lerp(0.95f, 0.35f, k);
                f.Cursor.color = c;
            }
        }
    }

    private SaoTouchButton FindButtonAt(Vector3 worldPoint)
    {
        // De trás para frente: o que foi criado por último (popup) fica por cima.
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            SaoTouchButton b = buttons[i];
            if (modal != null && !b.transform.IsChildOf(modal)) continue;
            if (!b.IsTouchable) continue;
            if (b.Contains(worldPoint)) return b;
        }
        return null;
    }

    private void Click(Finger f, SaoTouchButton b)
    {
        soundPlayedThisClick = false;
        b.Press();
        if (!soundPlayedThisClick) PlaySound(clickSound != null ? clickSound : SaoSounds.Click);

        InputDevice device = InputDevices.GetDeviceAtXRNode(f.Node);
        if (device.isValid) device.SendHapticImpulse(0u, hapticAmplitude, hapticDuration);
    }

    private bool TryGetTip(Finger f, out Vector3 tip)
    {
        tip = default;
        if (f.Tip == null && f.Hand != null && Time.unscaledTime >= f.NextTipSearch)
        {
            f.Tip = FindFingertip(f.Hand);
            f.NextTipSearch = Time.unscaledTime + 1f;
        }

        if (f.Tip != null && f.Tip.gameObject.activeInHierarchy)
        {
            tip = f.Tip.position;
            return true;
        }
        if (f.Hand != null && f.Hand.gameObject.activeInHierarchy)
        {
            tip = f.Hand.position + f.Hand.forward * 0.08f;
            return true;
        }
        return false;
    }

    private static Transform FindFingertip(Transform hand)
    {
        Transform fallback = null;
        foreach (Transform t in hand.GetComponentsInChildren<Transform>(true))
        {
            string n = t.name.ToLowerInvariant();
            if (n.EndsWith("index_ignore") || n.EndsWith("indextip") || n.EndsWith("index_tip")) return t;
            if (fallback == null && n.EndsWith("index3")) fallback = t;
        }
        return fallback;
    }

    // ======================================================================================
    // Ações dos botões
    // ======================================================================================

    private void OnMainPressed(int index)
    {
        if (index != 0) return; // por enquanto só o botão do personagem tem conteúdo

        if (selectedMain == index)
        {
            Collapse();
            return;
        }

        selectedMain = index;
        for (int i = 0; i < mainButtons.Count; i++) mainButtons[i].Selected = i == index;
        RefreshAll();
        ShowGroup(characterPanel.Root);
        ShowGroup(submenu);
    }

    private void Collapse()
    {
        selectedMain = -1;
        foreach (SaoTouchButton b in mainButtons) b.Selected = false;
        ClosePopup();
        SetInventoryOpen(false);
        HideGroup(characterPanel.Root);
        HideGroup(submenu);
    }

    private void OnItemsPressed()
    {
        SetInventoryOpen(!inventoryOpen);
    }

    private void SetInventoryOpen(bool open)
    {
        inventoryOpen = open;
        itemsButton.Selected = open;
        if (open)
        {
            inventoryPanel.Refresh(inventory);
            ShowGroup(inventoryPanel.Root);
        }
        else
        {
            if (popupSource == PopupSource.Inventory) ClosePopup();
            HideGroup(inventoryPanel.Root);
        }
    }

    public void OnInventorySlotPressed(int index)
    {
        ItemStack s = inventory.GetSlot(index);
        if (s == null) return;

        popupSource = PopupSource.Inventory;
        popupIndex = index;
        inventoryPanel.SetSelected(index);
        characterPanel.SetSelectedSlot(EquipSlot.None);

        string action = s.item.IsEquipment ? "Equipar" : s.item.IsConsumable ? "Usar" : null;
        popup.PlaceOver((RectTransform)inventoryPanel.Root.transform);
        popup.ShowDetails(s.item, s.count, action, true);
        OpenPopup();
    }

    public void OnEquipSlotPressed(EquipSlot slot)
    {
        ItemData item = inventory.GetEquipped(slot);
        if (item == null)
        {
            Toast(ItemData.SlotLabel(slot) + ": vazio");
            return;
        }

        popupSource = PopupSource.Equipment;
        popupSlot = slot;
        characterPanel.SetSelectedSlot(slot);
        inventoryPanel.SetSelected(-1);

        popup.PlaceOver((RectTransform)characterPanel.Root.transform);
        popup.ShowDetails(item, 1, "Desequipar", false);
        OpenPopup();
    }

    public void OnPopupPrimary()
    {
        if (popupSource == PopupSource.Equipment)
        {
            ItemData item = inventory.GetEquipped(popupSlot);
            if (inventory.Unequip(popupSlot))
            {
                PlaySound(equipSound != null ? equipSound : SaoSounds.Equip);
                Toast(item.itemName + " desequipado");
                ClosePopup();
            }
            else Error("Inventário cheio");
            return;
        }

        ItemStack s = inventory.GetSlot(popupIndex);
        if (s == null)
        {
            ClosePopup();
            return;
        }
        ItemData it = s.item;

        if (it.IsEquipment)
        {
            if (inventory.Equip(popupIndex))
            {
                PlaySound(equipSound != null ? equipSound : SaoSounds.Equip);
                Toast(it.itemName + " equipado");
                ClosePopup();
            }
            else Error("Inventário cheio");
        }
        else if (it.IsConsumable)
        {
            int hpBefore = stats.CurrentHp;
            if (inventory.Use(popupIndex))
            {
                PlaySound(equipSound != null ? equipSound : SaoSounds.Equip);
                Toast("HP +" + (stats.CurrentHp - hpBefore));
                ItemStack remaining = inventory.GetSlot(popupIndex);
                if (remaining != null && remaining.item == it) popup.ShowDetails(it, remaining.count, "Usar", true);
                else ClosePopup();
            }
            else Error("O HP já está cheio");
        }
    }

    public void OnPopupDiscard()
    {
        ItemStack s = inventory.GetSlot(popupIndex);
        if (popupSource != PopupSource.Inventory || s == null) return;
        string qty = s.count > 1 ? " x" + s.count : "";
        popup.ShowConfirm("Descartar\n" + s.item.itemName + qty + "?");
    }

    public void OnPopupConfirmYes()
    {
        ItemStack s = inventory.GetSlot(popupIndex);
        if (s != null)
        {
            Toast(s.item.itemName + " descartado");
            inventory.RemoveAt(popupIndex);
        }
        ClosePopup();
    }

    public void OnPopupConfirmNo()
    {
        if (inventory.GetSlot(popupIndex) != null) OnInventorySlotPressed(popupIndex);
        else ClosePopup();
    }

    private void OpenPopup()
    {
        popup.Root.transform.SetAsLastSibling();
        modal = popup.Root.transform;
        ShowGroup(popup.Root);
    }

    public void ClosePopup()
    {
        modal = null;
        popupSource = PopupSource.None;
        inventoryPanel.SetSelected(-1);
        characterPanel.SetSelectedSlot(EquipSlot.None);
        HideGroup(popup.Root);
    }

    private void Error(string message)
    {
        PlaySound(errorSound != null ? errorSound : SaoSounds.Error);
        Toast(message);
    }

    public void Toast(string message)
    {
        toastText.text = message;
        toast.gameObject.SetActive(true);
        toast.transform.SetAsLastSibling();
        toast.alpha = 1f;
        toastTimer = 2.2f;
    }

    private void OnDataChanged()
    {
        if (isOpen) RefreshAll();
    }

    private void RefreshAll()
    {
        characterPanel.Refresh(stats, inventory);
        inventoryPanel.Refresh(inventory);
    }

    private void ResetPanels()
    {
        selectedMain = -1;
        inventoryOpen = false;
        modal = null;
        popupSource = PopupSource.None;
        foreach (SaoTouchButton b in mainButtons) b.Selected = false;
        itemsButton.Selected = false;
        inventoryPanel.SetSelected(-1);
        characterPanel.SetSelectedSlot(EquipSlot.None);
        foreach (CanvasGroup g in new[] { characterPanel.Root, inventoryPanel.Root, submenu, popup.Root, toast })
        {
            StopTween(g);
            g.alpha = 0f;
            g.gameObject.SetActive(false);
        }
        toastTimer = 0f;
    }

    // ======================================================================================
    // Construção da UI
    // ======================================================================================

    private void BuildUI()
    {
        GameObject root = new GameObject("SAO Menu", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(CanvasGroup));
        canvasRect = (RectTransform)root.transform;
        canvasRect.SetParent(rig, false);
        canvasRect.sizeDelta = new Vector2(1400f, 900f);
        canvasRect.localScale = Vector3.one * (0.001f * menuScale);

        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = head != null ? head.GetComponent<Camera>() : null;
        root.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 4f; // texto nítido em VR
        canvasGroup = root.GetComponent<CanvasGroup>();

        audioSource = root.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        // Tudo é posicionado com a coluna de botões em x = 0; o conteúdo é deslocado para ficar centralizado.
        float leftEdge = -PanelGap - SaoCharacterPanel.Width;
        float rightEdge = InventoryLeft + SaoInventoryPanel.Width;
        content = SaoUI.Rect("Content", canvasRect, new Vector2(-(leftEdge + rightEdge) * 0.5f, 0f), canvasRect.sizeDelta);

        float topY = ButtonSpacing * 2f;

        // Painéis (atrás da coluna).
        characterPanel = new SaoCharacterPanel(this, content,
            new Vector2(-PanelGap - SaoCharacterPanel.Width * 0.5f, 0f), topY);
        Vector2 invPos = new Vector2(InventoryLeft + SaoInventoryPanel.Width * 0.5f, 5f);
        inventoryPanel = new SaoInventoryPanel(this, content, invPos, topY - invPos.y, inventory.Capacity);

        // Coluna de botões redondos (imagem 2).
        SaoMenuIcon[] icons = { SaoMenuIcon.Person, SaoMenuIcon.Party, SaoMenuIcon.Message, SaoMenuIcon.Map, SaoMenuIcon.Settings };
        for (int i = 0; i < icons.Length; i++)
        {
            int index = i;
            Vector2 pos = new Vector2(0f, topY - i * ButtonSpacing);
            SaoTouchButton b = SaoUI.CircleButton(icons[i] + " Button", content, pos, ButtonSize,
                SaoArt.MenuIcon(icons[i]), 0.6f, SaoColors.Bubble, SaoColors.BubbleHover, SaoColors.Orange,
                SaoColors.Icon, Color.white);
            b.gameObject.AddComponent<CanvasGroup>();
            b.Interactable = i == 0;
            b.OnClick = () => OnMainPressed(index);
            mainButtons.Add(b);
            mainButtonPositions.Add(pos);
        }

        // Submenu do personagem: barra "Itens".
        submenu = SaoUI.Group("Submenu", content, new Vector2(BarLeft + BarSize.x * 0.5f, topY), BarSize);
        itemsButton = SaoUI.BarButton("Itens Button", submenu.transform, Vector2.zero, BarSize, "Itens", 26,
            SaoColors.Bubble, SaoColors.BubbleHover, SaoColors.Orange, SaoColors.Text, Color.white);
        itemsButton.HoverScale = 1.03f;
        ((Text)itemsButton.Content[0]).rectTransform.anchoredPosition = new Vector2(24f, 0f);
        Image bagBg = SaoUI.Image("Bag Bg", itemsButton.transform, SaoArt.Circle, Color.white,
            new Vector2(-BarSize.x * 0.5f + 34f, 0f), new Vector2(44, 44));
        SaoUI.Image("Bag", bagBg.transform, SaoArt.MenuIcon(SaoMenuIcon.Bag), SaoColors.Orange, Vector2.zero, new Vector2(28, 28));
        itemsButton.OnClick = OnItemsPressed;

        // Popup e aviso (por cima de tudo).
        popup = new SaoItemPopup(this, content);
        // Aviso centralizado embaixo de tudo.
        toast = SaoUI.Group("Toast", content, new Vector2((leftEdge + rightEdge) * 0.5f, -345f), new Vector2(420, 50));
        SaoUI.Panel("Background", toast.transform, new Color(1f, 1f, 1f, 0.95f), Vector2.zero, new Vector2(420, 50));
        toastText = SaoUI.Label("Text", toast.transform, "", 22, SaoColors.Text, TextAnchor.MiddleCenter, Vector2.zero, new Vector2(400, 44));

        // Cursores da ponta dos dedos.
        right.Cursor = CreateCursor("Right Finger Cursor");
        left.Cursor = CreateCursor("Left Finger Cursor");

        root.GetComponentsInChildren(true, buttons);
        foreach (SaoTouchButton b in buttons) b.CacheGroups();

        SetLayerRecursively(root, 0);
        ResetPanels();
        root.SetActive(false);
        built = true;
    }

    private Image CreateCursor(string name)
    {
        Image img = SaoUI.Image(name, canvasRect, SaoArt.Circle, Color.white, Vector2.zero, new Vector2(16, 16));
        img.enabled = false;
        return img;
    }

    // ======================================================================================
    // Animações / utilitários
    // ======================================================================================

    private void ShowGroup(CanvasGroup g)
    {
        g.gameObject.SetActive(true);
        g.interactable = true;
        Transform t = g.transform;
        float startAlpha = g.alpha;
        Tween(g, 0.16f, k =>
        {
            float e = EaseOut(k);
            g.alpha = Mathf.Lerp(startAlpha, 1f, e);
            t.localScale = new Vector3(Mathf.Lerp(0.75f, 1f, e), 1f, 1f);
        });
    }

    private void HideGroup(CanvasGroup g)
    {
        if (!g.gameObject.activeSelf) return;
        g.interactable = false;
        float startAlpha = g.alpha;
        Tween(g, 0.1f, k => g.alpha = Mathf.Lerp(startAlpha, 0f, k), () => g.gameObject.SetActive(false));
    }

    private void Tween(UnityEngine.Object key, float duration, Action<float> step, Action done = null)
    {
        StopTween(key);
        tweens[key] = StartCoroutine(TweenRoutine(key, duration, step, done));
    }

    private void StopTween(UnityEngine.Object key)
    {
        if (tweens.TryGetValue(key, out Coroutine c) && c != null) StopCoroutine(c);
        tweens.Remove(key);
    }

    private IEnumerator TweenRoutine(UnityEngine.Object key, float duration, Action<float> step, Action done)
    {
        float t = 0f;
        step(0f);
        while (t < duration)
        {
            yield return null;
            t += Time.unscaledDeltaTime;
            step(Mathf.Clamp01(t / duration));
        }
        tweens.Remove(key);
        done?.Invoke();
    }

    private static float EaseOut(float t) => 1f - Mathf.Pow(1f - t, 3f);

    private void PlaySound(AudioClip clip)
    {
        soundPlayedThisClick = true;
        if (clip != null && audioSource != null && audioSource.isActiveAndEnabled)
            audioSource.PlayOneShot(clip, volume);
    }

    private static bool ReadButton(XRNode node, InputFeatureUsage<bool> usage)
    {
        InputDevice d = InputDevices.GetDeviceAtXRNode(node);
        return d.isValid && d.TryGetFeatureValue(usage, out bool value) && value;
    }

    private static Transform FindChild(Transform root, string name)
    {
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            if (t.name == name) return t;
        return null;
    }

    private static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform) SetLayerRecursively(child.gameObject, layer);
    }
}

// Adiciona o menu automaticamente no XR Origin se a cena não tiver um SaoMenu.
internal static class SaoMenuBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        if (UnityEngine.Object.FindAnyObjectByType<SaoMenu>() != null) return;
        XROrigin origin = UnityEngine.Object.FindAnyObjectByType<XROrigin>();
        if (origin != null) origin.gameObject.AddComponent<SaoMenu>();
    }
}
