using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMNavSumoChase : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material ChaseMat;
    private MeshRenderer CharacterRenderer;
    private GameObject Target;

    private NavMeshAgent NA;
    private SumoManager SumoM;


    public override void Init()
    {
        //Debug.Log("CHASE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_NavSumoChase;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;

        NA = SMOwner.GetComponent<NavMeshAgent>();
        SumoM = FindObjectOfType<SumoManager>();
    }

    public override void OnEnter()
    {
        //Debug.Log("CHASE Enter");
        CharacterRenderer.material = ChaseMat;
        Target = SMOwner.Target;
    }

    public override void OnExit()
    {
        //Debug.Log("CHASE Exit");
    }

    public override void OnUpdate()
    {
        //Debug.Log("CHASE Update");

        // Chase Character
        // Calculate direction
        if (Target != null)
        {
            NA.SetDestination(Target.transform.position);
            //Debug.Log("Current NA velocity: " + NA.velocity.magnitude);
        }
        else
        {
            Target = SMOwner.Target;
        }
    }

    public override void OnShutdown()
    {
        //Debug.Log("CHASE Shutdown");
        CharacterAgent.Jump();
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
