using System;
using UnityEngine;

// Status do personagem mostrados no menu. Os bônus dos equipamentos vêm do Inventory.
public class PlayerStats : MonoBehaviour
{
    [Header("Personagem")]
    [SerializeField] private string playerName = "Kirito";
    [Min(1)] [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;
    [SerializeField] private int col = 500;

    [Header("Status base")]
    [SerializeField] private int baseMaxHp = 250;
    [SerializeField] private int baseAttack = 20;
    [SerializeField] private int baseDefense = 10;
    [SerializeField] private int baseStrength = 5;
    [SerializeField] private int baseAgility = 5;
    [SerializeField] private int baseVitality = 5;
    [Tooltip("HP inicial em relação ao máximo (útil para testar as poções).")]
    [Range(0.05f, 1f)] [SerializeField] private float startingHpPercent = 0.6f;

    [Header("Ganho por nível")]
    [SerializeField] private int hpPerLevel = 50;
    [SerializeField] private int attackPerLevel = 4;
    [SerializeField] private int defensePerLevel = 3;
    [SerializeField] private int attributesPerLevel = 1;

    public event Action Changed;
    public event Action<int> LeveledUp;

    private StatBonus equipment;
    private bool initialized;
    private int currentHp;

    public string PlayerName => playerName;
    public int Level => level;
    public int Experience => experience;
    public int ExpToNextLevel => Mathf.RoundToInt(100f * Mathf.Pow(level, 1.5f));
    public int Col => col;

    public int MaxHp => baseMaxHp + equipment.maxHp + Vitality * 10;
    public int CurrentHp { get { EnsureInit(); return currentHp; } }
    public int Attack => baseAttack + equipment.attack + Strength * 2;
    public int Defense => baseDefense + equipment.defense;
    public int Strength => baseStrength + equipment.strength;
    public int Agility => baseAgility + equipment.agility;
    public int Vitality => baseVitality + equipment.vitality;
    public bool IsFullHp => CurrentHp >= MaxHp;

    private void Awake()
    {
        EnsureInit();
    }

    private void EnsureInit()
    {
        if (initialized) return;
        initialized = true;
        currentHp = Mathf.Max(1, Mathf.RoundToInt(MaxHp * startingHpPercent));
    }

    public void SetEquipmentBonus(StatBonus bonus)
    {
        EnsureInit();
        equipment = bonus;
        currentHp = Mathf.Clamp(currentHp, 1, MaxHp);
        Changed?.Invoke();
    }

    public void SetName(string newName)
    {
        playerName = newName;
        Changed?.Invoke();
    }

    public int Heal(int amount)
    {
        EnsureInit();
        int before = currentHp;
        currentHp = Mathf.Min(MaxHp, currentHp + Mathf.Max(0, amount));
        Changed?.Invoke();
        return currentHp - before;
    }

    public void TakeDamage(int amount)
    {
        EnsureInit();
        currentHp = Mathf.Max(0, currentHp - Mathf.Max(0, amount));
        Changed?.Invoke();
    }

    public void AddCol(int amount)
    {
        col = Mathf.Max(0, col + amount);
        Changed?.Invoke();
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0) return;
        EnsureInit();
        experience += amount;
        while (experience >= ExpToNextLevel)
        {
            experience -= ExpToNextLevel;
            level++;
            baseMaxHp += hpPerLevel;
            baseAttack += attackPerLevel;
            baseDefense += defensePerLevel;
            baseStrength += attributesPerLevel;
            baseAgility += attributesPerLevel;
            baseVitality += attributesPerLevel;
            currentHp = MaxHp; // subir de nível recupera a vida
            LeveledUp?.Invoke(level);
        }
        Changed?.Invoke();
    }
}
