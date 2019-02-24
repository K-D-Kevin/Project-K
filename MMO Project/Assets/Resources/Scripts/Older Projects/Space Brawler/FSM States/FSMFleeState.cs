using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMFleeState : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material FleeMat;
    private MeshRenderer CharacterRenderer;
    private GameObject Target;

    [SerializeField]
    private float FieldOfView = 90;
    private float FOVHelper = 1 / (180 * 2);

    public override void Init()
    {
        //Debug.Log("FLEE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_Flee;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;
    }

    public override void OnEnter()
    {
        //Debug.Log("FLEE Enter");
        CharacterRenderer.material = FleeMat;
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

            // Rotate towards Character
            if (SMOwner.Target)
            {
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

                //SMOwner.transform.LookAt(Target.transform);
            }
            else
            {
                CharacterAgent.StopTurnMovement();
            }


            //Move Character
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
        else
        {
            Target = SMOwner.Target;
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
