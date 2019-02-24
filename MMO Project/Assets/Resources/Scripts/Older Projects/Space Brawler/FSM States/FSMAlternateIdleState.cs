using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMAlternateIdleState : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material IdleMat;
    private MeshRenderer CharacterRenderer;

    public override void Init()
    {
        //Debug.Log("IDLE 2 Init");
        m_stateId = FSMStateIDs.StateIds.FSM_Idle2;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
    }

    public override void OnEnter()
    {
        //Debug.Log("IDLE 2 Enter");
        CharacterRenderer.material = IdleMat;
        CharacterAgent.StopMovement();
    }

    public override void OnExit()
    {
        //Debug.Log("IDLE 2 Exit");
        //CharacterAgent.Jump();
        CharacterAgent.StopMovement();
    }

    public override void OnUpdate()
    {
        //Debug.Log("IDLE 2 Update");

        // Rotate Character
        CharacterAgent.YawTurn(-1);
    }

    public override void OnShutdown()
    {
        //Debug.Log("IDLE 2 Shutdown");
        CharacterAgent.StopMovement();
        CharacterAgent.Jump();
    }

    public override void GetStateManager(FSMStateManager sm)
    {
        SMOwner = sm;
    }
    public override void FSMOnCollisionEnter(Collision c)
    {
    }
}
