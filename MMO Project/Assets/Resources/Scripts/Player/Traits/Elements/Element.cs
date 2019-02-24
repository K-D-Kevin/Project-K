using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : Traits
{

    [SerializeField]
    protected ElementIDs.Element Type;
    [SerializeField]
    protected List<ElementIDs.Element> Weaknesses;

    // Ratio of the buff when attacking a type you have advantage on or debuff you get when recieving an attack from a weakness typing
    [HideInInspector]
    readonly public static float TypeAdvantage = 1.5f;

    public bool CheckIfWeak(ElementIDs.Element other)
    {
        bool check = false;
        foreach(ElementIDs.Element E in Weaknesses)
        {
            if (E == other)
            {
                check = true;
                break;
            }
        }
        return check;
    }

    public List<ElementIDs.Element> GetWeaknessList()
    {
        return Weaknesses;
    }

    public ElementIDs.Element GetElementType()
    {
        return Type;
    }
}
