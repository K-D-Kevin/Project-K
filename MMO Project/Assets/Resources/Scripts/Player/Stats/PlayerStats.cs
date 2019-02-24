using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // Players Speed
    [SerializeField]
    private Agent PlayerAgent;

    // Players Traits
    [SerializeField]
    protected Transform ElementTraits;
    protected Element[] ElementList;

    [SerializeField]
    protected Transform StatTraits;
    protected StatTrait[] StatList;

    // Player stats
    [SerializeField]
    private int BaseMaxHp = 100;
    private int MaxHp;
    public int maxHp
    {
        get { return MaxHp; }
    }


    [SerializeField]
    private int BaseAttack = 100;
    private int Attack;
    public int attack
    {
        get { return Attack; }
    }

    [SerializeField]
    private int BaseSpecialAttack = 100;
    private int SpecialAttack;
    public int specialAttack
    {
        get { return SpecialAttack; }
    }

    [SerializeField]
    private int BasePowerAttack = 100;
    private int PowerAttack;
    public int powerAttack
    {
        get { return PowerAttack; }
    }

    [SerializeField]
    private int BaseDefense = 100;
    private int Defense;
    public int defense
    {
        get { return Defense; }
    }

    [SerializeField]
    private int BaseSpecialDefense = 100;
    private int SpecialDefense;
    public int specialDefense
    {
        get { return SpecialDefense; }
    }

    [SerializeField]
    private int BasePowerDefense = 100;
    private int PowerDefense;
    public int powerDefense
    {
        get { return PowerDefense; }
    }

    [SerializeField]
    private int BaseSpeed = 100;
    private int Speed;
    public int speed
    {
        get { return Speed; }
    }

    [SerializeField]
    private int BaseAgility = 100;
    private int Agility;
    public int agility
    {
        get { return Agility; }
    }

    [SerializeField]
    private int BaseStrength = 100;
    private int Strength;
    public int strength
    {
        get { return Strength; }
    }

    [SerializeField]
    private int BaseWeight = 100;
    private int Weight;
    public int weight
    {
        get { return Weight; }
    }

    [SerializeField]
    private int BaseKinesthetics = 100;
    private int Kinesthetics;
    public int kinesthetics
    {
        get { return Kinesthetics; }
    }

    [SerializeField]
    private int BasePractice = 100;
    private int Practice;
    public int practice
    {
        get { return Practice; }
    }

    [SerializeField]
    private int BaseIntelligence = 100;
    private int Intelligence;
    public int intelligence
    {
        get { return Intelligence; }
    }

    [SerializeField]
    private int BaseStudy = 100;
    private int Study;
    public int study
    {
        get { return Study; }
    }

    [SerializeField]
    private int BaseTalent = 100;
    private int Talent;
    public int talent
    {
        get { return Talent; }
    }

    [SerializeField]
    private int BaseWisdom = 100;
    private int Wisdom;
    public int wisdom
    {
        get { return Wisdom; }
    }


    // Use this for initialization
    public virtual void FindChildrenAndStart () {
        ElementList = ElementTraits.GetComponentsInChildren<Element>();
        StatList = StatTraits.GetComponentsInChildren<StatTrait>();
        CalculateStats();
        ApplyStats();
    }

    public void CalculateStats()
    {
        // Set the stats
        MaxHp = BaseMaxHp;

        Attack = BaseAttack;
        SpecialAttack = BaseSpecialAttack;
        PowerAttack = BasePowerAttack;

        Defense = BaseDefense;
        SpecialDefense = BaseSpecialDefense;
        PowerDefense = BaseDefense;

        Speed = BaseSpeed;
        Agility = BaseAgility;

        Strength = BaseStrength;
        Weight = BaseWeight;

        Kinesthetics = BaseKinesthetics;
        Practice = BasePractice;

        Intelligence = BaseIntelligence;
        Study = BaseStudy;

        Talent = BaseTalent;
        Wisdom = BaseWisdom;

        //int ElementCnt = 0;
        // Add the effects from the traits
        foreach (Element E in ElementList)
        {
            MaxHp += E.GetMaxHpUpgrades() * E.GetTier();

            Attack += E.GetAttackUpgrades() * E.GetTier();
            SpecialAttack += E.GetSpecialAttackUpgrades() * E.GetTier();
            PowerAttack += E.GetPowerAttackUpgrades() * E.GetTier();

            Defense += E.GetDefenseUpgrades() * E.GetTier();
            SpecialDefense += E.GetSpecialDefenseUpgrades() * E.GetTier();
            PowerDefense += E.GetPowerDefenseUpgrades() * E.GetTier();

            Speed += E.GetSpeedUpgrades() * E.GetTier();
            Agility += E.GetAgilityUpgrades() * E.GetTier();

            Strength += E.GetStrengthUpgrades() * E.GetTier();
            Weight += E.GetWeightUpgrades() * E.GetTier();

            //ElementCnt++;
        }
        //Debug.Log("Element Count " + ElementCnt);

        //int StatCnt = 0;
        foreach (StatTrait S in StatList)
        {
            MaxHp += S.GetMaxHpUpgrades() * S.GetTier();

            Attack += S.GetAttackUpgrades() * S.GetTier();
            SpecialAttack += S.GetSpecialAttackUpgrades() * S.GetTier();
            PowerAttack += S.GetPowerAttackUpgrades() * S.GetTier();

            Defense += S.GetDefenseUpgrades() * S.GetTier();
            SpecialDefense += S.GetSpecialDefenseUpgrades() * S.GetTier();
            PowerDefense += S.GetPowerDefenseUpgrades() * S.GetTier();

            Speed += S.GetSpeedUpgrades() * S.GetTier();
            Agility += S.GetAgilityUpgrades() * S.GetTier();

            Strength += S.GetStrengthUpgrades() * S.GetTier();
            Weight += S.GetWeightUpgrades() * S.GetTier();

            Kinesthetics += S.GetKinestheticUpgrades() * S.GetTier();
            Practice += S.GetPracticeUpgrades() * S.GetTier();

            Intelligence += S.GetIntelligenceUpgrades() * S.GetTier();
            Study += S.GetStudyUpgrades() * S.GetTier();

            Talent += S.GetTalentUpgrades() * S.GetTier();
            Wisdom += S.GetWisdomUpgrades() * S.GetTier();

            //StatCnt++;
        }
        //Debug.Log("Stat Count " + StatCnt);

        if (MaxHp < 1)
        {
            MaxHp = 1;
        }

        if (Attack < 1)
        {
            Attack = 1;
        }
        if (SpecialAttack < 1)
        {
            SpecialAttack = 1;
        }
        if (PowerAttack < 1)
        {
            PowerAttack = 1;
        }

        if (Defense < 1)
        {
            Defense = 1;
        }
        if (SpecialDefense < 1)
        {
            SpecialDefense = 1;
        }
        if (PowerDefense < 1)
        {
            PowerDefense = 1;
        }

        if (Speed < 1)
        {
            Speed = 1;
        }
        if (Agility < 1)
        {
            Agility = 1;
        }
        if (Weight < 1)
        {
            Weight = 1;
        }
        if (Strength < 1)
        {
            Strength = 1;
        }

        if (Kinesthetics < 1)
        {
            Kinesthetics = 1;
        }
        if (Practice < 1)
        {
            Practice = 1;
        }

        if (Intelligence < 1)
        {
            Intelligence = 1;
        }
        if (Study < 1)
        {
            Study = 1;
        }

        if (Talent < 1)
        {
            Talent = 1;
        }
        if (Wisdom < 1)
        {
            Wisdom = 1;
        }
    }

    public void ApplyStats()
    {
        float LinearSpeed = PlayerAgent.GetLinearSpeed();
        //Debug.Log("Linear Speed " + LinearSpeed);

        float StrafeSpeed = PlayerAgent.GetStrafeSpeed();
        //Debug.Log("Strafe Speed " + StrafeSpeed);

        float JumpForce = PlayerAgent.GetJumpForce();
        //Debug.Log("Jump Force " + JumpForce);

        //Debug.Log("Speed " + Speed);
        LinearSpeed = LinearSpeed * Speed / 100;
        //Debug.Log("Linear Speed During " + LinearSpeed);

        StrafeSpeed = StrafeSpeed + LinearSpeed * Agility / 200 - StrafeSpeed * Agility / 200;
        //Debug.Log("Strafe Speed During " + StrafeSpeed);

        JumpForce = JumpForce * Agility / 100;
        //Debug.Log("Jump Force During " + JumpForce);

        PlayerAgent.ResetMaxLinearSpeed(LinearSpeed);
        PlayerAgent.ResetMaxStrafeSpeed(StrafeSpeed);
        PlayerAgent.ResetJumpForce(JumpForce);
    }
}
