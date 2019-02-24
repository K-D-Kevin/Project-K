using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    // Player Stats
    [SerializeField]
    private PlayerStats PS;
    [SerializeField]
    private PlayerActions PA;
    [SerializeField]
    private InventoryActions IA;

    private int InventoryWeight = 0;
    private int PlayerStrength = 100;
    private int PlayerWeight = 100;
    private int AddableWeight;
    public int GetAddableWeight
    {
        get
        {
            return AddableWeight;
        }
    }

    // Other Ui elements that are effected by inventory
    [SerializeField]
    private StatMenuUi StatUI;

    // Get the players inventory slots
    [SerializeField]
    private List<InventorySlot> InventorySlots;
    [SerializeField]
    private Transform ItemTransform;

    // Get the game items
    private ItemCatalogue Catalogue;

    // Use this for initialization
    void Start () {
        SetParentInventory();
    }

    /// <summary>
    /// This calculates the amount  of weight the inventory can add to it
    /// </summary>
    public void CalculateAddableWeight()
    {
        PA.CalculateCurrentWeight();
        PlayerStrength = PS.strength;
        PlayerWeight = PA.currentWeight;
        AddableWeight = PlayerStrength - PlayerWeight;
        StatUI.UpdateStateUI();
    }

    public void SetInventoryWeight(int w)
    {
        if (w -InventoryWeight >= AddableWeight)
        {
            InventoryWeight = w;
            CalculateAddableWeight();
        }
    }

    public int GetInventoryWeight()
    {
        return InventoryWeight;
    }

    public void AddWeight(int w)
    {
        InventoryWeight += w;
        CalculateAddableWeight();
    }

    public void ReduceWeight(int w)
    {
        InventoryWeight -= w;
        CalculateAddableWeight();
    }

    public void SetPlayerStrength(int w)
    {
        PlayerStrength = w;
    }

    public int GetStrength()
    {
        return PlayerStrength;
    }

    public void SetParentInventory()
    {
        int SlotNum = 1;
        foreach (InventorySlot IS in InventorySlots)
        {
            IS.slotNumber = SlotNum;
            SlotNum++;
            IS.SetParentInventory(this);
            IS.RememberHeldItem();
            IS.CalculateWeight();
            InventoryWeight += IS.slotWeight;
            IS.ResetIventory(ItemTransform);
        }
        CalculateAddableWeight();
        IA.SetInventory();
        IA.SetHotbar();
    }

    public void PickUp(Item item)
    {
        CalculateAddableWeight();
        // Look for a slot that already has this item
        if (item.maxStackCount > 1)
        {
            foreach (InventorySlot i in InventorySlots)
            {
                Item temp = i.GetHeldItem();
                if (temp != null && temp.itemID == item.itemID)
                {
                    int itemsThatCanBeAdded = item.stackCount;
                    int amountCanAdd = Mathf.FloorToInt(AddableWeight / temp.itemWeight);
                    int diff = temp.maxStackCount - temp.stackCount;
                    int amount = itemsThatCanBeAdded <= amountCanAdd ? itemsThatCanBeAdded
                        : amountCanAdd;
                    amount = diff <= amount ? diff
                        : amount;
                    if (amount != 0)
                    {
                        if (amount * temp.itemWeight <= AddableWeight)
                        {
                            temp.AddToStack(amount);
                            item.RemoveFromStack(amount);
                            AddWeight(temp.itemWeight * amount);
                            CalculateAddableWeight();
                            if (item.stackCount <= 0)
                                Destroy(item.gameObject);
                            i.UpdateNumber();
                            i.UpdateItemDisplay();
                        }
                    }
                }
            }
        }

        // if no invetory  slot has this item then will try to put it in an empty
        if (item.itemWeight <= AddableWeight && item.stackCount > 0)
        {
            foreach (InventorySlot i in InventorySlots)
            {
                Item temp = i.GetHeldItem();
                if (temp == null)
                {
                    if (item.maxStackCount > 1)
                    {
                        Item dropItem = i.AddItem(item);
                        dropItem.ReleaseItem();
                    }
                    else
                    {
                        i.AddItem(item);
                    }
                    i.UpdateNumber();
                    i.UpdateItemDisplay();
                    temp = i.GetHeldItem();
                    temp.transform.parent = ItemTransform;
                    temp.transform.localPosition = Vector3.zero;
                    break;
                }
            }
        }
    }

    private void UpdateItemDisplay()
    {
        foreach(InventorySlot i in InventorySlots)
        {
            i.UpdateItemDisplay();
        }
    }

    public void ClearInventory()
    {
        foreach(InventorySlot IS in InventorySlots)
        {
            Item temp = IS.GetHeldItem();
            if (temp != null)
            {
                temp = IS.RemoveItem(IS.GetHeldItem().stackCount);
                Destroy(temp.gameObject);
            }
        }
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return InventorySlots;
    }
}
