using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletSlot : InventorySlot
{

    /// <summary>
    /// Removes a certain amount of the item in the inventory slot
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> This returns the item with the amount that was removed, whatever was left  stays  inn the inventory slot</returns>
    public override Item RemoveItem(int amount = 1)
    {
        Item temp = Instantiate(HeldItem, HeldItem.transform);
        temp.transform.parent = null;
        SlotWeight -= HeldItem.itemWeight * amount;
        ParentInventory.ReduceWeight(HeldItem.itemWeight * amount);
        if (amount > HeldItem.stackCount)
        {
            amount = HeldItem.stackCount;
        }
        if (amount == HeldItem.stackCount)
        {
            Destroy(HeldItem.gameObject);
            HeldItem = null;
        }
        else
        {
            if (temp.stackCount - amount > 0)
                temp.RemoveFromStack(temp.stackCount - amount);

            HeldItem.RemoveFromStack(amount);
        }
        SetHeldItem();
        UpdateItemDisplay();
        return temp;
    }

    /// <summary>
    /// Adds the item to the inventory slot
    /// </summary>
    /// <param name="item"></param>
    /// <returns> This Returns the left over amount of items that couldn't be picked up cause the slot was either full or it went over the characters strength limit </returns>
    public override Item AddItem(Item item)
    {
        Item temp = item;
        if (HeldItem == null)
        {
            if (ParentInventory != null && item != null)
            {
                if (item.stackCount * item.itemWeight <= ParentInventory.GetAddableWeight)
                {
                    HeldItem = item;
                    temp = Instantiate(HeldItem, HeldItem.transform.position + new Vector3(0, 0.2f, 0), HeldItem.transform.rotation);
                    temp.transform.parent = null;
                    Destroy(temp.gameObject);
                    SlotWeight = item.stackCount * item.itemWeight;
                    ParentInventory.AddWeight(SlotWeight);
                }
                else
                {
                    int numberOfAddableItems = Mathf.FloorToInt(ParentInventory.GetAddableWeight / item.itemWeight);
                    if (numberOfAddableItems > 0)
                    {
                        HeldItem = item;
                        temp = Instantiate(HeldItem, HeldItem.transform);
                        temp.transform.parent = null;

                        HeldItem.RemoveFromStack(HeldItem.stackCount - numberOfAddableItems);
                        temp.RemoveFromStack(numberOfAddableItems);

                        SlotWeight = numberOfAddableItems * item.itemWeight;
                        ParentInventory.AddWeight(SlotWeight);
                    }
                }
            }
        }
        else
        {
            if (ParentInventory != null && item != null)
            {
                if (item.itemID == HeldItem.itemID)
                {
                    int ItemAmount = HeldItem.stackCount + item.stackCount;
                    if (ItemAmount > HeldItem.maxStackCount)
                    {
                        int AmountToFill = HeldItem.maxStackCount - HeldItem.stackCount <= item.stackCount ? HeldItem.maxStackCount - HeldItem.stackCount
                            : item.stackCount;
                        if (AmountToFill > 0)
                        {
                            if (AmountToFill * item.itemWeight <= ParentInventory.GetAddableWeight)
                            {
                                HeldItem.AddToStack(AmountToFill);
                                temp.RemoveFromStack(AmountToFill);
                                SlotWeight += AmountToFill * item.itemWeight;
                                ParentInventory.AddWeight(AmountToFill * item.itemWeight);
                            }
                            else
                            {
                                int numberOfAddableItems = Mathf.FloorToInt(ParentInventory.GetAddableWeight / item.itemWeight) <= item.stackCount ? Mathf.FloorToInt(ParentInventory.GetAddableWeight / item.itemWeight)
                                    : item.stackCount;
                                if (numberOfAddableItems > 0)
                                {
                                    HeldItem.AddToStack(numberOfAddableItems);
                                    temp.RemoveFromStack(numberOfAddableItems);
                                    if (temp.stackCount <= 0)
                                    {
                                        Destroy(temp.gameObject);
                                    }
                                    SlotWeight += numberOfAddableItems * item.itemWeight;
                                    ParentInventory.AddWeight(numberOfAddableItems * item.itemWeight);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (item.stackCount * item.itemWeight <= ParentInventory.GetAddableWeight)
                        {
                            HeldItem.AddToStack(temp.stackCount);
                            temp = Instantiate(HeldItem, HeldItem.transform);
                            temp.transform.parent = null;
                            temp.RemoveFromStack(temp.stackCount);
                            if (temp.stackCount <= 0)
                            {
                                Destroy(temp.gameObject);
                            }
                            SlotWeight += item.stackCount * item.itemWeight;
                            ParentInventory.AddWeight(item.stackCount * item.itemWeight);
                        }
                        else
                        {
                            int numberOfAddableItems = Mathf.FloorToInt(ParentInventory.GetAddableWeight / item.itemWeight) <= item.stackCount ? Mathf.FloorToInt(ParentInventory.GetAddableWeight / item.itemWeight)
                                : item.stackCount;
                            if (numberOfAddableItems > 0)
                            {
                                HeldItem.AddToStack(numberOfAddableItems);
                                temp.RemoveFromStack(numberOfAddableItems);
                                SlotWeight += numberOfAddableItems * item.itemWeight;
                                ParentInventory.AddWeight(numberOfAddableItems * item.itemWeight);
                            }
                        }
                    }
                }
            }
        }
        if (HeldItem != null)
        {
            HeldItem.DeactivateItem();
        }
        SetHeldItem();
        UpdateItemDisplay();
        if (temp != null)
        {
            temp.ReleaseItem();
        }
        return temp;
    }

    // Saves the item
    public override void SetHeldItem()
    {
        if (HeldItem != null)
        {
            ItemIDHeld = HeldItem.itemID;
            ItemsHeld = HeldItem.stackCount;
            HeldItem.RememberName();
            PlayerPrefs.SetString("GauntletsID" + SlotNumber, ItemIDHeld);
            PlayerPrefs.SetInt("GauntletsCount" + SlotNumber, ItemsHeld);
            //Debug.Log("SlotID" + SlotNumber + " ID Saved: " + ItemIDHeld + ", Amount: " + ItemsHeld);
        }
        else
        {
            PlayerPrefs.SetString("GauntletsID" + SlotNumber, "None");
            PlayerPrefs.SetInt("GauntletsCount" + SlotNumber, 0);
        }
        SetDurability(Durability);
    }

    // Remembers the saved item if it existed
    public override void RememberHeldItem()
    {
        ItemIDHeld = PlayerPrefs.GetString("GauntletsID" + SlotNumber, "None");
        if (ItemIDHeld == "None")
        {
            //Debug.Log("Nothing Loaded; ID: 'None'");
            SlotWeight = 0;
            ItemsHeld = 0;
        }
        else
        {
            ItemsHeld = PlayerPrefs.GetInt("GauntletsCount" +
                "" + SlotNumber, 1);
            string[] str = ItemIDHeld.Split('-');
            ItemCatalogue cat = Resources.Load<ItemCatalogue>("Prefabs/Items/Catalogue/" + str[0]);
            if (cat != null)
            {
                cat = Instantiate(cat);
                //Debug.Log(cat.gameObject.name + " Loaded");
                if (cat.FindObjectID(str[0], int.Parse(str[1])) != null)
                    HeldItem = Instantiate(cat.FindObjectID(str[0], int.Parse(str[1])));
                if (HeldItem != null)
                {
                    //Debug.Log(HeldItem.gameObject.name + " Loaded (ID: " + HeldItem.itemID + ")");
                    HeldItem.transform.parent = ParentInventory.transform;
                    HeldItem.transform.localPosition = Vector3.zero;
                }
            }
            if (HeldItem == null)
            {
                string Name = PlayerPrefs.GetString(ItemIDHeld);
                Item folderItem = Resources.Load<Item>("Prefabs/Items/" + Name + ".prefab");
                if (folderItem != null)
                {
                    HeldItem = Instantiate(folderItem, ParentInventory.transform);
                }
            }
            Destroy(cat.gameObject);
            if (HeldItem != null)
            {
                Durability =  PlayerPrefs.GetInt("GauntletID_Durability" + SlotNumber, Durability);
                SetDurability(Durability);
                HeldItem.SetStack(ItemsHeld);
                ItemsHeld = HeldItem.stackCount;
                SetHeldItem();
                HeldItem.DeactivateItem();
            }
        }
        UpdateItemDisplay();
    }

    // Only a helmet can go into this slot
    public override Item SwapItem(Item i = null)
    {
        Item temp = HeldItem;
        Weapon WTemp = null;
        if (i != null)
        {
            WTemp = i.GetComponent<Weapon>();
        }
        else
        {
            HeldItem = i;
            SetHeldItem();
            UpdateItemDisplay();
        }

        if (WTemp != null)
        {
            Weapon.WeaponSlot wSlot = WTemp.GetSlot();
            if (WTemp.GetSlot() == Weapon.WeaponSlot.Gauntlets || WTemp.GetSlot() == Weapon.WeaponSlot.All)
            {
                HeldItem = i;
                SetHeldItem();
                UpdateItemDisplay();
            }
            else
            {
                temp = i;
            }
        }
        else
        {
            temp = i;
        }

        if (HeldItem != null)
        {
            Durability = HeldItem.durability;
        }
        else
        {
            Durability = 100;
        }
        SetDurability(Durability);

        return temp;
    }

    public override void SwapItem(InventorySlot IS)
    {
        if (IS.GetHeldItem() != null)
        {
            Weapon Wtemp = IS.GetHeldItem().GetComponent<Weapon>();
            if (Wtemp != null)
            {
                if (Wtemp.GetSlot() == Weapon.WeaponSlot.Gauntlets || Wtemp.GetSlot() == Weapon.WeaponSlot.All)
                {
                    Item temp = IS.SwapItem(HeldItem);
                    SwapItem(temp);
                }
            }
        }
        else // If the cursor is empty
        {
            Item temp = IS.SwapItem(HeldItem);
            SwapItem(temp);
        }
    }

    public override void SetDurability(int durable)
    {
        Durability = durable;
        PlayerPrefs.SetInt("GauntletID_Durability" + SlotNumber, Durability);
        if (HeldItem != null)
            HeldItem.durability = Durability;
        SetDurabilityState();
    }
}
