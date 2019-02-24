using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMFreezeState : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;

    private FreezeTagManager FTM;

    public override void Init()
    {
        //Debug.Log("Freeze Init");
        m_stateId = FSMStateIDs.StateIds.FSM_Freeze;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        FTM = FindObjectOfType<FreezeTagManager>();
    }

    public override void OnEnter()
    {
        //Debug.Log("Freeze Enter");
        CharacterRenderer.material = StateMat;
        CharacterAgent.StopMovement();
        FTM.AddToFrozen();
    }

    public override void OnExit()
    {
        //Debug.Log("Freeze Exit");
        //CharacterAgent.Jump();
        CharacterAgent.StopMovement();
        FTM.RemoveFromFrozen();
    }

    public override void OnUpdate()
    {
        //Debug.Log("Freeze Update");

        // Rotate Character
        //CharacterAgent.YawTurn();
        CharacterAgent.StopMovement();
    }

    public override void OnShutdown()
    {
        //Debug.Log("Freeze Shutdown");
        CharacterAgent.StopMovement();
        CharacterAgent.Jump();
    }

    public override void GetStateManager(FSMStateManager sm)
    {
        SMOwner = sm;
    }

    public override void FSMOnCollisionEnter(Collision c)
    {
        //Debug.Log("Collision");
        if (c.gameObject.GetComponent<FSMStateManager>() != null)
        {
            if (c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_UnFreeze || c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_TagFlee || c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_PlayerFlee)
            {
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_TagFlee;
            }
        }
    }
}
