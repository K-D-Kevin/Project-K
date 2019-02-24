using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats{

    public enum StatIDs
    {
        MaxHealth = 0, // Max health of the Character
        Attack = 1, // Physical Natural Attacks Ei - Swords, Punches, Basic Arrows, Basic Bullets
        SpecialAttack = 2, // Non-Physical Natural Attacks Ei - Electrical Attacks, Fire Attacks, Water Attacks
        PowerAttack = 3, // UnNatural Attacks Ei - Aura, Ki, Magic, Psychic attacks
        Defense = 4, // Physical Defense
        SpecialDefense = 5, // Non-Physical Defense
        PowerDefense  = 6, // UnNatural Defense
        Speed = 7, // Top Speed
        Agility = 8, // Change of Direction -- Strafe Speed
        Strength = 9, // Ability to carry objects
        Weight = 10, // base Character weight
        Kinesthetics = 11, // Ability to learn physical moves
        Practice = 12, // Ability to retain physical moves
        Intelligence = 13, // Abillity to learn non-physical moves
        Study = 14, // Ability to retain non-physical moves
        Talent = 15, // Ability to learn unnatural Moves
        Wisdom = 16, // Ability to retain unnatural Moves
        Stats_Count,
    }
}
