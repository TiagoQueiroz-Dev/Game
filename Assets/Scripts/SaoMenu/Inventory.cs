using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public ItemData item;
    [Min(1)] public int count = 1;

    public ItemStack(ItemData item, int count)
    {
        this.item = item;
        this.count = count;
    }
}

// Inventário (grade de slots) + equipamentos. Envia a soma dos bônus para o PlayerStats.
[RequireComponent(typeof(PlayerStats))]
public class Inventory : MonoBehaviour
{
    [Tooltip("Quantidade de slots da grade (o menu mostra 5x5 = 25).")]
    [SerializeField] private int capacity = 25;
    [Tooltip("Itens dados ao jogador no início.")]
    [SerializeField] private List<ItemStack> startingItems = new List<ItemStack>();
    [Tooltip("Itens já equipados no início.")]
    [SerializeField] private List<ItemData> startingEquipment = new List<ItemData>();
    [Tooltip("Se as listas acima estiverem vazias, recebe um kit de teste inspirado em SAO.")]
    [SerializeField] private bool giveDemoItemsIfEmpty = true;

    public event Action Changed;

    private ItemStack[] slots;
    private readonly Dictionary<EquipSlot, ItemData> equipped = new Dictionary<EquipSlot, ItemData>();
    private PlayerStats stats;

    public int Capacity => slots.Length;
    public PlayerStats Stats => stats;

    private void Awake()
    {
        // No APK o [RequireComponent] não criou o PlayerStats quando o Inventory é adicionado por código.
        stats = GetComponent<PlayerStats>();
        if (stats == null) stats = gameObject.AddComponent<PlayerStats>();
        slots = new ItemStack[Mathf.Max(1, capacity)];

        bool useDemo = giveDemoItemsIfEmpty && startingItems.Count == 0 && startingEquipment.Count == 0;
        if (useDemo) BuildDemoKit(startingItems, startingEquipment);

        foreach (ItemStack stack in startingItems)
            if (stack != null && stack.item != null) AddItem(stack.item, stack.count);

        foreach (ItemData item in startingEquipment)
            if (item != null && item.IsEquipment) equipped[item.equipSlot] = item;
    }

    private void Start()
    {
        RecalculateBonus();
    }

    public ItemStack GetSlot(int index) => index >= 0 && index < slots.Length ? slots[index] : null;

    public ItemData GetEquipped(EquipSlot slot) => equipped.TryGetValue(slot, out ItemData item) ? item : null;

    public int UsedSlots
    {
        get
        {
            int used = 0;
            foreach (ItemStack s in slots) if (s != null) used++;
            return used;
        }
    }

    // Adiciona itens e devolve quantos NÃO couberam.
    public int AddItem(ItemData item, int count = 1)
    {
        if (item == null || count <= 0) return 0;

        // Completa pilhas existentes.
        for (int i = 0; i < slots.Length && count > 0; i++)
        {
            ItemStack s = slots[i];
            if (s == null || s.item != item || s.count >= item.maxStack) continue;
            int add = Mathf.Min(count, item.maxStack - s.count);
            s.count += add;
            count -= add;
        }

        // Usa slots vazios.
        for (int i = 0; i < slots.Length && count > 0; i++)
        {
            if (slots[i] != null) continue;
            int add = Mathf.Min(count, item.maxStack);
            slots[i] = new ItemStack(item, add);
            count -= add;
        }

        Changed?.Invoke();
        return count;
    }

    public void RemoveAt(int index, int count = int.MaxValue)
    {
        ItemStack s = GetSlot(index);
        if (s == null) return;
        s.count -= Mathf.Max(1, count);
        if (s.count <= 0) slots[index] = null;
        Changed?.Invoke();
    }

    // Equipa o item do slot. O que estava equipado volta para o mesmo slot da grade.
    public bool Equip(int index)
    {
        ItemStack s = GetSlot(index);
        if (s == null || !s.item.IsEquipment) return false;

        ItemData item = s.item;
        ItemData previous = GetEquipped(item.equipSlot);

        if (s.count > 1)
        {
            // Pilha: tira só um. O anterior precisa de espaço livre.
            if (previous != null && FirstEmptySlot() < 0) return false;
            s.count--;
            if (previous != null) AddItem(previous);
        }
        else
        {
            slots[index] = previous != null ? new ItemStack(previous, 1) : null;
        }

        equipped[item.equipSlot] = item;
        RecalculateBonus();
        Changed?.Invoke();
        return true;
    }

    public bool Unequip(EquipSlot slot)
    {
        ItemData item = GetEquipped(slot);
        if (item == null) return false;
        if (FirstEmptySlot() < 0) return false;

        equipped.Remove(slot);
        AddItem(item);
        RecalculateBonus();
        Changed?.Invoke();
        return true;
    }

    // Usa um consumível. Devolve false se não deu (ex.: HP cheio).
    public bool Use(int index)
    {
        ItemStack s = GetSlot(index);
        if (s == null || !s.item.IsConsumable) return false;
        if (s.item.healAmount > 0 && stats.IsFullHp) return false;

        stats.Heal(s.item.healAmount);
        RemoveAt(index, 1);
        return true;
    }

