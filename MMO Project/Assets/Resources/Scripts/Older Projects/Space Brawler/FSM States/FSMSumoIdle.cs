using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMSumoIdle : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;

    private NavMeshAgent NA;

    private SumoManager SumoM;
    private float SpeedOnEnter;

    public override void Init()
    {
        //Debug.Log("Freeze Init");
        m_stateId = FSMStateIDs.StateIds.FSM_SumoIdle;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();

        SumoM = FindObjectOfType<SumoManager>();

        NA = SMOwner.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        CharacterRenderer.material = StateMat;
        SpeedOnEnter = NA.speed;
        NA.speed = 0;
    }

    public override void OnExit()
    {
        NA.speed = SpeedOnEnter;
    }

    public override void OnUpdate()
    {
        //Debug.Log("Current NA velocity: " + NA.velocity.magnitude);
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
            SumoM.AddScorePlayer();
            Destroy(SMOwner.gameObject);
        }
    }
}
