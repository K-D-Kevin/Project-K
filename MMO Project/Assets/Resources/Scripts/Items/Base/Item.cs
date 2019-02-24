using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    // Item Components
    [SerializeField]
    private Rigidbody RB;
    [SerializeField]
    private Collider C;

    // Item durability states and IDs
    public enum DurabilityState
    {
        Unbreakable = 0,
        Undamaged = 1,
        Damaged = 2,
        Broken = 3,
        Destroyed = 4,
    }

    // The item id meannt for saving and finding the item
    [SerializeField]
    private string ItemID = "None";
    public string itemID
    {
        get
        {
            return ItemID;
        }
    }

    // Item of the item that can be used to find the item
    [SerializeField]
    private string ItemName= "None"; // What the game thinks the name is
    public string itemName
    {
        get
        {
            return ItemName;
        }
    }

    /// <summary>
    /// Durability is the state of an item in which can cause the obeject to weaken and / or be destroyed upon use
    /// </summary>
    [SerializeField]
    private DurabilityState durabilityState = DurabilityState.Unbreakable;

    // The item Durability
    [SerializeField]
    private int MaxDurability = 100;
    public int maxDurability
    {
        get
        {
            return MaxDurability;
        }
        set
        {
            MaxDurability = value;
        }
    }

    [SerializeField]
    private int Durability = 100;
    public int durability
    {
        get
        {
            return Durability;
        }
        set
        {
            Durability = value;
        }
    }

    // When the item becomes Broken - Reduced weapons stats  near destroyed - cann't repair a destroyed item
    [SerializeField]
    private int BrokenDurability = 10;
    public int setBrokennDurability
    {
        get
        {
            return BrokenDurability;
        }
        set
        {
            BrokenDurability = value;
        }
    }

    // Item Weight
    [SerializeField]
    private int ItemWeight = 100;
    public int itemWeight
    {
        get
        {
            return ItemWeight;
        }
        set
        {
            ItemWeight = value;
        }
    }


    // 2D sprite for Invenotry and such
    [SerializeField]
    private Texture ItemSprite;
    [SerializeField]
    private Texture EmptySprite;

    // How many of this item can be stacked
    [SerializeField]
    private int MaxStackCount = 1;
    public int maxStackCount
    {
        get
        {
            return MaxStackCount;
        }
    }
    [SerializeField]
    private int StackCount = 1;
    public int stackCount
    {
        get
        {
            return StackCount;
        }
    }

    // Can the item be activated (If not will stay as a sprite in the hand)
    [SerializeField]
    private bool CanActivate = true;
    public bool IsActivated = false;
    [SerializeField]
    private GameObject ChildItem;

    // Saves the current name
    public void RememberName()
    {
        PlayerPrefs.SetString(ItemID, ItemName);
    }

    // Finds the ID that is saved
    public void GetID()
    {
        ItemID = PlayerPrefs.GetString(ItemName, "None");
        SetItemName(ItemName);
    }

    // Sets the Item ID
    public void SetItemID(string ID)
    {
        // if the item doesn't have an ID, give it one
        if (ID == "None")
        {
            ID = FindNewID();
        }
        ItemID = ID;
        PlayerPrefs.SetString(ItemName, ItemID);
    }

    // If the the item does not have a valid ID, its sets a new one
    private string FindNewID()
    {
        string id = "IDNull";
        int ItemIDCount = PlayerPrefs.GetInt("ItemIDNewCount", 0);
        ItemIDCount++;
        PlayerPrefs.SetInt("ItemIDNewCount", ItemIDCount);
        id = "New-" + ItemIDCount;
        // Keeps looking for an openn ID if its taken
        if (PlayerPrefs.HasKey(id))
        {
            id = FindNewID();
        }
        return id;
    }

    // Manually sets the name of the item
    public void SetItemName(string name)
    {
        if (name != ItemName)
        {
            // set the ID to none
            if (PlayerPrefs.HasKey(ItemName))
                PlayerPrefs.DeleteKey(ItemName);

            // Set the name to the current item ID
            ItemName = name;
            PlayerPrefs.SetString(ItemID, ItemName);

            // Reset Item ID to the new name
            SetItemID(ItemID);
        }
    }

    // Sets the game objects name to the items name
    public void SetGameObjectName()
    {
        gameObject.name = ItemName;
    }

    // Sets the game objects name manually
    public void SetGameObjectName(string name)
    {
        gameObject.name = name;
    }

    // Set the durability state of the item
    public void SetDurabilityState(DurabilityState StateID)
    {
        durabilityState = StateID;
    }

    // Get the sprite of the item
    public Texture GetSprite()
    {
        Texture sprite = ItemSprite != null ? ItemSprite
            : EmptySprite != null ? EmptySprite
            : null;
        if (sprite == null)
        {
            Debug.LogError(ItemName + " (" + ItemID + ") Not Found");
        }
        return sprite;
    }

    // Manually set the stack count of the item
    public void SetStack(int amount)
    {
        StackCount = amount;
        if (StackCount < 1)
        {
            Debug.LogWarning("Incorrect Stack Size");
            StackCount = 1;
        }
    }

    // Add a amount to the current stack
    public void AddToStack(int add = 1)
    {
        if (StackCount + add <= MaxStackCount && add >= 0)
        {
            StackCount += add;
        }
    }

    // Remove a amount to the current stack
    public void RemoveFromStack(int minus = 1)
    {
        if (stackCount - minus > 0)
        {
            if (minus >= 0)
                StackCount -= minus;
        }
        else
        {
            StackCount = 0;
        }
    }

    // Equip the item onto a given transfrom
    public void EquipItem(Transform putItem)
    {
        if (CanActivate)
        {
            ChildItem.SetActive(true);
            gameObject.SetActive(true);
            RB.isKinematic = true;
            IsActivated = true;
            transform.parent = putItem;
            C.isTrigger = true;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.rotation = putItem.rotation;
        }
    }

    public void UnEquipItem(Transform putItem)
    {
        ChildItem.SetActive(false);
        gameObject.SetActive(false);
        RB.isKinematic = false;
        IsActivated = false;
        transform.parent = putItem;
        C.isTrigger = false;
        transform.localPosition = Vector3.zero;
    }

    // put the item on the ground
    public void ReleaseItem()
    {
        ChildItem.SetActive(true);
        gameObject.SetActive(true);
        IsActivated = true;
        RB.isKinematic = false;
        gameObject.transform.parent = null;
    }

    // Deactivate the item so it doesn't show up
    public void DeactivateItem()
    {
        ChildItem.SetActive(false);
        gameObject.SetActive(false);
        IsActivated = false;
        RB.isKinematic = false;
    }

    // Toggle the item off annd on based on a given transform
    public void ToggleItem(Transform putItem)
    {
        if (IsActivated)
        {
            DeactivateItem();
        }
        else
        {
            if (CanActivate)
            {
                EquipItem(putItem);
            }
        }
    }

    public DurabilityState GetDurabilityState()
    {
        return durabilityState;
    }

    // These actions are called by the player, they are to  remain empty if they are n not supposed to have a function
    // Left Mouse Button is the defualt button
    public virtual void PrimaryAction()
    {

    }

    // Secondary Actions is suppresed during dual wielding
    // Right Mouse Button is the defualt button
    public virtual void SecondaryAction()
    {

    }

    // R key is the defualt key press
    public virtual void TertiaryAction()
    {

    }

    // Middle Mouse Button is the default key
    public virtual void FourthAction()
    {

    }

    // Scroll wheel forwards
    // Left Weapon is suppressed
    public virtual void FifthAction()
    {

    }

    // Scroll wheel backwards
    // Right Weapon is suppresed
    public virtual void SixthAction()
    {

    }


}
