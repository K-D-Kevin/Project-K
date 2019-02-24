using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    private bool Empty = true;

    // Item in the item slot
    protected Item HeldItem = null;
    protected int SlotWeight = 0;
    public int slotWeight
    {
        get
        {
            return SlotWeight;
        }
    }
    protected int Durability = 100;
    protected Item.DurabilityState DurabilityState = Item.DurabilityState.Undamaged;

    // Ui Item Elements
    [SerializeField]
    private Text NumberText;
    [SerializeField]
    private RawImage image; // Sprite Image
    [SerializeField]
    private RawImage NumbersBackground;
    [SerializeField]
    private RawImage Background;
    [SerializeField]
    private RawImage Foreground;

    protected Inventory ParentInventory;

    /// <summary>
    /// Item ID for the held item,it is used to save the item in the inventory slot
    /// </summary>
    protected string ItemIDHeld = "None";
    protected int ItemsHeld = 0;

    /// <summary>
    /// Each inventory slot is different, this number indicates which inventory slot it is
    /// </summary>
    protected int SlotNumber = -1;
    public int slotNumber
    {
        get
        {
            return SlotNumber;
        }
        set
        {
            SlotNumber = value;
        }
    }

    /// <summary>
    /// This is used to update the number on the UI for the amount of items are apart are in the stack
    /// </summary>
    public void UpdateNumber()
    {
        if (ItemsHeld == 0 || HeldItem == null)
        {
            NumberText.text = "";
            NumbersBackground.gameObject.SetActive(false);
        }
        else
        {
            NumbersBackground.gameObject.SetActive(true);
            ItemsHeld = HeldItem.stackCount;
            SetHeldItem();
            if (HeldItem.maxStackCount == 1)
                NumbersBackground.gameObject.SetActive(false);
            else
                NumbersBackground.gameObject.SetActive(true);
            NumberText.text = "" + ItemsHeld;
        }

    }

    /// <summary>
    /// Adds the item to the inventory slot
    /// </summary>
    /// <param name="item"></param>
    /// <returns> This Returns the left over amount of items that couldn't be picked up cause the slot was either full or it went over the characters strength limit </returns>
    public virtual Item AddItem(Item item)
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

    /// <summary>
    /// Removes a certain amount of the item in the inventory slot
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> This returns the item with the amount that was removed, whatever was left  stays  inn the inventory slot</returns>
    public virtual Item RemoveItem(int amount = 1)
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

    // Sets if the slot is empty or not
    public void SetEmpty(bool isEmpty = true)
    {
        Empty = isEmpty;
    }

    // For initializing the slot to the appropriate inventory
    public void SetParentInventory(Inventory parent)
    {
        ParentInventory = parent;
    }

    // send a reference to the item that is getting held at the moment
    public Item GetHeldItem()
    {
        return HeldItem;
    }

    // Calculates the weight  of this inventory slot
    public void CalculateWeight()
    {
        if (HeldItem == null)
        {
            SlotWeight = 0;
        }
        else
        {
            SlotWeight = HeldItem.itemWeight * HeldItem.stackCount;

        }
    }

    // Remembers the saved item if it existed
    public virtual void RememberHeldItem()
    {
        ItemIDHeld = PlayerPrefs.GetString("SlotID" + SlotNumber, "None");
        if (ItemIDHeld == "None")
        {
            //Debug.Log("Nothing Loaded; ID: 'None'");
            SlotWeight = 0;
            ItemsHeld = 0;
        }
        else
        {
            ItemsHeld = PlayerPrefs.GetInt("SlotCount" + SlotNumber, 1);
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
                Durability = PlayerPrefs.GetInt("SlotID_Durability" + SlotNumber, Durability);
                SetDurability(Durability);
                HeldItem.SetStack(ItemsHeld);
                ItemsHeld = HeldItem.stackCount;
                SetHeldItem();
                HeldItem.DeactivateItem();
            }
        }
        UpdateItemDisplay();
    }

    // Saves the item
    public virtual void SetHeldItem()
    {
        if (HeldItem != null)
        {
            ItemIDHeld = HeldItem.itemID;
            ItemsHeld = HeldItem.stackCount;
            HeldItem.RememberName();
            PlayerPrefs.SetString("SlotID" + SlotNumber, ItemIDHeld);
            PlayerPrefs.SetInt("SlotCount" + SlotNumber, ItemsHeld);
            //Debug.Log("SlotID" + SlotNumber + " ID Saved: " + ItemIDHeld + ", Amount: " + ItemsHeld);
        }
        else
        {
            PlayerPrefs.SetString("SlotID" + SlotNumber, "None");
            PlayerPrefs.SetInt("SlotCount" + SlotNumber, 0);
        }
        SetDurability(Durability);
    }

    // if the application closes, save the item
    void OnApplicationQuit()
    {
        SetHeldItem();
        //Debug.Log("Application ending after " + Time.time + " seconds");
    }

    // Updates the UI elements that go with  this inventory slot
    public void UpdateItemDisplay()
    {
        if (HeldItem != null)
        {
            image.color = Color.white;
            image.texture = HeldItem.GetSprite();
        }
        else
        {
            image.color = Color.clear;
            NumberText.transform.parent.gameObject.SetActive(true);
        }
        UpdateNumber();
    }

    // Sets the tranform of the item to the player
    public void ResetIventory(Transform t)
    {
        if (HeldItem != null)
            HeldItem.transform.parent = t;
    }

    // these functions give a reference to the UI elements of the inventory slot
    public RawImage GetNumbersBackground()
    {
        return NumbersBackground;
    }

    // these functions give a reference to the UI elements of the inventory slot
    public RawImage GetBackground()
    {
        return Background;
    }

    // these functions give a reference to the UI elements of the inventory slot
    public RawImage GetForeground()
    {
        return Foreground;
    }

    public void SetBackgrounColors(Color backgroundColor)
    {
        Background.color = backgroundColor;
    }

    public virtual Item SwapItem(Item i = null)
    {
        Item temp = HeldItem;
        HeldItem = i;
        SetHeldItem();
        UpdateItemDisplay();
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

    public virtual void SwapItem(InventorySlot IS)
    {

        Item temp = IS.SwapItem(HeldItem);
        SwapItem(temp);
    }

    public virtual void SetDurability(int durable)
    {
        Durability = durable;
        PlayerPrefs.SetInt("SlotID_Durability" + SlotNumber, Durability);
        if (HeldItem != null)
            HeldItem.durability = Durability;
        SetDurabilityState();
    }

    public void SetDurabilityState()
    {
        if (HeldItem != null)
        {
            if (HeldItem.GetDurabilityState() != Item.DurabilityState.Unbreakable)
            {
                if (Durability >= HeldItem.maxDurability)
                {
                    HeldItem.SetDurabilityState(Item.DurabilityState.Undamaged);
                }
                else if (Durability >= 0.1f * HeldItem.maxDurability)
                {
                    HeldItem.SetDurabilityState(Item.DurabilityState.Damaged);
                }
                else if (Durability <= 0.1f * HeldItem.maxDurability)
                {
                    HeldItem.SetDurabilityState(Item.DurabilityState.Broken);
                }
                else if (Durability <= 0)
                {
                    HeldItem.SetDurabilityState(Item.DurabilityState.Destroyed);
                }
            }
        }
    }
}
