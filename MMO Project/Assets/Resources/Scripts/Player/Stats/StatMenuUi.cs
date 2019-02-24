using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatMenuUi : MonoBehaviour {

    // Get stats
    [SerializeField]
    private PlayerStats PS;
    [SerializeField]
    private PlayerActions PA;

    // Get text
    [SerializeField]
    private Text PhysicalAttack;
    [SerializeField]
    private Text SpecialAttack;
    [SerializeField]
    private Text PowerAttack;
    [SerializeField]
    private Text PhysicalDefense;
    [SerializeField]
    private Text SpecialDefense;
    [SerializeField]
    private Text PowerDefense;
    [SerializeField]
    private Text Speed;
    [SerializeField]
    private Text Agility;
    [SerializeField]
    private Text Strength;
    [SerializeField]
    private Text Weight;
    [SerializeField]
    private Text Kinesthetics;
    [SerializeField]
    private Text Practice;
    [SerializeField]
    private Text Intelligence;
    [SerializeField]
    private Text Study;
    [SerializeField]
    private Text Talent;
    [SerializeField]
    private Text Wisdom;

    // Update Text
    public void UpdateStateUI()
    {
        PhysicalAttack.text = "Physical: " + PS.attack;
        SpecialAttack.text = "Special: " + PS.specialAttack;
        PowerAttack.text = "Power: " + PS.powerAttack;

        PhysicalDefense.text = "Physical: " + PS.defense;
        SpecialDefense.text = "Special: " + PS.specialDefense;
        PowerDefense.text = "Power: " + PS.powerDefense;

        Speed.text = "Speed: " + PS.speed;
        Agility.text = "Agility: " + PS.agility;
        Strength.text = "Strength: " + PS.strength + " (" + PA.currentWeight + ")";
        Weight.text = "Weight: " + PS.weight;

        Kinesthetics.text = "Kinesthetics: " + PS.kinesthetics;
        Practice.text = "Practice: " + PS.practice;
        Intelligence.text = "Intelligence: " + PS.intelligence;
        Study.text = "Study: " + PS.study;
        Talent.text = "Talent: " + PS.talent;
        Wisdom.text = "Wisdom: " + PS.wisdom;
    }
}
