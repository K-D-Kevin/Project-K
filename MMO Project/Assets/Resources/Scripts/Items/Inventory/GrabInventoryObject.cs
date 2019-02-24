using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInventoryObject : MonoBehaviour {

    // Cursor Elements
    [SerializeField]
    private MimicInventorySlot CursorDisplay;
    [SerializeField]
    private InventorySlot CursorSlot;
    private InventorySlot CopySlot;

    //Inventory Elements
    [SerializeField]
    private InventoryActions IA;

    public void SetSlot(InventorySlot InvSlot)
    {
        CopySlot = InvSlot;
    }

    public void SwitchItems()
    {
        CopySlot.SwapItem(CursorSlot);
        CursorDisplay.CopyThisSlot(CursorSlot);
        CursorDisplay.ApplyCopy();
        IA.SetHotbar();
    }

    public void ResetItem()
    {
        if (CursorSlot.GetHeldItem() != null)
            SwitchItems();
    }
}
