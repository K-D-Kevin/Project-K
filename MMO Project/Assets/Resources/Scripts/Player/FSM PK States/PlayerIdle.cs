using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material IdleMat;
    private MeshRenderer CharacterRenderer;
    private Camera Cam;

    public override void Init()
    {
        m_stateId = FSMStateIDs.StateIds.FSM_PlayerIdle;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Cam = SMOwner.Cam;
        //Debug.Log("IDLE Init");
    }

    public override void OnEnter()
    {
        //Debug.Log("IDLE Enter");
        //CharacterRenderer.material = IdleMat;
        CharacterAgent.StopMovement();
        Cam.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        //Debug.Log("IDLE Exit");
        //CharacterAgent.Jump();
        CharacterAgent.StopMovement();
        Cam.gameObject.SetActive(false);
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