    public int FirstEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
            if (slots[i] == null) return i;
        return -1;
    }

    private void RecalculateBonus()
    {
        StatBonus total = default;
        foreach (ItemData item in equipped.Values)
            if (item != null) total += item.bonus;
        stats.SetEquipmentBonus(total);
    }

    // Kit de teste com itens do 1º arco de Sword Art Online.
    private static void BuildDemoKit(List<ItemStack> items, List<ItemData> equipment)
    {
        Color steel = new Color(0.72f, 0.77f, 0.84f);
        Color black = new Color(0.22f, 0.22f, 0.26f);
        Color leather = new Color(0.62f, 0.44f, 0.28f);

        ItemData anneal = ItemData.Create("Anneal Blade",
            "Espada de uma mão, recompensa da missão \"Segredo da Floresta\" do 1º andar.",
            ItemCategory.Weapon, EquipSlot.MainHand, ItemIconShape.Sword, steel,
            new StatBonus { attack = 28 });
        ItemData coat = ItemData.Create("Coat of Midnight",
            "Sobretudo negro dropado pelo boss do 1º andar, Illfang the Kobold Lord.",
            ItemCategory.Armor, EquipSlot.Chest, ItemIconShape.Coat, black,
            new StatBonus { defense = 40, agility = 3 });

        equipment.Add(anneal);
        equipment.Add(coat);

        items.Add(new ItemStack(ItemData.Create("Elucidator",
            "Espada negra de altíssimo nível, dropada pelo boss do 50º andar.",
            ItemCategory.Weapon, EquipSlot.MainHand, ItemIconShape.Sword, new Color(0.18f, 0.18f, 0.22f),
            new StatBonus { attack = 95, strength = 5 }), 1));
        items.Add(new ItemStack(ItemData.Create("Dark Repulser",
            "Espada forjada por Lisbeth com o Crystallite Ingot. Ideal para a mão esquerda.",
            ItemCategory.Weapon, EquipSlot.OffHand, ItemIconShape.Sword, new Color(0.55f, 0.95f, 0.88f),
            new StatBonus { attack = 90, agility = 5 }), 1));
        items.Add(new ItemStack(ItemData.Create("Escudo de Madeira",
            "Escudo simples vendido na Cidade do Início.",
            ItemCategory.Armor, EquipSlot.OffHand, ItemIconShape.Shield, leather,
            new StatBonus { defense = 12 }), 1));
        items.Add(new ItemStack(ItemData.Create("Capuz de Couro",
            "Capuz leve que esconde o rosto.",
            ItemCategory.Armor, EquipSlot.Head, ItemIconShape.Helmet, black,
            new StatBonus { defense = 6 }), 1));
        items.Add(new ItemStack(ItemData.Create("Luvas de Couro",
            "Luvas de couro curtido de Frenzy Boar.",
            ItemCategory.Armor, EquipSlot.Hands, ItemIconShape.Gloves, leather,
            new StatBonus { defense = 4, strength = 1 }), 1));
        items.Add(new ItemStack(ItemData.Create("Calça de Couro",
            "Proteção básica para as pernas.",
            ItemCategory.Armor, EquipSlot.Legs, ItemIconShape.Pants, black,
            new StatBonus { defense = 8 }), 1));
        items.Add(new ItemStack(ItemData.Create("Botas de Couro",
            "Botas leves que ajudam na esquiva.",
            ItemCategory.Armor, EquipSlot.Feet, ItemIconShape.Boots, leather,
            new StatBonus { defense = 5, agility = 2 }), 1));
        items.Add(new ItemStack(ItemData.Create("Anel da Agilidade",
            "Anel encantado que aumenta a agilidade.",
            ItemCategory.Accessory, EquipSlot.Ring, ItemIconShape.Ring, new Color(0.98f, 0.8f, 0.3f),
            new StatBonus { agility = 6 }), 1));
        items.Add(new ItemStack(ItemData.Create("Colar de Prata",
            "Colar com um pequeno cristal. Aumenta o HP máximo.",
            ItemCategory.Accessory, EquipSlot.Necklace, ItemIconShape.Necklace, new Color(0.85f, 0.88f, 0.95f),
            new StatBonus { maxHp = 50, vitality = 2 }), 1));
        items.Add(new ItemStack(ItemData.Create("Poção de Cura",
            "Recupera 100 de HP aos poucos.",
            ItemCategory.Consumable, EquipSlot.None, ItemIconShape.Potion, new Color(1f, 0.42f, 0.42f),
            heal: 100, maxStack: 10), 5));
        items.Add(new ItemStack(ItemData.Create("Cristal de Cura",
            "Diga \"Heal!\" para recuperar todo o HP na hora.",
            ItemCategory.Consumable, EquipSlot.None, ItemIconShape.Crystal, new Color(1f, 0.55f, 0.85f),
            heal: 99999, maxStack: 10), 2));
        items.Add(new ItemStack(ItemData.Create("Couro de Frenzy Boar",
            "Material dropado pelos javalis do 1º andar.",
            ItemCategory.Material, EquipSlot.None, ItemIconShape.Hide, leather,
            maxStack: 99), 6));
        items.Add(new ItemStack(ItemData.Create("Lingote de Ferro",
            "Material de forja.",
            ItemCategory.Material, EquipSlot.None, ItemIconShape.Ore, new Color(0.7f, 0.72f, 0.75f),
            maxStack: 99), 3));
    }
}
