using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPosition : MonoBehaviour {

    /*
    [SerializeField]
    private OVRGrabbable GrabbedObject;

    private Vector3 InitialPosition;
    private Quaternion InitialRotation;

    private float PosLerp = 0;
    private float RotLerp = 0;
    private Rigidbody RB;
    private Transform InitialParent;

	// Use this for initialization
	void Start () {
        InitialPosition = transform.localPosition;
        InitialRotation = transform.localRotation;
        RB = GetComponent<Rigidbody>();
        InitialParent = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GrabbedObject.isGrabbed)
        {
            //Debug.Log(gameObject.name + " Not Grabbed");
            transform.parent = InitialParent;
            if (InitialPosition != transform.localPosition)
            {
                //Debug.Log(gameObject.name + " Not Grabbed && Fixing");
                // Move back to initial Position
                //PosLerp += Time.deltaTime;
                //if (PosLerp > 1)
                //    PosLerp = 1;
                //transform.localPosition = Vector3.Lerp(transform.localPosition, InitialPosition, PosLerp);
                transform.localPosition = InitialPosition;
            }
            if (PosLerp >= 1f)
            {
                //Debug.Log(gameObject.name + " Not Grabbed && Fixed");
                PosLerp = 0;
            }
            if (InitialRotation != transform.localRotation)
            {
                // Move back to initial Rotation
                //RotLerp += Time.deltaTime;
                //if (RotLerp > 1)
                //    RotLerp = 1;
                //transform.localRotation = Quaternion.Lerp(transform.localRotation, InitialRotation, RotLerp);
                transform.localRotation = InitialRotation;
            }
            //if (RotLerp >= 1f)
            //{
            //    RotLerp = 0;
            //}
        }
        else
        {
            transform.parent = GrabbedObject.grabbedBy.transform;
            //Debug.Log(gameObject.name + " Grabbed");
            //PosLerp = 0;
            //RotLerp = 0;
        }
    }
    */
}
