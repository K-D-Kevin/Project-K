using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMNavSumoFlee : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material FleeMat;
    private MeshRenderer CharacterRenderer;
    private GameObject Target;

    private PointScoring[] PointList;
    private PointScoring LowestScorePoint;
    private SumoManager SumoM;
    private int AiNum = 0;

    private NavMeshAgent NA;

    public override void Init()
    {
        //Debug.Log("FLEE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_NavSumoFlee;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;

        PointList = FindObjectsOfType<PointScoring>();
        SumoM = FindObjectOfType<SumoManager>();

        NA = SMOwner.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        //Debug.Log("FLEE Enter");
        CharacterRenderer.material = FleeMat;
        Target = SMOwner.Target;
        AiNum = SumoM.GetAiNum();
    }

    public override void OnExit()
    {
        //Debug.Log("FLEE Exit");
    }

    public override void OnUpdate()
    {
        //Debug.Log("FLEE Update");

        // FLEE Character
        // Calculate direction to target
        if (AiNum == 1)
        {
            float DesiredScore = 0;
            foreach (PointScoring PS in PointList)
            {
                float score = PS.GetPrimaryScore();
                if (score > DesiredScore)
                {
                    DesiredScore = score;
                    LowestScorePoint = PS;
                }
            }

            //Debug.Log("Desired Score: " + DesiredScore);
            NA.SetDestination(LowestScorePoint.transform.position);
        }
        else if (AiNum == 2)
        {
            float DesiredScore = 0;
            foreach (PointScoring PS in PointList)
            {
                float score = PS.GetPrimaryScore();
                if (score > DesiredScore)
                {
                    DesiredScore = score;
                    LowestScorePoint = PS;
                }
            }

            //Debug.Log("Desired Score: " + DesiredScore);
            NA.SetDestination(LowestScorePoint.transform.position);
        }
    }

    public override void OnShutdown()
    {
        //Debug.Log("FLEE Shutdown");
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
