using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTrait : Traits
{
    [SerializeField]
    protected int KinestheticsUpgrades = 0;
    [SerializeField]
    protected int PracticeUpgrades = 0;
    [SerializeField]
    protected int IntelligenceUpgrades = 0;
    [SerializeField]
    protected int StudyUpgrades = 0;
    [SerializeField]
    protected int TalentUpgrades = 0;
    [SerializeField]
    protected int WisdomUpgrades = 0;

    [SerializeField]
    private Stats.StatIDs Trait;

    public Stats.StatIDs GetStatType()
    {
        return Trait;
    }

    public int GetKinestheticUpgrades()
    {
        return KinestheticsUpgrades;
    }

    public int GetPracticeUpgrades()
    {
        return PracticeUpgrades;
    }

    public int GetIntelligenceUpgrades()
    {
        return IntelligenceUpgrades;
    }

    public int GetStudyUpgrades()
    {
        return StudyUpgrades;
    }

    public int GetTalentUpgrades()
    {
        return TalentUpgrades;
    }

    public int GetWisdomUpgrades()
    {
        return WisdomUpgrades;
    }
}
