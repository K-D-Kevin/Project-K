using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMIdleState : FSMState {

    private Agent CharacterAgent;
    [SerializeField]
    private Material IdleMat;
    private MeshRenderer CharacterRenderer;

    public override void Init()
    {
        m_stateId = FSMStateIDs.StateIds.FSM_Idle;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        //Debug.Log("IDLE Init");
    }

    public override void OnEnter()
    {
        //Debug.Log("IDLE Enter");
        //CharacterRenderer.material = IdleMat;
        CharacterAgent.StopMovement();
    }

    public override void OnExit()
    {
        //Debug.Log("IDLE Exit");
        //CharacterAgent.Jump();
        CharacterAgent.StopMovement();
    }

    public override void OnUpdate()
    {
        //Debug.Log("IDLE Update " + SMOwner.gameObject.name);

        // Rotate Character
    }

    public override void OnShutdown()
    {
        //Debug.Log("IDLE Shutdown");
        CharacterAgent.StopMovement();
    }

    public override void GetStateManager(FSMStateManager sm)
    {
        SMOwner = sm;
    }
    public override void FSMOnCollisionEnter(Collision c)
    {
    }
}
