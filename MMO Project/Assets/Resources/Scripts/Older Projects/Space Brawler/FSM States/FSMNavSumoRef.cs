using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMNavSumoRef : FSMState
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

    [SerializeField]
    private float MinMoveScore = 1;
    private float distanceScore;

    public override void Init()
    {
        //Debug.Log("Ref Init");
        m_stateId = FSMStateIDs.StateIds.FSM_NavSumoRef;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;

        PointList = FindObjectsOfType<PointScoring>();
        SumoM = FindObjectOfType<SumoManager>();
        NA = SMOwner.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        //Debug.Log("Ref Enter");
        CharacterRenderer.material = FleeMat;
        CharacterAgent.StopMovement();
        Target = SMOwner.Target;
        AiNum = SumoM.GetAiNum();
    }

    public override void OnExit()
    {
        //Debug.Log("Ref Exit");
        CharacterAgent.Jump();
    }

    public override void OnUpdate()
    {
        //Debug.Log("Ref Update");

        // Ref Character
        
        if (AiNum == 1)
        {
            float DesiredScore = 0;
            foreach (PointScoring PS in PointList)
            {
                distanceScore = PS.GetTertiaryScore();
                float score = PS.GetPrimaryScore() + PS.GetSecondaryScore() - distanceScore;
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
                distanceScore = PS.GetExtraScore();
                float score = PS.GetPrimaryScore() + PS.GetSecondaryScore() + PS.GetTertiaryScore() - distanceScore;
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
        //Debug.Log("Ref Shutdown");
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
