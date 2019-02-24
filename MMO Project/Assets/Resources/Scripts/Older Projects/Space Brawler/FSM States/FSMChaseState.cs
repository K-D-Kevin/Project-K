using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMChaseState : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material ChaseMat;
    private MeshRenderer CharacterRenderer;
    private GameObject Target;

    [SerializeField]
    private float FieldOfView = 90;
    private float FOVHelper = 1 / (180 * 2);

    public override void Init()
    {
        //Debug.Log("CHASE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_Chase;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;
    }

    public override void OnEnter()
    {
        //Debug.Log("CHASE Enter");
        //CharacterRenderer.material = ChaseMat;
        CharacterAgent.StopMovement();
        Target = SMOwner.Target;
    }

    public override void OnExit()
    {
        //Debug.Log("CHASE Exit");
        CharacterAgent.Jump();
    }

    public override void OnUpdate()
    {
        //Debug.Log("CHASE Update");

        // Chase Character
        // Calculate direction
        if (Target != null)
        {
            Vector3 Direction = Target.transform.position - SMOwner.transform.position;
            Vector3 NormDirection = Direction.normalized;
            float dotToTargetX = Vector3.Dot(SMOwner.transform.right, NormDirection);
            float dotToTargetY = Vector3.Dot(-SMOwner.transform.up, NormDirection);
            if (Direction.z <= 0)
            {
                dotToTargetY = -dotToTargetY;
            }
            if (Direction.x <= 0)
            {
                dotToTargetX = 0;
                dotToTargetY = -dotToTargetY;
            }
            //Debug.Log("Dot To Target: " + dotToTarget);

            // Rotate towards Character
            if (SMOwner.Target)
            {
                if (dotToTargetX > FieldOfView * FOVHelper || dotToTargetX < -FieldOfView * FOVHelper)
                {
                    CharacterAgent.Turn(dotToTargetX);
                }

                if (dotToTargetY > FieldOfView * FOVHelper || dotToTargetY < -FieldOfView * FOVHelper)
                {
                    CharacterAgent.PitchTurn(dotToTargetY);
                }

                //SMOwner.transform.LookAt(Target.transform);
            }
            else
            {
                CharacterAgent.StopTurnMovement();
            }


            // Move Character
            if (!CharacterAgent.UseSpeedAsForce)
            {
                CharacterAgent.MoveLinear(NormDirection.z);
                CharacterAgent.Strafe(NormDirection.x);
                CharacterAgent.Rise(NormDirection.y);
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
        //Debug.Log("CHASE Shutdown");
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
