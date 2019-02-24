using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActions : MonoBehaviour {

    // Player Atttributes
    [SerializeField]
    private PlayerActions PA;

    // Inventories
    [SerializeField]
    private Inventory ItemInventory;
    private List<InventorySlot> ItemInventorySlots;

    // Hotbar
    [SerializeField]
    private MimicInventorySlot[] HotbarSlots;

    // UI colors // NOTE FOR LATER: Colors are always changing to green, not sure why - fix this later
    [SerializeField]
    private Color DefaultItemInventoryColor;
    [SerializeField]
    private Color DefaultHotbarColor;
    [SerializeField]
    private Color EquiptedHotbarSlotColor;

    // Active inventoryslots
    private int ActiveRow = 1;
    [SerializeField]
    private int MaxRows = 3;
    [SerializeField]
    private int MaxActiveColumns = 10;
    [SerializeField]
    private int MaxColumns = 17;
    private int EquiptedSlotNum = 0;

    // Equipted Items
    private InventorySlot EquiptedSlot = null;
    private MimicInventorySlot EquiptedMimicSlot = null;

    // Use this for initialization
    void Start () {
        ItemInventorySlots = ItemInventory.GetInventorySlots();
        SetInventory();
        SetHotbar();
	}

    public void SetInventory()
    {
        for (int j = 0; j < MaxRows; j++)
        {
            for (int i = 0; i < MaxActiveColumns; i++)
            {
                int slotToCopy = i + MaxColumns * j;

                InventorySlot slot = ItemInventorySlots[slotToCopy];
                slot.SetBackgrounColors(Color.cyan);

            }
        }
        if (EquiptedSlotNum > 0)
        {
            EquipItem(EquiptedSlotNum);
        }
    }

    public void SetHotbar()
    {
        for (int i = 0; i < MaxActiveColumns; i++)
        {
            int slotToCopy = i + MaxColumns * (ActiveRow - 1);
            MimicInventorySlot CopyHere = HotbarSlots[i];
            InventorySlot CopyThis = ItemInventorySlots[slotToCopy];
            CopyThis.SetBackgrounColors(Color.green);
            CopyHere.CopyThisSlot(CopyThis);
            CopyHere.ApplyCopy();
        }
        if (EquiptedSlotNum > 0)
        {
            EquipItem(EquiptedSlotNum);
        }
    }

    public void EquipItem(int slotNumber = 1)
    {
        int slotToCopy = slotNumber + MaxColumns * (ActiveRow - 1) - 1;
        MimicInventorySlot CopyHere = HotbarSlots[slotNumber - 1];
        InventorySlot CopyThis = ItemInventorySlots[slotToCopy];
        CopyThis.SetBackgrounColors(Color.red);
        CopyHere.CopyThisSlot(CopyThis);
        CopyHere.ApplyCopy();

        // Set the equipted item
        if (EquiptedSlot == null)
        {
            // Set up slot
            EquiptedSlot = CopyThis;
            EquiptedMimicSlot = CopyHere;
            PA.Equip(EquiptedSlot.GetHeldItem());
        }
        else if (EquiptedSlot != CopyThis)
        {
            // Set old slot to default
            if (slotNumber != EquiptedSlotNum)
            {
                EquiptedSlot.SetBackgrounColors(Color.green);
                EquiptedMimicSlot.CopyThisSlot(EquiptedSlot);
                EquiptedMimicSlot.ApplyCopy();
            }
            PA.UnEquip(EquiptedSlot.GetHeldItem());

            // Set new slot
            EquiptedSlot = CopyThis;
            EquiptedMimicSlot = CopyHere;
            PA.Equip(EquiptedSlot.GetHeldItem());
        }
        EquiptedSlotNum = slotNumber;
    }

    public void UnEquip()
    {
        EquiptedSlot.SetBackgrounColors(Color.green);
        EquiptedMimicSlot.CopyThisSlot(EquiptedSlot);
        EquiptedMimicSlot.ApplyCopy();
        PA.UnEquip(EquiptedSlot.GetHeldItem());
        EquiptedSlot = null;
    }

    public void RotateHotbar(bool reverse = false)
    {
        if (reverse)
        {
            ActiveRow--;
            if (ActiveRow == 0)
                ActiveRow = MaxRows;

            SetInventory();
            SetHotbar();
        }
        else
        {
            ActiveRow++;
            if (ActiveRow > MaxRows)
                ActiveRow = 1;

            SetInventory();
            SetHotbar();
        }
    }
    
}
