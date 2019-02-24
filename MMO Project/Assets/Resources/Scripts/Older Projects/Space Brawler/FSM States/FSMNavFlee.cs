using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMNavFlee : FSMState
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
    private NavMeshHit RRay;
    private NavMeshHit LRay;

    [SerializeField]
    private float SightRange = 1f;

    private Transform RightEye;
    private Transform LeftEye;
    private bool RightHit = false;
    private bool LeftHit = false;
    private bool PrioritizeTurning = false;
    // Frames that right / left eyes don't hit the navmesh
    private int RightEyeEdgeCounter = 0;
    private int LeftEyeEdgeCounter = 0;

    public override void Init()
    {
        //Debug.Log("FLEE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_NavMeshFlee;
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
        RightHit = !NavMesh.Raycast(RightEye.position, RightEye.forward, out RRay, NavMesh.AllAreas);
        LeftHit = !NavMesh.Raycast(LeftEye.position, LeftEye.forward, out LRay, NavMesh.AllAreas);
        Debug.DrawRay(RightEye.position, RightEye.forward * RRay.distance, Color.blue);
        Debug.DrawRay(LeftEye.position, LeftEye.forward * LRay.distance, Color.red);

        if (Target != null)
        {
            if (!RightHit || !LeftHit)
            {
                PrioritizeTurning = true;
                if (!RightHit)
                {
                    RightEyeEdgeCounter++;
                }
                else
                {
                    RightEyeEdgeCounter = 0;
                }

                if (!LeftHit)
                {
                    RightEyeEdgeCounter++;
                }
                else
                {
                    RightEyeEdgeCounter = 0;
                }
            } // If eyes don't hit a navmesh, prioritize turning, higher the left / right counter determines the turning direction if they are the same turn left

            if (PrioritizeTurning)
            {
                if (RightEyeEdgeCounter >= LeftEyeEdgeCounter)
                {
                    // Turn Left
                    CharacterAgent.Turn(-1);
                }
                else
                {
                    // Turn Right
                    CharacterAgent.Turn(1);
                }
            }
            else
            {
                Vector3 Direction = Target.transform.position - SMOwner.transform.position;
                Vector3 NormDirection = Direction.normalized;
                float dotToTarget = Vector3.Dot(SMOwner.transform.right, NormDirection);

                if (dotToTarget > FieldOfView * FOVHelper)
                {
                    CharacterAgent.Turn(-dotToTarget);
                }
                else if (dotToTarget < -FieldOfView * FOVHelper)
                {
                    CharacterAgent.Turn(-dotToTarget);
                }
                else
                {
                    CharacterAgent.StopTurnMovement();
                }

                //Move Character
                if (Direction.magnitude <= RunRange)
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
                else if (Direction.magnitude <= walkRange)
                {
                    float PorportionalSpeed = (walkRange - Direction.magnitude) / (walkRange - RunRange);
                    if (!CharacterAgent.UseSpeedAsForce)
                    {
                        CharacterAgent.MoveLinear(-NormDirection.z * PorportionalSpeed);
                        CharacterAgent.Strafe(-NormDirection.x * PorportionalSpeed);
                    }
                    else
                    {
                        CharacterAgent.MoveLinear(1);
                    }
                }
                else
                {
                    CharacterAgent.StopMovement();
                }
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
    }
}
