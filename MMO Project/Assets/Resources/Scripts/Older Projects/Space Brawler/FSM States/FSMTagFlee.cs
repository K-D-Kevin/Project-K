using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTagFlee : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;

    private GameObject Target;

    [SerializeField]
    private float FieldOfView = 90;
    private float FOVHelper = 1 / (180 * 2);

    [SerializeField]
    private float RunRange = 5;
    [SerializeField]
    private float walkRange = 12;

    // eyes
    [SerializeField]
    private LayerMask WallLayer;

    private RaycastHit RRay;
    private RaycastHit LRay;

    [SerializeField]
    private float SightRange = 8f;

    private Transform RightEye;
    private Transform LeftEye;
    private bool RightHit = false;
    private bool LeftHit = false;

    public override void Init()
    {
        //Debug.Log("FLEE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_TagFlee;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;
        RightEye = SMOwner.RightEye;
        LeftEye = SMOwner.LeftEye;
    }

    public override void OnEnter()
    {
        //Debug.Log("Tag FLEE Enter");
        CharacterRenderer.material = StateMat;
        CharacterAgent.StopMovement();
        Target = SMOwner.Target;
    }

    public override void OnExit()
    {
        //Debug.Log("FLEE Exit");
        CharacterAgent.Jump();
    }

    public override void OnUpdate()
    {
        //Debug.Log("FLEE Update");

        // FLEE Character
        // Calculate direction to target
        if (Target != null)
        {
            Vector3 Direction = Target.transform.position - SMOwner.transform.position;
            Vector3 NormDirection = Direction.normalized;
            float dotToTarget = Vector3.Dot(SMOwner.transform.right, NormDirection);
            //Debug.Log("Dot To Target: " + dotToTarget);
            RightHit = Physics.Raycast(RightEye.position, RightEye.forward, out RRay, SightRange, WallLayer);
            LeftHit = Physics.Raycast(LeftEye.position, LeftEye.forward, out LRay, SightRange, WallLayer);
            // Add some sight look forward and side to side
            if (RightHit || LeftHit)
            {
                
                if (RightHit && !LeftHit) // Near a wall to AI right and forward
                {
                    // Turn Left
                    CharacterAgent.StopTurnMovement();
                    CharacterAgent.Turn(-(1 - RRay.distance / SightRange) - 0.1f);
                }
                else if (!RightHit && LeftHit) // Near a wall to AI left and forward
                {
                    // Turn right
                    CharacterAgent.StopTurnMovement();
                    CharacterAgent.Turn(1 - LRay.distance / SightRange + 0.1f);
                }
                else
                {
                    // if closer to the right
                    if (RRay.distance < LRay.distance)
                    {
                        CharacterAgent.StopTurnMovement();
                        CharacterAgent.Turn(-(1 - RRay.distance / SightRange) - 0.1f);
                    }
                    else
                    {
                        CharacterAgent.StopTurnMovement();
                        CharacterAgent.Turn(1 - LRay.distance / SightRange + 0.1f);
                    }
                }
                Debug.DrawRay(RightEye.position, RightEye.forward * RRay.distance, Color.blue);
                Debug.DrawRay(LeftEye.position, LeftEye.forward * LRay.distance, Color.red);
            }
            // Rotate towards Character
            else if (SMOwner.Target)
            {
                if (dotToTarget > FieldOfView * FOVHelper)
                {
                    CharacterAgent.StopTurnMovement();
                    CharacterAgent.Turn(-dotToTarget);
                }
                else if (dotToTarget < -FieldOfView * FOVHelper)
                {
                    CharacterAgent.StopTurnMovement();
                    CharacterAgent.Turn(-dotToTarget);
                }
                else
                {
                    CharacterAgent.StopTurnMovement();
                }

                //SMOwner.transform.LookAt(Target.transform);
            }
            else
            {
                CharacterAgent.StopTurnMovement();
            }

            if (Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_Freeze)
            {
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_UnFreeze;
            }

            //Move Character
            if (Direction.magnitude <= RunRange || Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_Freeze)
            {
                if (!CharacterAgent.UseSpeedAsForce)
                {
                    CharacterAgent.MoveLinear(-NormDirection.z);
                    CharacterAgent.Strafe(-NormDirection.x);
                }
                else
                {
                    CharacterAgent.MoveLinear(1);
                }
            }
            else if (Direction.magnitude <= walkRange || Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_Freeze)
            {
                if (!CharacterAgent.UseSpeedAsForce)
                {
                    CharacterAgent.MoveLinear(-NormDirection.z / 2);
                    CharacterAgent.Strafe(-NormDirection.x / 2);
                }
                else
                {
                    CharacterAgent.MoveLinear(1);
                }
            }
            else
            {
                CharacterAgent.StopMovement();
                Target = null;
                SMOwner.Target = null;
            }
        }
        else
        {
            Target = SMOwner.Target;
            // Look for target
            FSMStateManager[] tempArray = FindObjectsOfType<FSMStateManager>();
            List<FSMStateManager> targetableList = new List<FSMStateManager>();
            List<FSMStateManager> UnFreezingAi = new List<FSMStateManager>();
            List<FSMStateManager> FrozenAi = new List<FSMStateManager>();
            foreach (FSMStateManager state in tempArray)
            {
                if (state.m_currentState == FSMStateIDs.StateIds.FSM_TagChase || state.m_currentState == FSMStateIDs.StateIds.FSM_PlayerChase)
                {
                    targetableList.Add(state);
                }
                if (state.m_currentState == FSMStateIDs.StateIds.FSM_UnFreeze)
                {
                    UnFreezingAi.Add(state);
                }
                if (state.m_currentState == FSMStateIDs.StateIds.FSM_Freeze || state.m_currentState == FSMStateIDs.StateIds.FSM_PlayerFrozen)
                {
                    FrozenAi.Add(state);
                }
            }
            if (UnFreezingAi.Count < FrozenAi.Count && FrozenAi.Count > 0)
            {
                int Rand = Random.Range(0, FrozenAi.Count);
                Target = FrozenAi[Rand].gameObject;
                SMOwner.Target = Target;
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_UnFreeze;
            }
            else if (targetableList.Count > 0)
            {
                int Rand = Random.Range(0, targetableList.Count);
                Target = targetableList[Rand].gameObject;
                SMOwner.Target = Target;
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_TagFlee;
            }
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
        //Debug.Log("Collision");
        if (c.gameObject.GetComponent<FSMStateManager>() != null)
        {
            if (c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_TagChase || c.gameObject.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_PlayerChase)
            {
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_Freeze;
            }
        }
    }
}

