using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {


    public enum WeaponSlot
    {
        PrimarySecondary = 0,
        Primary = 1,
        Secondary = 2,
        Tertiary = 3,
        Gauntlets = 4,
        All = 5,
    }
    [SerializeField]
    private WeaponSlot Slot;


    public enum WeaponHold
    {
        OneHanded = 0,
        TwoHanded = 1,
    }
    [SerializeField]
    private WeaponHold Hold;
    
    public enum DamageType
    {
        Melee =  0,
        Projectile = 1,
        Hitscan = 2,
    }
    [SerializeField]
    private DamageType Type;

    // Weapon Attack Stat
    [SerializeField]
    private int PhysicalAttack = 10;
    public int physicalAttack
    {
        get
        {
            return PhysicalAttack;
        }
        set
        {
            PhysicalAttack = value;
        }
    }
    [SerializeField]
    private int SpecialAttack = 0;
    public int specialAttack
    {
        get
        {
            return SpecialAttack;
        }
        set
        {
            SpecialAttack = value;
        }
    }
    [SerializeField]
    private int PowerAttack = 0;
    public int powerAttack
    {
        get
        {
            return PowerAttack;
        }
        set
        {
            PowerAttack = value;
        }
    }

    public void SetDamageType(DamageType type)
    {
        Type = type;
    }

    public void SetHold(WeaponHold hold)
    {
        Hold = hold;
    }

    public void SetSlot(WeaponSlot slot)
    {
        Slot = slot;
    }

    public WeaponSlot GetSlot()
    {
        return Slot;
    }

    public WeaponHold GetHold()
    {
        return Hold;
    }
}
