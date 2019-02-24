using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traits : MonoBehaviour {

    // The item id meannt for saving and finding the item
    [SerializeField]
    private string TraitID = "None";
    public string traitID
    {
        get
        {
            return TraitID;
        }
    }

    [SerializeField]
    private int Tier = 1; // Natural Strength in this Trait

    // Effect this type has on the players stats
    [SerializeField]
    protected int MaxHealthUpgrades = 0;
    [SerializeField]
    protected int AttackUpgrades = 0;
    [SerializeField]
    protected int SpecialAttackUpgrades = 0;
    [SerializeField]
    protected int PowerAttackUpgrades = 0;
    [SerializeField]
    protected int DefenseUpgrades = 0;
    [SerializeField]
    protected int SpecialDefenseUpgrades = 0;
    [SerializeField]
    protected int PowerDefenseUpgrades = 0;
    [SerializeField]
    protected int SpeedUpgrades = 0;
    [SerializeField]
    protected int AgilityUpgrades = 0;
    [SerializeField]
    protected int StrengthUpgrades = 0;
    [SerializeField]
    protected int WeightUpgrades = 0;


    public int GetMaxHpUpgrades()
    {
        return MaxHealthUpgrades;
    }

    public int GetAttackUpgrades()
    {
        return AttackUpgrades;
    }

    public int GetSpecialAttackUpgrades()
    {
        return SpecialAttackUpgrades;
    }

    public int GetPowerAttackUpgrades()
    {
        return PowerAttackUpgrades;
    }

    public int GetDefenseUpgrades()
    {
        return DefenseUpgrades;
    }

    public int GetSpecialDefenseUpgrades()
    {
        return SpecialDefenseUpgrades;
    }

    public int GetPowerDefenseUpgrades()
    {
        return PowerDefenseUpgrades;
    }

    public int GetSpeedUpgrades()
    {
        return SpeedUpgrades;
    }

    public int GetAgilityUpgrades()
    {
        return AgilityUpgrades;
    }

    public int GetStrengthUpgrades()
    {
        return StrengthUpgrades;
    }

    public int GetWeightUpgrades()
    {
        return WeightUpgrades;
    }

    public void SetTier(int tier)
    {
        Tier = tier;
    }

    public int GetTier()
    {
        return Tier;
    }
}
