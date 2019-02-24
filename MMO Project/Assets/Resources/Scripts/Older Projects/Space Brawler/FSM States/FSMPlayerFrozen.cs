using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMPlayerFrozen : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;

    private FreezeTagManager FTM;

    private Camera Cam;

    public override void Init()
    {
        //Debug.Log("Freeze Init");
        m_stateId = FSMStateIDs.StateIds.FSM_PlayerFrozen;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        FTM = FindObjectOfType<FreezeTagManager>();
        Cam = SMOwner.Cam;
    }

    public override void OnEnter()
    {
        //Debug.Log("Freeze Enter");
        CharacterRenderer.material = StateMat;
        CharacterAgent.StopMovement();
        FTM.AddToFrozen();
        Cam.gameObject.SetActive(true);
        CharacterAgent.ResetMaxTurnForce(10);
    }

    public override void OnExit()
    {
        //Debug.Log("Freeze Exit");
        //CharacterAgent.Jump();
        CharacterAgent.StopMovement();
        FTM.RemoveFromFrozen();
        Cam.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        if (!FTM)
        {
            FTM = FindObjectOfType<FreezeTagManager>();
            FTM.AddToFrozen();
        }
        //Debug.Log("Freeze Update");

        // Rotate Character
        //CharacterAgent.YawTurn();
        CharacterAgent.YawTurn(Input.GetAxis("Horizontal"));
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
            if (c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_UnFreeze || c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_TagFlee)
            {
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_PlayerFlee;
            }
        }
    }
}
