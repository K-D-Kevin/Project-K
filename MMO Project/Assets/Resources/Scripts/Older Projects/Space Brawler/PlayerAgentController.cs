using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class PlayerAgentController : AgentController {

    // Layers and Raycasts
    [SerializeField]
    private LayerMask GroundLayer;

    // Agent Body
    [SerializeField]
    private Transform Feet;

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        A.MoveLinear(-Input.GetAxis("Horizontal"));
        A.Strafe(Input.GetAxis("Vertical"));
        if (Physics.Raycast(-Feet.position, -A.T.up, 0.1f, GroundLayer) && Input.GetKeyDown(KeyCode.Space))
            A.Jump();
	}
}
