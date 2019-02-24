using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : MonoBehaviour {

    [SerializeField]
    private Transform AimingPosition;
    [SerializeField]
    private Transform AimingTarget;
    private Transform Target;

    [SerializeField]
    private LayerMask PlayerLayers;
    [SerializeField]
    private LayerMask HitLayers;
    [HideInInspector]
    public bool LineOfSight;

    private RaycastHit AimingLOSPlayer;
    private RaycastHit AimingLOSHit;
    private RaycastHit LauncherLOSPlayer;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // If we are not aiming at the ship
        if (!Physics.Raycast(AimingPosition.position, AimingPosition.forward, out AimingLOSPlayer, 15f, PlayerLayers))
        {
            // Find if we get a target
            if (Physics.Raycast(AimingPosition.position, AimingPosition.forward, out AimingLOSHit, 300f, HitLayers))
            {
                Target = AimingLOSHit.collider.gameObject.transform;
            }
            else // if no target, assume the target is the aiming target
            {
                Target = AimingTarget;
            }
            // Look at the target
            transform.LookAt(Target);

            // If the guns are not aiming at the ship
            if (!Physics.Raycast(transform.position, transform.forward, out LauncherLOSPlayer, 30f, HitLayers))
            {
                LineOfSight = true;
            }
            else
            {
                LineOfSight = false;
            }
        }
        else
        {
            LineOfSight = false;
        }
    }
}
