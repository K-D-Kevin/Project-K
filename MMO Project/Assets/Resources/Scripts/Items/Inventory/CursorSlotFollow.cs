using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorSlotFollow : MonoBehaviour {

    // Positions 
    [SerializeField]
    private float Y_Offset = -10;
    private float yOffset;
    [SerializeField]
    private float X_Offset = 10;
    private float xOffset;
    public Vector3 MousePos;
    private Vector3 DesiredPos;
    private Vector3 OriginalPos;
    [SerializeField]
    private float BufferZone = 40;

    // Screen elements
    private float ScreenH;
    private float ScreenW;

    // UI elements
    [SerializeField]
    private RectTransform RT;

    private bool active = true;

	// Use this for initialization
	void Start () {
        yOffset = Y_Offset;
        xOffset = X_Offset;
        ScreenH = Screen.height;
        ScreenW = Screen.width;
        OriginalPos = RT.position;

    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            MousePos = Input.mousePosition;
            GetDesiredPosition();
            SetPosition();
        }
        else if (DesiredPos != OriginalPos)
        {
            DesiredPos = OriginalPos;
            RT.position = DesiredPos;
        }
	}

    private void GetDesiredPosition()
    {
        if (MousePos.y + Mathf.Abs(Y_Offset) + BufferZone >= ScreenH)
        {
            yOffset = -Mathf.Abs(Y_Offset) * ScreenH / 600;
        }
        else if (MousePos.y - Mathf.Abs(Y_Offset) - BufferZone <= 0)
        {
            yOffset = Mathf.Abs(Y_Offset) * ScreenH / 600;
        }
        else
        {
            yOffset = Y_Offset * ScreenH / 600;
        }

        if (MousePos.x + Mathf.Abs(X_Offset) + BufferZone >= ScreenW)
        {
            xOffset = -Mathf.Abs(X_Offset) * ScreenW / 1200;
        }
        else if (MousePos.x - Mathf.Abs(X_Offset) - BufferZone <= 0)
        {
            xOffset = Mathf.Abs(X_Offset) * ScreenW / 1200;
        }
        else
        {
            xOffset = X_Offset * ScreenW / 1200;
        }
    }

    private void SetPosition()
    {
        DesiredPos = MousePos + new Vector3(xOffset, yOffset, 0);
        RT.position = DesiredPos;
    }

    public void SetActive(bool act = true)
    {
        active = act;
    }
}
