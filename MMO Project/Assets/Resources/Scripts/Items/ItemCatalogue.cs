using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatalogue : MonoBehaviour {

    private Item[] Catalogue;
    private bool CatalogueFilled = false;
    
    // Use this for initialization
    public void FindChildren () {
        Catalogue = gameObject.GetComponentsInChildren<Item>();
        CatalogueFilled = true;
        //SetIDs();
	}

    public void SetIDs()
    {
        if (Catalogue.Length > 0)
        {
            foreach (Item I in Catalogue)
            {
                I.GetID();
            }
        }
    }

    // For later
    //public void SortCatalogue()
    //{

    //}

    // ID search section, Number is the number coorisponding to the ID section
    // ID  strings are always 3 characters long
    public Item FindObjectID(string ID, int Number, int jump = 10)
    {
        Item temp = null;
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
                primarySearch = "PS Single Search: " + Catalogue[0].itemID + " vs " + ID + "-" + Number;
                if (Catalogue[0].itemID == ID + "-" + Number)
                {
                    temp = Instantiate(Catalogue[0], transform);
                    primarySearch = "PF Single Search: " + Catalogue[0].itemID + " vs " + ID + "-" + Number;
                }
            }
            else if (CatLength <= 10)
            {
                primarySearch = "PS" + CatLength;
                foreach (Item I in Catalogue)
                {
                    if  (I.itemID == ID + "-" + Number)
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
                    string id = Catalogue[count].itemID.Split('-')[0];
                    if (id != ID)
                    {
                        count += jump;
                    }
                    else 
                    {
                        int idNum = int.Parse(Catalogue[count].itemID.Split('-')[1]);
                        int difference = Number - idNum;
                        count += difference;
                    }

                    if (Catalogue[count].itemID == ID + "-" + Number)
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

    public Item FindObjectID(string IDWithNumber)
    {
        Item temp = null;
        string str = IDWithNumber.Split('-')[0];
        int Num = int.Parse(IDWithNumber.Split('-')[1]);
        temp = FindObjectID(str, Num);
        return temp;
    }
}
