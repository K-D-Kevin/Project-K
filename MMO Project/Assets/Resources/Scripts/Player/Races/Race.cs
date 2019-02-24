using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : PlayerStats {

    public enum Races
    {
        Humanoid = 0, // Humans - Dwarfs - Elves -- Free Traits -- Free Choose / Ki
        AquaRace = 1, // Fish people, Mermaids etc -- Free Traits -- Water / Fast Swim
        FireRace = 2, // Demons / Vampire -- Free Traits -- Fire / Lava Swim
        ElectricalRace = 3, // Plasma People -- Free Traits -- Electrical / Speed
        WindRace = 4, // Brid People -- Free Traits --  Wind / Acrobatics
        EarthRace = 5, // Beastmen / Golem -- Free Traits -- Earth / Defense
        PlantRace = 6, // Sentient Plants -- Free Traits -- Grass / Healing
        FrozenRace = 7, // Froslings / Yeti -- Free Traits -- Ice / Battery
        HolyRace = 8, // Angels -- Free Traits -- Light / Health
        DarkRace = 9, // Barely Sentient beings that wish destruction on everthing -- Free Traits -- Dark / Stealth
        GravityRace = 10, // Fourth dimension beings -- Free Traits -- Gravity / Transform
        PosionRace = 11, // Lizard and Bug People -- Free Traits -- Posion / Hex
        AetherRace = 12, // Ghost people, this  race was  never living -- Free Traits -- Aether / Phase
        DragonRace = 13, // Dragon People -- Free Traits -- Free Choose / Berserk / SpecificTransform(Dragon)
    }
    [SerializeField]
    private Races PlayerRace = Races.Humanoid;

    // Position to put other traits in
    [SerializeField]
    protected Transform OtherTraits;
    protected Traits[] OtherTraitList;

    private int FreeTraitChoices = 1;
    private string SetUpRaceTraits = "No";

    // Traits to remember
    private Traits TraitOne = null;
    private Traits TraitTwo = null;
    private Traits TraitThree = null;
    private Traits TraitFour = null;
    private Traits TraitFive = null;
    private Traits TraitSix = null;

    // Use this for initialization
    public override void FindChildrenAndStart()
    {
        SetUpFreeTraits();
        SaveTraits();
        ElementList = ElementTraits.GetComponentsInChildren<Element>();
        StatList = StatTraits.GetComponentsInChildren<StatTrait>();
        OtherTraitList = OtherTraits.GetComponentsInChildren<Traits>();
        CalculateStats();
        ApplyStats();
    }

    public int GetChoices()
    {
        return FreeTraitChoices;
    }

    private void SetUpFreeTraits()
    {
        SetUpRaceTraits = PlayerPrefs.GetString("FreeTraits", "No");
        if (SetUpRaceTraits == "No")
        {
            if (PlayerRace == Races.Humanoid)
            {
                FreeTraitChoices++;
                TraitOne = Instantiate(Resources.Load<Traits>("Prefabs/Traits/DPS/Ki"), OtherTraits);
                SetTraitID(2, "None");
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.FireRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Fire"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Other/LavaSwim"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.AquaRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Water"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Other/FastSwim"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.ElectricalRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Electric"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<StatTrait>("Prefabs/Traits/Stats/Speed"), StatTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.WindRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Wind"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Other/Acrobatics"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.EarthRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Earth"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<StatTrait>("Prefabs/Traits/Stats/Defense"), StatTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.PlantRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Grass"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Support/Healing"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.FrozenRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Ice"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Support/Battery"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.HolyRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Light"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<StatTrait>("Prefabs/Traits/Stats/Health"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.DarkRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Dark"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Other/Stealth"), OtherTraits);
            }
            else if (PlayerRace == Races.GravityRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Gravity"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Other/Transform"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.AetherRace)
            {
                TraitOne = Instantiate(Resources.Load<Element>("Prefabs/Traits/Elements/Aether"), ElementTraits);
                TraitTwo = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Other/Phase"), OtherTraits);
                SetTraitID(6, "None");
            }
            else if (PlayerRace == Races.DragonRace)
            {
                FreeTraitChoices++;
                TraitOne = Instantiate(Resources.Load<Traits>("Prefabs/Traits/Other/Berserk"), OtherTraits);
                TraitSix = Instantiate(Resources.Load<Traits>("Prefabs/Traits/DPS/DragonForm"), OtherTraits);
                SetTraitID(2, "None");
            }
            if (TraitOne != null)
            {
                SetTraitID(1, TraitOne.traitID);
            }
            if (TraitTwo != null)
            {
                SetTraitID(2, TraitTwo.traitID);
            }
            if (TraitSix != null)
            {
                SetTraitID(6, TraitSix.traitID);
            }
            PlayerPrefs.SetString("FreeTraits", "Yes");
        }
    }

    // remebers and saves traits
    private void RememberTraits()
    {
        TraitCatalogue Cat = Resources.Load<TraitCatalogue>("Prefabs/Traits/Trt");
        if (Cat != null)
        {
            Cat = Instantiate(Cat);

            for (int i = 0; i < 6; i++)
            {
                int WhichTrait = i + 1;
                string TraitIDFind = PlayerPrefs.GetString("TraitID" + WhichTrait, "None");

                if (WhichTrait == 1 && TraitIDFind != "None")
                {
                    if (TraitOne != null)
                    {
                        Destroy(TraitOne.gameObject);
                        TraitOne = null;
                    }
                    TraitOne = Cat.FindObjectID(TraitIDFind);
                    if (TraitOne != null)
                    {
                        TraitOne.transform.parent = FindTransform(TraitOne);
                        TraitOne.transform.localPosition = Vector3.zero;
                        SetTraitID(WhichTrait, TraitOne.traitID);
                    }
                    else
                    {
                        SetTraitID(WhichTrait, "None");
                    }
                }
                else if (WhichTrait == 2 && TraitIDFind != "None")
                {
                    if (TraitTwo)
                    {
                        Destroy(TraitTwo.gameObject);
                        TraitTwo = null;
                    }
                    TraitTwo = Cat.FindObjectID(TraitIDFind);

                    if (TraitTwo != null)
                    {
                        TraitTwo.transform.parent = FindTransform(TraitTwo);
                        TraitTwo.transform.localPosition = Vector3.zero;
                        SetTraitID(WhichTrait, TraitTwo.traitID);
                    }
                    else
                    {
                        SetTraitID(WhichTrait, "None");
                    }
                }
                else if (WhichTrait == 3 && TraitIDFind != "None")
                {
                    if (TraitThree != null)
                    {
                        Destroy(TraitThree.gameObject);
                        TraitThree = null;
                    }
                    TraitThree = Cat.FindObjectID(TraitIDFind);
                    if (TraitThree != null)
                    {
                        TraitThree.transform.parent = FindTransform(TraitThree);
                        TraitThree.transform.localPosition = Vector3.zero;
                        SetTraitID(WhichTrait, TraitThree.traitID);
                    }
                    else
                    {
                        SetTraitID(WhichTrait, "None");
                    }
                }
                else if (WhichTrait == 4 && TraitIDFind != "None")
                {
                    if (TraitFour != null)
                    {
                        Destroy(TraitFour.gameObject);
                        TraitFour = null;
                    }
                    TraitFour = Cat.FindObjectID(TraitIDFind);
                    if (TraitFour != null)
                    {
                        TraitFour.transform.parent = FindTransform(TraitFour);
                        TraitFour.transform.localPosition = Vector3.zero;
                        SetTraitID(WhichTrait, TraitFour.traitID);
                    }
                    else
                    {
                        SetTraitID(WhichTrait, "None");
                    }
                }
                else if (WhichTrait == 5 && TraitIDFind != "None")
                {
                    if (TraitFive != null)
                    {
                        Destroy(TraitFive.gameObject);
                        TraitFive = null;
                    }
                    TraitFive = Cat.FindObjectID(TraitIDFind);
                    if (TraitFive != null)
                    {
                        TraitFive.transform.parent = FindTransform(TraitFive);
                        TraitFive.transform.localPosition = Vector3.zero;
                        SetTraitID(WhichTrait, TraitFive.traitID);
                    }
                    else
                    {
                        SetTraitID(WhichTrait, "None");
                    }
                }
                else if (WhichTrait == 6 && TraitIDFind != "None")
                {
                    if (TraitSix != null)
                    {
                        Destroy(TraitSix.gameObject);
                        TraitSix = null;
                    }
                    TraitSix = Cat.FindObjectID(TraitIDFind);
                    if (TraitSix != null)
                    {
                        TraitSix.transform.parent = FindTransform(TraitSix);
                        TraitSix.transform.localPosition = Vector3.zero;
                        SetTraitID(WhichTrait, TraitSix.traitID);
                    }
                    else
                    {
                        SetTraitID(WhichTrait, "None");
                    }
                }
            }
            Destroy(Cat.gameObject);
        }
        else
        {
            Debug.Log("Catalogue Not Found");
        }

    }

    // Makes the traits that are duplicates get added  to the others
    private void ConsolodateTraits()
    {
        if (TraitOne != null)
        {
            if (TraitTwo != null)
            {
                if (TraitOne.traitID == TraitTwo.traitID)
                {
                    TraitOne.SetTier(TraitOne.GetTier() + TraitTwo.GetTier());
                    Destroy(TraitTwo.gameObject);
                    TraitTwo = null;
                }
            }
            if (TraitThree != null)
            {
                if (TraitOne.traitID == TraitThree.traitID)
                {
                    TraitOne.SetTier(TraitOne.GetTier() + TraitThree.GetTier());
                    Destroy(TraitThree.gameObject);
                    TraitThree = null;
                }
            }
            if (TraitFour != null)
            {
                if (TraitOne.traitID == TraitFour.traitID)
                {
                    TraitOne.SetTier(TraitOne.GetTier() + TraitFour.GetTier());
                    Destroy(TraitFour.gameObject);
                    TraitFour = null;
                }
            }
            if (TraitFive != null)
            {
                if (TraitOne.traitID == TraitFive.traitID)
                {
                    TraitOne.SetTier(TraitOne.GetTier() + TraitFive.GetTier());
                    Destroy(TraitFive.gameObject);
                    TraitFive = null;
                }
            }
            if (TraitSix != null)
            {
                if (TraitOne.traitID == TraitSix.traitID)
                {
                    TraitOne.SetTier(TraitOne.GetTier() + TraitSix.GetTier());
                    Destroy(TraitSix.gameObject);
                    TraitSix = null;
                }
            }
        }
        if (TraitTwo != null)
        {
            if (TraitThree != null)
            {
                if (TraitTwo.traitID == TraitThree.traitID)
                {
                    TraitTwo.SetTier(TraitTwo.GetTier() + TraitThree.GetTier());
                    Destroy(TraitThree.gameObject);
                    TraitThree = null;
                }
            }
            if (TraitFour != null)
            {
                if (TraitTwo.traitID == TraitFour.traitID)
                {
                    TraitTwo.SetTier(TraitTwo.GetTier() + TraitFour.GetTier());
                    Destroy(TraitFour.gameObject);
                    TraitFour = null;
                }
            }
            if (TraitFive != null)
            {
                if (TraitTwo.traitID == TraitFive.traitID)
                {
                    TraitTwo.SetTier(TraitTwo.GetTier() + TraitFive.GetTier());
                    Destroy(TraitFive.gameObject);
                    TraitFive = null;
                }
            }
            if (TraitSix != null)
            {
                if (TraitTwo.traitID == TraitSix.traitID)
                {
                    TraitTwo.SetTier(TraitTwo.GetTier() + TraitSix.GetTier());
                    Destroy(TraitSix.gameObject);
                    TraitSix = null;
                }
            }
        }
        if (TraitThree != null)
        {
            if (TraitFour != null)
            {
                if (TraitThree.traitID == TraitFour.traitID)
                {
                    TraitThree.SetTier(TraitThree.GetTier() + TraitFour.GetTier());
                    Destroy(TraitFour.gameObject);
                    TraitFour = null;
                }
            }
            if (TraitFive != null)
            {
                if (TraitThree.traitID == TraitFive.traitID)
                {
                    TraitThree.SetTier(TraitThree.GetTier() + TraitFive.GetTier());
                    Destroy(TraitFive.gameObject);
                    TraitFive = null;
                }
            }
            if (TraitSix != null)
            {
                if (TraitThree.traitID == TraitSix.traitID)
                {
                    TraitThree.SetTier(TraitThree.GetTier() + TraitSix.GetTier());
                    Destroy(TraitSix.gameObject);
                    TraitSix = null;
                }
            }
        }
        if (TraitFour != null)
        {
            if (TraitFive != null)
            {
                if (TraitFour.traitID == TraitFive.traitID)
                {
                    TraitFour.SetTier(TraitFour.GetTier() + TraitFive.GetTier());
                    Destroy(TraitFive.gameObject);
                    TraitFive = null;
                }
            }
            if (TraitSix != null)
            {
                if (TraitFour.traitID == TraitSix.traitID)
                {
                    TraitFour.SetTier(TraitFour.GetTier() + TraitSix.GetTier());
                    Destroy(TraitSix.gameObject);
                    TraitSix = null;
                }
            }
        }
        if (TraitFive != null)
        {
            if (TraitSix != null)
            {
                if (TraitFive.traitID == TraitSix.traitID)
                {
                    TraitFive.SetTier(TraitFive.GetTier() + TraitSix.GetTier());
                    Destroy(TraitSix.gameObject);
                    TraitSix = null;
                }
            }
        }
    }

    // Saves the traits then puts them together
    public void SaveTraits()
    {
        RememberTraits();
        ConsolodateTraits();
    }

    private void SetTraitID(int i, string val = "None")
    {
        PlayerPrefs.SetString("TraitID" + i, val);
    }

    public void SetTrait(Traits set, int traitSpot)
    {
        if (traitSpot > 0 && traitSpot < 7)
        {
            if (traitSpot == 1)
            {
                TraitOne = set;
                SetTraitID(traitSpot, TraitOne.traitID);
            }
            else if (traitSpot == 2)
            {
                TraitTwo = set;
                SetTraitID(traitSpot, TraitTwo.traitID);
            }
            else if (traitSpot == 3)
            {
                TraitThree = set;
                SetTraitID(traitSpot, TraitThree.traitID);
            }
            else if (traitSpot == 4)
            {
                TraitFour = set;
                SetTraitID(traitSpot, TraitFour.traitID);
            }
            else if (traitSpot == 5)
            {
                TraitFive = set;
                SetTraitID(traitSpot, TraitFive.traitID);
            }
            else if (traitSpot == 6)
            {
                TraitSix = set;
                SetTraitID(traitSpot, TraitSix.traitID);
            }
        }
        else
        {
            Debug.Log("Incorrect Trait Spot");
        }
    }

    public Traits GetTrait(int traitSpot)
    {

        if (traitSpot > 0 && traitSpot < 7)
        {
            Traits temp = null;
            if (traitSpot == 1)
            {
                temp = TraitOne;
            }
            else if (traitSpot == 2)
            {
                temp = TraitTwo;
            }
            else if (traitSpot == 3)
            {
                temp = TraitThree;
            }
            else if (traitSpot == 4)
            {
                temp = TraitFour;
            }
            else if (traitSpot == 5)
            {
                temp = TraitFive;
            }
            else
            {
                temp = TraitSix;
            }
            return temp;
        }
        else
        {
            Debug.Log("Incorrect Trait Spot");
            return null;
        }
    }

    public void ResetFreeTraits()
    {
        PlayerPrefs.SetString("FreeTraits", "No");
    }

    private Transform FindTransform(Traits trait)
    {
        Transform temp = transform;
        if (trait.GetComponent<Element>() != null)
        {
            temp = ElementTraits;
        }
        else if (trait.GetComponent<StatTrait>() != null)
        {
            temp = StatTraits;
        }
        else
        {
            temp = OtherTraits;
        }
        return temp;
    }
}
