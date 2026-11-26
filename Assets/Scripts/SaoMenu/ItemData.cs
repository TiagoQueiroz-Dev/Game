using System;
using UnityEngine;

public enum ItemCategory { Weapon, Armor, Accessory, Consumable, Material }

// Slots em volta da silhueta do personagem (estilo SAO).
public enum EquipSlot { None = -1, Head, Necklace, Chest, MainHand, OffHand, Hands, Ring, Legs, Feet }

// Forma usada para desenhar o ícone automaticamente quando o item não tem Sprite.
public enum ItemIconShape { Auto, Sword, Dagger, Shield, Helmet, Coat, Gloves, Pants, Boots, Ring, Necklace, Potion, Crystal, Hide, Ore }

[Serializable]
public struct StatBonus
{
    public int maxHp;
    public int attack;
    public int defense;
    public int strength;
    public int agility;
    public int vitality;

    public static StatBonus operator +(StatBonus a, StatBonus b)
    {
        return new StatBonus
        {
            maxHp = a.maxHp + b.maxHp,
            attack = a.attack + b.attack,
            defense = a.defense + b.defense,
            strength = a.strength + b.strength,
            agility = a.agility + b.agility,
            vitality = a.vitality + b.vitality,
        };
    }

    // Texto curto tipo "ATK +28   AGI +3".
    public string Describe()
    {
        var sb = new System.Text.StringBuilder();
        Append(sb, "HP", maxHp);
        Append(sb, "ATK", attack);
        Append(sb, "DEF", defense);
        Append(sb, "STR", strength);
        Append(sb, "AGI", agility);
        Append(sb, "VIT", vitality);
        return sb.ToString();
    }

    private static void Append(System.Text.StringBuilder sb, string label, int value)
    {
        if (value == 0) return;
        if (sb.Length > 0) sb.Append("   ");
        sb.Append(label).Append(value > 0 ? " +" : " ").Append(value);
    }
}

[CreateAssetMenu(menuName = "SAO/Item", fileName = "NovoItem")]
public class ItemData : ScriptableObject
{
    [Header("Identificação")]
    public string itemName = "Item";
    [TextArea(2, 4)] public string description;
    public ItemCategory category = ItemCategory.Material;
    [Tooltip("Slot de equipamento. Deixe None para itens que não se equipam.")]
    public EquipSlot equipSlot = EquipSlot.None;

    [Header("Ícone")]
    [Tooltip("Se vazio, o ícone é desenhado automaticamente com a forma e a cor abaixo.")]
    public Sprite icon;
    public ItemIconShape iconShape = ItemIconShape.Auto;
    public Color iconColor = new Color(0.78f, 0.8f, 0.84f);

    [Header("Status")]
    public StatBonus bonus;
    [Tooltip("HP recuperado ao usar (só consumíveis).")]
    public int healAmount;

    [Header("Pilha")]
    [Min(1)] public int maxStack = 1;

    public bool IsEquipment => equipSlot != EquipSlot.None;
    public bool IsConsumable => category == ItemCategory.Consumable;

    public ItemIconShape ResolvedShape
    {
        get
        {
            if (iconShape != ItemIconShape.Auto) return iconShape;
            switch (equipSlot)
            {
                case EquipSlot.Head: return ItemIconShape.Helmet;
                case EquipSlot.Necklace: return ItemIconShape.Necklace;
                case EquipSlot.Chest: return ItemIconShape.Coat;
                case EquipSlot.MainHand: return ItemIconShape.Sword;
                case EquipSlot.OffHand: return ItemIconShape.Shield;
                case EquipSlot.Hands: return ItemIconShape.Gloves;
                case EquipSlot.Ring: return ItemIconShape.Ring;
                case EquipSlot.Legs: return ItemIconShape.Pants;
                case EquipSlot.Feet: return ItemIconShape.Boots;
            }
            return category == ItemCategory.Consumable ? ItemIconShape.Potion : ItemIconShape.Ore;
        }
    }

    public string CategoryLabel
    {
        get
        {
            if (IsEquipment) return SlotLabel(equipSlot);
            switch (category)
            {
                case ItemCategory.Consumable: return "Consumível";
                case ItemCategory.Material: return "Material";
                default: return category.ToString();
            }
        }
    }

    public static string SlotLabel(EquipSlot slot)
    {
        switch (slot)
        {
            case EquipSlot.Head: return "Cabeça";
            case EquipSlot.Necklace: return "Colar";
            case EquipSlot.Chest: return "Armadura";
            case EquipSlot.MainHand: return "Arma (mão direita)";
            case EquipSlot.OffHand: return "Mão esquerda";
            case EquipSlot.Hands: return "Luvas";
            case EquipSlot.Ring: return "Anel";
            case EquipSlot.Legs: return "Pernas";
            case EquipSlot.Feet: return "Botas";
        }
        return "";
    }

    // Cria um item em tempo de execução (usado pelo kit de teste do inventário).
    public static ItemData Create(string name, string description, ItemCategory category, EquipSlot slot,
        ItemIconShape shape, Color color, StatBonus bonus = default, int heal = 0, int maxStack = 1)
    {
        ItemData item = CreateInstance<ItemData>();
        item.name = name;
        item.itemName = name;
        item.description = description;
        item.category = category;
        item.equipSlot = slot;
        item.iconShape = shape;
        item.iconColor = color;
        item.bonus = bonus;
        item.healAmount = heal;
        item.maxStack = Mathf.Max(1, maxStack);
        return item;
    }
}
