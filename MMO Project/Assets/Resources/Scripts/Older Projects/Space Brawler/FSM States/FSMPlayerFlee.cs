using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMPlayerFlee : FSMState
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
        m_stateId = FSMStateIDs.StateIds.FSM_PlayerFlee;
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
        Cam.gameObject.SetActive(true);
        CharacterAgent.ResetMaxTurnSpeed(2);
        CharacterAgent.ResetMaxLinearSpeed(8);
        CharacterAgent.ResetMaxTurnForce(10);
    }

    public override void OnExit()
    {
        //Debug.Log("Freeze Exit");
        //CharacterAgent.Jump();
        CharacterAgent.StopMovement();
        Cam.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        //Debug.Log("Freeze Update");

        // Rotate Character
        CharacterAgent.YawTurn(Input.GetAxis("Horizontal"));
        CharacterAgent.MoveLinear(Input.GetAxis("Vertical"));
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
            if (c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_TagChase)
            {
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_PlayerFrozen;
            }
        }
    }
}
