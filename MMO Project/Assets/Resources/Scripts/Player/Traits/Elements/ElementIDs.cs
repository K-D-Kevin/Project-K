using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementIDs{

    public enum Element
    {
        Elementless = 0, // Max health of the Character
        Water = 1, // Physical Natural Attacks Ei - Swords, Punches, Basic Arrows, Basic Bullets
        Fire = 2, // Non-Physical Natural Attacks Ei - Electrical Attacks, Fire Attacks, Water Attacks
        Electrical = 3, // UnNatural Attacks Ei - Aura, Ki, Magic, Psychic attacks
        Wind = 4, // Physical Defense
        Earth = 5, // Non-Physical Defense
        Grass = 6, // UnNatural Defense
        Ice = 7, // Top Speed
        Light = 8, // Change of Direction
        Dark = 9, // Ability to carry objects
        Gravity = 10, // Ability to learn physical moves
        Poison = 11, // Ability to retain physical moves
        Aether = 12, // Abillity to learn non-physical moves
        Stats_Count,
    }
}
