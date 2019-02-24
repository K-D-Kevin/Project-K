using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitCatalogue : MonoBehaviour {

    private Traits[] Catalogue;
    private bool CatalogueFilled = false;

    // Use this for initialization
    public void FindChildren()
    {
        Catalogue = gameObject.GetComponentsInChildren<Traits>();
        CatalogueFilled = true;
    }

    // For later
    //public void SortCatalogue()
    //{

    //}

    // ID search section, Number is the number coorisponding to the ID section
    // ID  strings are always 3 characters long
    public Traits FindObjectID(string ID, int Number, int jump = 10)
    {
        Traits temp = null;
        string primarySearch = "";
        string secondarySearch = "";
        if (!CatalogueFilled)
        {
            FindChildren();
        }
        int CatLength = Catalogue.Length;
        if (ID.Length == 3)
        {
            if (CatLength == 1)
            {
                primarySearch = "PS Single Search: " + Catalogue[0].traitID + " vs " + ID + "-" + Number;
                if (Catalogue[0].traitID == ID + "-" + Number)
                {
                    temp = Instantiate(Catalogue[0], transform);
                    primarySearch = "PF Single Search: " + Catalogue[0].traitID + " vs " + ID + "-" + Number;
                }
            }
            else if (CatLength <= 10)
            {
                primarySearch = "PS" + CatLength;
                foreach (Traits I in Catalogue)
                {
                    if (I.traitID == ID + "-" + Number)
                    {
                        temp = Instantiate(I, transform);
                        primarySearch = "PF";
                        break;
                    }
                }
            }
            else
            {
                secondarySearch = "SS" + CatLength;
                int count = 0;
                int loops = 0;
                while (temp == null || loops > 100)
                {
                    loops++;
                    string id = Catalogue[count].traitID.Split('-')[0];
                    if (id != ID)
                    {
                        count += jump;
                    }
                    else
                    {
                        int idNum = int.Parse(Catalogue[count].traitID.Split('-')[1]);
                        int difference = Number - idNum;
                        count += difference;
                    }

                    if (Catalogue[count].traitID == ID + "-" + Number)
                    {
                        secondarySearch = "SF";
                        temp = Instantiate(Catalogue[count], transform);
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Improper ID length");
            return temp;
        }

        if (temp == null)
        {
            Debug.Log("Could not find item: " + ID + "-" + Number + " " + primarySearch + secondarySearch);
        }
        return temp;
    }

    public Traits FindObjectID(string IDWithNumber)
    {
        Traits temp = null;
        string str = IDWithNumber.Split('-')[0];
        int Num = int.Parse(IDWithNumber.Split('-')[1]);
        temp = FindObjectID(str, Num);
        return temp;
    }
}
