using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {

    // Reference to Ui Elements
    [SerializeField]
    private HpUI HpUIElement;
    [SerializeField]
    private GameObject TabMenuUI;
    [SerializeField]
    private GameObject EscMenuUI;
    private bool TabMenuActive = false;
    private bool EscMenuActive = false;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private InventoryActions IA;
    [SerializeField]
    GrabInventoryObject GrabItems;

    // Players Kit
    [SerializeField]
    private HelmetSlot HS;
    [SerializeField]
    private ChestPieceSlot CS;
    [SerializeField]
    private LegSlot LS;
    [SerializeField]
    private FeetSlot FS;
    [SerializeField]
    private PrimarySlot PS;
    [SerializeField]
    private SecondarySlot SS;
    [SerializeField]
    private TertiarySlot TS;
    [SerializeField]
    private GauntletSlot GS;

    // Reference to Players stats and elements
    [SerializeField]
    private Race Stats;
    private int MaxHp;
    private int CurrentHp;
    private int BaseWeight;
    private int CurrentWeight;
    public int currentWeight
    {
        get
        {
            return CurrentWeight;
        }
    }
    private int Strength;

    [SerializeField]
    private FSMStateManager FSM;
    [SerializeField]
    private Camera Cam;

    // Geometry Position
    [SerializeField]
    private Transform LeftHand;
    [SerializeField]
    private Transform RightHand;
    [SerializeField]
    private Transform Back1;
    [SerializeField]
    private Transform Back2;
    [SerializeField]
    private Transform Belt;

    // Stat Display
    [SerializeField]
    private StatMenuUi StatUI;

    // PickUp
    [SerializeField]
    private LayerMask ItemLayer;
    [SerializeField]
    private float PickUpRange = 3;
    [SerializeField]
    private Transform ItemTransform;

    // Equip
    private Item EquiptedItem = null; // Right Hand
    private Item EquiptedItemLeft = null; // Left Hand

	// Use this for initialization
	void Start () {
        Stats.FindChildrenAndStart(); // Make sure start is done for the player stats
        HpUIElement.InitialStart(); // Make sure it starts before this
        MaxHp = Stats.maxHp;
        BaseWeight = Stats.weight;
        CurrentWeight = BaseWeight;
        Strength = Stats.strength;

        CurrentHp = PlayerPrefs.GetInt("CurrentHp", MaxHp);
        HpUIElement.SetMaxHp(MaxHp);
        HpUIElement.SetCurrentHp(CurrentHp);
        StatUI.UpdateStateUI();
        SetPlayerKit();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (TabMenuActive)
            {
                GrabItems.ResetItem();
            }
            ToggleTabMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TabMenuActive)
            {
                GrabItems.ResetItem();
            }
            ToggleEscMenu();
        }

        if (TabMenuActive || EscMenuActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            FSM.DesiredState = FSMStateIDs.StateIds.FSM_PlayerIdle;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            FSM.DesiredState = FSMStateIDs.StateIds.FSM_BasePlayer;
        }

        if (!EscMenuActive)
        {
            // Rotate Hotbar
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IA.RotateHotbar();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                IA.RotateHotbar(true);
            }

            // Equiptitem
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                IA.SetHotbar();
                IA.EquipItem();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                IA.SetHotbar();
                IA.EquipItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                IA.SetHotbar();
                IA.EquipItem(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                IA.SetHotbar();
                IA.EquipItem(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                IA.SetHotbar();
                IA.EquipItem(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                IA.SetHotbar();
                IA.EquipItem(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                IA.SetHotbar();
                IA.EquipItem(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                IA.SetHotbar();
                IA.EquipItem(8);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                IA.SetHotbar();
                IA.EquipItem(9);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                IA.SetHotbar();
                IA.EquipItem(10);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PickUp();
        }

        // Temporary Command (Empty Inventory)
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.ClearInventory();
            ErasePlayerKit();
            Stats.ResetFreeTraits();
        }

        // equip right hand, preference to primary weapon
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PressZ();
        }

        // Dual Wield if possible
        if (Input.GetKeyDown(KeyCode.X))
        {
            PressX();
        }

        if (EquiptedItem != null)
        {
            if (EquiptedItemLeft != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    EquiptedItem.PrimaryAction();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    EquiptedItemLeft.PrimaryAction();
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    EquiptedItemLeft.TertiaryAction();
                    EquiptedItem.TertiaryAction();
                }
                if (Input.GetMouseButtonDown(2))
                {
                    EquiptedItemLeft.FourthAction();
                    EquiptedItem.FourthAction();
                }
                if (Input.mouseScrollDelta.y > 0)
                {
                    EquiptedItem.FifthAction();
                }
                if (Input.mouseScrollDelta.y < 0)
                {
                    EquiptedItemLeft.SixthAction();
                }

            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    EquiptedItem.PrimaryAction();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    EquiptedItem.SecondaryAction();
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    EquiptedItem.TertiaryAction();
                }
                if (Input.GetMouseButtonDown(2))
                {
                    EquiptedItem.FourthAction();
                }
                if (Input.mouseScrollDelta.y > 0)
                {
                    EquiptedItem.FifthAction();
                }
                if (Input.mouseScrollDelta.y < 0)
                {
                    EquiptedItem.SixthAction();
                }
            }
        }
        else
        {
            // Punch and kick actions here
        }
    }

    private void PressZ()
    {
        if (EquiptedItem == null)
        {
            if (PS.GetHeldItem() != null)
            {
                Equip(PS.GetHeldItem());
            }
            else if (SS.GetHeldItem() != null)
            {
                Equip(SS.GetHeldItem());
            }
            else if (TS.GetHeldItem() != null)
            {
                Equip(TS.GetHeldItem());
            }
        }
        else
        {
            if (PS.GetHeldItem() != null)
            {
                if (EquiptedItem != PS.GetHeldItem())
                {
                    if (SS.GetHeldItem() != null)
                    {
                        if (EquiptedItem != SS.GetHeldItem())
                        {
                            if (TS.GetHeldItem() != null && EquiptedItem == TS.GetHeldItem())
                            {
                                UnEquip(EquiptedItem);
                                Equip(PS.GetHeldItem());
                                SetTertiary();
                            }
                            else
                            {
                                UnEquip(EquiptedItem);
                                Equip(PS.GetHeldItem());
                            }
                        }
                        else
                        {
                            UnEquip(EquiptedItem);
                            if (TS.GetHeldItem() != null)
                            {
                                Equip(TS.GetHeldItem());
                            }
                            SetSecondary();

                        }
                    }
                }
                else
                {
                    UnEquip(EquiptedItem);
                    if (SS.GetHeldItem() != null)
                    {
                        Equip(SS.GetHeldItem());
                    }
                    SetPrimary();

                }
            }
            else if (SS.GetHeldItem() != null)
            {
                if (EquiptedItem != SS.GetHeldItem())
                {
                    if (TS.GetHeldItem() != null)
                    {
                        if (EquiptedItem != TS.GetHeldItem())
                        {
                            if (TS.GetHeldItem() != null && EquiptedItem == TS.GetHeldItem())
                            {
                                UnEquip(EquiptedItem);
                                Equip(SS.GetHeldItem());
                                SetTertiary();
                            }
                            else
                            {
                                UnEquip(EquiptedItem);
                                Equip(SS.GetHeldItem());
                            }
                        }
                        else
                        {
                            UnEquip(EquiptedItem);
                            SetTertiary();
                        }
                    }
                }
                else
                {
                    UnEquip(EquiptedItem);
                    if (TS.GetHeldItem() != null)
                    {
                        Equip(TS.GetHeldItem());
                    }
                    SetSecondary();
                }
            }
            else if (TS.GetHeldItem() != null)
            {
                if (EquiptedItem != TS.GetHeldItem())
                {
                    UnEquip(EquiptedItem);
                    Equip(TS.GetHeldItem());
                }
                else
                {
                    UnEquip(EquiptedItem);
                    SetTertiary();
                }
            }
            else
            {
                UnEquip(EquiptedItem);
            }
        }
    }

    private void PressX()
    {
        if (EquiptedItemLeft == null)
        {
            if (SS.GetHeldItem() != null)
            {
                if (PS.GetHeldItem() != null)
                {
                    if (EquiptedItem == PS.GetHeldItem())
                    {
                        Weapon ps = PS.GetHeldItem().GetComponent<Weapon>();
                        Weapon ss = SS.GetHeldItem().GetComponent<Weapon>();
                        if (ps.GetHold() == Weapon.WeaponHold.OneHanded && ss.GetHold() == Weapon.WeaponHold.OneHanded)
                        {
                            EquipLeft(SS.GetHeldItem());
                        }
                        else
                        {
                            UnEquip(PS.GetHeldItem());
                            Equip(SS.GetHeldItem());
                            SetPrimary();
                        }
                    }
                    else
                    {
                        if (TS.GetHeldItem() != null)
                        {
                            if (EquiptedItem == TS.GetHeldItem())
                            {
                                UnEquip(EquiptedItem);
                                SetTertiary();
                            }
                            if (EquiptedItem != SS.GetHeldItem())
                            {
                                UnEquip(EquiptedItem);
                            }
                        }

                        Weapon ps = PS.GetHeldItem().GetComponent<Weapon>();
                        Weapon ss = SS.GetHeldItem().GetComponent<Weapon>();
                        if (ps.GetHold() == Weapon.WeaponHold.OneHanded && ss.GetHold() == Weapon.WeaponHold.OneHanded)
                        {
                            EquipLeft(SS.GetHeldItem());
                            Equip(PS.GetHeldItem());
                        }
                        else
                        {
                            Equip(SS.GetHeldItem());
                        }
                    }
                }
            }
            else if (PS.GetHeldItem() != null)
            {
                if (SS.GetHeldItem() != null)
                {
                    if (EquiptedItem == SS.GetHeldItem())
                    {
                        Weapon ps = PS.GetHeldItem().GetComponent<Weapon>();
                        Weapon ss = SS.GetHeldItem().GetComponent<Weapon>();
                        if (ps.GetHold() == Weapon.WeaponHold.OneHanded && ss.GetHold() == Weapon.WeaponHold.OneHanded)
                        {
                            EquipLeft(PS.GetHeldItem());
                        }
                        else
                        {
                            UnEquip(SS.GetHeldItem());
                            Equip(PS.GetHeldItem());
                            SetSecondary();
                        }
                    }
                    else
                    {
                        if (TS.GetHeldItem() != null)
                        {
                            if (EquiptedItem == TS.GetHeldItem())
                            {
                                UnEquip(EquiptedItem);
                                SetTertiary();
                            }
                            if (EquiptedItem != SS.GetHeldItem())
                            {
                                UnEquip(EquiptedItem);
                            }
                        }

                        Weapon ps = PS.GetHeldItem().GetComponent<Weapon>();
                        Weapon ss = SS.GetHeldItem().GetComponent<Weapon>();
                        if (ps.GetHold() == Weapon.WeaponHold.OneHanded && ss.GetHold() == Weapon.WeaponHold.OneHanded)
                        {
                            EquipLeft(PS.GetHeldItem());
                            Equip(SS.GetHeldItem());
                        }
                        else
                        {
                            Equip(PS.GetHeldItem());
                        }
                    }
                }
            }
        }
        else
        {
            if (PS.GetHeldItem() != null)
            {
                if (PS.GetHeldItem() == EquiptedItem)
                {
                    UnEquip(EquiptedItem);
                    SetPrimary();
                }
                else if (PS.GetHeldItem() == EquiptedItemLeft)
                {
                    UnEquip(EquiptedItemLeft);
                    SetPrimary();
                }
            }
            if (SS.GetHeldItem() != null)
            {
                if (SS.GetHeldItem() == EquiptedItem)
                {
                    UnEquip(EquiptedItem);
                    SetSecondary();
                }
                else if (SS.GetHeldItem() == EquiptedItemLeft)
                {
                    UnEquip(EquiptedItemLeft);
                    SetSecondary();
                }
            }
        }
    }


    // Set Up the Players kit
    public void SetPlayerKit()
    {
        HS.slotNumber = 1;
        CS.slotNumber = 1;
        LS.slotNumber = 1;
        FS.slotNumber = 1;
        PS.slotNumber = 1;
        SS.slotNumber = 1;
        TS.slotNumber = 1;
        GS.slotNumber = 1;

        HS.SetParentInventory(inventory);
        CS.SetParentInventory(inventory);
        LS.SetParentInventory(inventory);
        FS.SetParentInventory(inventory);
        PS.SetParentInventory(inventory);
        SS.SetParentInventory(inventory);
        TS.SetParentInventory(inventory);
        GS.SetParentInventory(inventory);

        HS.RememberHeldItem();
        CS.RememberHeldItem();
        LS.RememberHeldItem();
        FS.RememberHeldItem();
        PS.RememberHeldItem();
        SS.RememberHeldItem();
        TS.RememberHeldItem();
        GS.RememberHeldItem();

        HS.UpdateItemDisplay();
        CS.UpdateItemDisplay();
        LS.UpdateItemDisplay();
        FS.UpdateItemDisplay();
        PS.UpdateItemDisplay();
        SS.UpdateItemDisplay();
        TS.UpdateItemDisplay();
        GS.UpdateItemDisplay();

        HS.SetBackgrounColors(Color.cyan);
        CS.SetBackgrounColors(Color.cyan);
        LS.SetBackgrounColors(Color.cyan);
        FS.SetBackgrounColors(Color.cyan);
        PS.SetBackgrounColors(Color.cyan);
        SS.SetBackgrounColors(Color.cyan);
        TS.SetBackgrounColors(Color.cyan);
        GS.SetBackgrounColors(Color.cyan);

        SetPrimary();
        SetSecondary();
        SetTertiary();
    }
    /// <summary>
    /// Finds the current weight of the player
    /// </summary>
    public void CalculateCurrentWeight()
    {
        int PlayerKitWeight = HS.slotWeight + CS.slotWeight + LS.slotWeight + FS.slotWeight + PS.slotWeight + SS.slotWeight + TS.slotWeight + GS.slotWeight;
        CurrentWeight = inventory.GetInventoryWeight() + BaseWeight; // Weight of the player armor weapons and invenotory as well as the players base weight
    }

    /// <summary>
    ///  Toggle the Inventory On and Off
    /// </summary>
    private void ToggleTabMenu()
    {
        TabMenuActive = !TabMenuActive;
        TabMenuUI.SetActive(TabMenuActive);
        StatUI.UpdateStateUI();
        inventory.CalculateAddableWeight();
        IA.SetInventory();
        IA.SetHotbar();
        if (EscMenuActive)
            ToggleEscMenu();
    }

    /// <summary>
    /// Toggle the Pause / Start Menu On and Off
    /// </summary>
    private void ToggleEscMenu()
    {
        EscMenuActive = !EscMenuActive;
        EscMenuUI.SetActive(EscMenuActive);
        if (TabMenuActive)
            ToggleTabMenu();
    }

    /// <summary>
    /// Sends a raycast to find a possible item that can be picked up
    /// </summary>
    public void PickUp()
    {
        RaycastHit RH;
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RH, PickUpRange, ItemLayer))
        {
            Item pickup = RH.collider.gameObject.GetComponent<Item>();
            inventory.PickUp(pickup);
            if (pickup.stackCount <= 0)
                Destroy(pickup.gameObject);
        }
        Debug.DrawRay(Cam.transform.position, Cam.transform.forward * RH.distance, Color.blue, 0.1f);
    }

    public bool GetActiveInventory()
    {
        return TabMenuActive;
    }

    public void Equip(Item i)
    {
        if (i != null)
            i.EquipItem(RightHand);
        EquiptedItem = i;
    }

    public void EquipLeft(Item i)
    {
        if (i != null)
            i.EquipItem(LeftHand);
        EquiptedItemLeft = i;
    }

    public void UnEquip(Item i)
    {
        if (i != null)
            i.UnEquipItem(ItemTransform);
        if (EquiptedItem == i)
        {
            EquiptedItem = null;
        }
        else if (EquiptedItemLeft == i)
        {
            EquiptedItemLeft = null;
        }
    }

    // if the application closes, save the item
    void OnApplicationQuit()
    {
        GrabItems.ResetItem();
        //Debug.Log("Application ending after " + Time.time + " seconds");
    }

    private void ErasePlayerKit()
    {
        Item temp = HS.GetHeldItem();
        if (temp != null)
        {
            temp = HS.RemoveItem(HS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }

        temp = CS.GetHeldItem();
        if (temp != null)
        {
            temp = CS.RemoveItem(CS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }

        temp = LS.GetHeldItem();
        if (temp != null)
        {
            temp = LS.RemoveItem(LS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }

        temp = FS.GetHeldItem();
        if (temp != null)
        {
            temp = FS.RemoveItem(FS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }

        temp = PS.GetHeldItem();
        if (temp != null)
        {
            temp = PS.RemoveItem(PS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }

        temp = SS.GetHeldItem();
        if (temp != null)
        {
            temp = SS.RemoveItem(SS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }

        temp = TS.GetHeldItem();
        if (temp != null)
        {
            temp = TS.RemoveItem(TS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }

        temp = GS.GetHeldItem();
        if (temp != null)
        {
            temp = GS.RemoveItem(GS.GetHeldItem().stackCount);
            Destroy(temp.gameObject);
        }
    }

    public void SetPrimary()
    {
        if (PS.GetHeldItem() != null)
        {
            if (PS.GetHeldItem() != EquiptedItem)
            {
                PS.GetHeldItem().EquipItem(Back1);
            }
        }
    }

    public void SetSecondary()
    {
        if (SS.GetHeldItem() != null)
        {
            if (SS.GetHeldItem() != EquiptedItem)
            {
                SS.GetHeldItem().EquipItem(Back2);
            }
        }
    }

    public void SetTertiary()
    {
        if (TS.GetHeldItem() != null)
        {
            if (TS.GetHeldItem() != EquiptedItem)
            {
                TS.GetHeldItem().EquipItem(Belt);
            }
        }
    }

    public void UnEquipPrimary()
    {
        if (PS.GetHeldItem() != null)
        {
            if (PS.GetHeldItem() == EquiptedItem)
            {
                IA.UnEquip();
            }
            else
            {
                PS.GetHeldItem().UnEquipItem(ItemTransform);
            }
        }
    }

    public void UnEquipSecondary()
    {
        if (SS.GetHeldItem() != null)
        {
            if (SS.GetHeldItem() == EquiptedItem)
            {
                IA.UnEquip();
            }
            else
            {
                SS.GetHeldItem().UnEquipItem(ItemTransform);
            }
        }
    }

    public void UnEquipTertiary()
    {
        if (TS.GetHeldItem() != null)
        {
            if (TS.GetHeldItem() == EquiptedItem)
            {
                IA.UnEquip();
            }
            else
            {
                TS.GetHeldItem().UnEquipItem(ItemTransform);
            }
        }
    }
}
