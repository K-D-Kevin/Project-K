using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimicInventorySlot : MonoBehaviour {

    // Where to copy
    [SerializeField]
    private RawImage ImageSpace;
    [SerializeField]
    private RawImage SlotBackground;
    [SerializeField]
    private RawImage SlotForeground;
    [SerializeField]
    private RawImage NumberBackground;
    [SerializeField]
    private Text NumberText;

    // Defaults
    [SerializeField]
    private Color BackgroundDefaultColor;
    [SerializeField]
    private Color ForegroundDefaultColor;
    [SerializeField]
    private Color ImageDefaultColor;

    // Items to copy
    private InventorySlot CopySlot = null;
    private Item CopyItem = null;
    private Texture CopyTexture = null;
    private int CopyItemCount = 0;
    private Color CopyBackgroundColor = Color.magenta; // MEGENTA is the null value for images
    private RawImage BackgroundImage = null;
    private Color CopyForegroundColor = Color.magenta;
    private RawImage ForegroundImage = null;


    public void CopyThisSlot(InventorySlot IS)
    {
        CopySlot = IS;
        if (CopySlot != null)
        {
            CopyItem = CopySlot.GetHeldItem();
            if (CopyItem != null)
            {
                CopyTexture = CopyItem.GetSprite();
                CopyItemCount = CopyItem.stackCount;
            }
            BackgroundImage = CopySlot.GetBackground();
            CopyBackgroundColor = BackgroundImage.color;
            ForegroundImage = CopySlot.GetForeground();
            CopyForegroundColor = ForegroundImage.color;
        }
    }

    public void ApplyCopy()
    {
        if (CopyBackgroundColor != Color.magenta)
        {
            SlotBackground.color = CopyBackgroundColor;
        }
        else
        {
            SlotBackground.color = BackgroundDefaultColor;
        }

        if (CopyForegroundColor != Color.magenta)
        {
            SlotForeground.color = CopyForegroundColor;
        }
        else
        {
            SlotForeground.color = ForegroundDefaultColor;
        }

        if (CopyItem != null)
        {
            ImageSpace.color = Color.white;
            ImageSpace.texture = CopyTexture;
        }
        else
        {
            ImageSpace.color = ImageDefaultColor;
            ImageSpace.texture = null;
            CopyItemCount = 0;
        }

        if (CopyItemCount > 1)
        {
            NumberText.gameObject.SetActive(true);
            NumberText.text = "" + CopyItemCount;
            NumberBackground.color = Color.black;
        }
        else
        {
            NumberText.text = "";
            NumberText.gameObject.SetActive(false);
            NumberBackground.color = Color.clear;
        }
    }

    public void DefualtReset()
    {
        CopySlot = null;
        CopyItem = null;
        CopyTexture = null;
        CopyItemCount = -1;
        CopyBackgroundColor = Color.magenta;
        BackgroundImage = null;
        CopyForegroundColor = Color.magenta;
        ForegroundImage = null;
        ApplyCopy();
}
}
