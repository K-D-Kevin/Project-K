using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item {

    public enum ArmorSlot
    {
        Helmet = 0,
        Chestpiece = 1,
        Leggings = 2,
        Footware = 3,
        Gauntlets = 4,
    }
    [SerializeField]
    private ArmorSlot Slot;

    // Weapon Defense Stat
    [SerializeField]
    private int PhysicalDefense = 10;
    public int physicalDefense
    {
        get
        {
            return PhysicalDefense;
        }
        set
        {
            PhysicalDefense = value;
        }
    }
    [SerializeField]
    private int SpecialDefense = 0;
    public int specialDefense
    {
        get
        {
            return SpecialDefense;
        }
        set
        {
            SpecialDefense = value;
        }
    }
    [SerializeField]
    private int PowerDefense = 0;
    public int powerDefense
    {
        get
        {
            return PowerDefense;
        }
        set
        {
            PowerDefense = value;
        }
    }

    public void SetSlot(ArmorSlot slot)
    {
        Slot = slot;
    }

    public ArmorSlot GetSlot()
    {
        return Slot;
    }

}
