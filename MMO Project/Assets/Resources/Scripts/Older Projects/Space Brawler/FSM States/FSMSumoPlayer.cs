using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSumoPlayer : FSMState
{

    private Agent CharacterAgent;
    private Rigidbody RB;

    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;

    private SumoManager SumoM;

    [SerializeField]
    private float MoveAcceleration = 12;
    private float VerticalAxis = 0;
    private float HorizontalAxis = 0;

    [SerializeField]
    private float MaxSpeed = 8;

    public override void Init()
    {
        //Debug.Log("Freeze Init");
        m_stateId = FSMStateIDs.StateIds.FSM_SumoPlayer;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();

        SumoM = FindObjectOfType<SumoManager>();
        RB = SMOwner.GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        CharacterRenderer.material = StateMat;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        VerticalAxis = Input.GetAxis("Vertical");
        HorizontalAxis = Input.GetAxis("Horizontal");
        Vector3 Force;
        if (Mathf.Abs(RB.velocity.magnitude) < MaxSpeed)
        {
            Force = new Vector3(HorizontalAxis, 0, VerticalAxis) * MoveAcceleration;
        }
        else
        {
            Force = Vector3.zero;
        }
        RB.AddForce(Force, ForceMode.Acceleration);
    }

    public override void OnShutdown()
    {
    }

    public override void GetStateManager(FSMStateManager sm)
    {
        SMOwner = sm;
    }

    public override void FSMOnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Wall")
        {
            SumoM.AddScoreAi();
            Destroy(SMOwner.gameObject);
        }
    }
}
