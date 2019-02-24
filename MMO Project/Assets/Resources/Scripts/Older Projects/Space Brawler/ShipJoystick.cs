using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipJoystick : MonoBehaviour {

    /*
    public OVRGrabbable Handle;

    [HideInInspector]
    public bool Shoot = false;
    [HideInInspector]
    public bool Button2Pressed = false;
    [HideInInspector]
    public bool Button1Pressed = false;
    [HideInInspector]
    public bool Trigger1Pressed = false;
    [HideInInspector]
    public bool Trigger2Pressed = false;
    [HideInInspector]
    public float Trigger1Axis = 0;

    [SerializeField]
    private ReturnToPosition RP;

    [SerializeField]
    private Transform ChildReference;

    private Vector3 InitialEulerAngle = Vector3.zero;
    private Vector3 CurrentEulerAngle = Vector3.zero;
    //[HideInInspector]
    public Vector3 DifferenceEulerAngles = Vector3.zero;
    private bool GrabbedNow = false;

    // Update is called once per frame
    void Update () {
		if (Handle.isGrabbed)
        {
            if (!GrabbedNow)
            {
                InitialEulerAngle = ChildReference.localEulerAngles;
                CurrentEulerAngle = InitialEulerAngle;
                GrabbedNow = true;
            }
            else
            {
                CurrentEulerAngle = ChildReference.localEulerAngles;
                float DifX = CurrentEulerAngle.x - InitialEulerAngle.x < 0 ? CurrentEulerAngle.x - InitialEulerAngle.x + 360
                    : CurrentEulerAngle.x - InitialEulerAngle.x;
                float DifY = CurrentEulerAngle.y - InitialEulerAngle.y < 0 ? CurrentEulerAngle.y - InitialEulerAngle.y + 360
                    : CurrentEulerAngle.y - InitialEulerAngle.y;
                float DifZ = CurrentEulerAngle.z - InitialEulerAngle.z < 0 ? CurrentEulerAngle.z - InitialEulerAngle.z + 360
                    : CurrentEulerAngle.z - InitialEulerAngle.z;

                DifferenceEulerAngles = new Vector3(DifX, DifY, DifZ);
            }
            //RP.enabled = false;
            if (Handle.grabbedBy.LRTouch() == OVRInput.Controller.LTouch)
            {
                Button2Pressed = OVRInput.Get(OVRInput.Button.Four);
                Shoot = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
                Button1Pressed = OVRInput.Get(OVRInput.Button.Three);
                Trigger2Pressed = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) >= 0.5f;
                Trigger1Pressed = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) >= 0.5f;
                Trigger1Axis = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            }
            else if (Handle.grabbedBy.LRTouch() == OVRInput.Controller.RTouch)
            {
                Button2Pressed = OVRInput.Get(OVRInput.Button.Two);
                Shoot = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
                Button1Pressed = OVRInput.Get(OVRInput.Button.One);
                Trigger2Pressed = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) >= 0.5f;
                Trigger1Pressed = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) >= 0.5f;
                Trigger1Axis = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
            }
        }
        else
        {
            //RP.enabled = true;
            Trigger1Axis = 0;
            GrabbedNow = false;
            DifferenceEulerAngles = Vector3.zero;
            InitialEulerAngle = Vector3.zero;
            CurrentEulerAngle = Vector3.zero;
        }
	}
    */
}
