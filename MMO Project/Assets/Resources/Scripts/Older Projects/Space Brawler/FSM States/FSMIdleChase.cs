using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMIdleChase : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;
    private GameObject Target;

    [SerializeField]
    private float FieldOfView = 90;
    private float FOVHelper = 1 / (180 * 2);

    public override void Init()
    {
        //Debug.Log("Freeze Init");
        m_stateId = FSMStateIDs.StateIds.FSM_IdleChase;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;
    }

    public override void OnEnter()
    {
        //Debug.Log("Freeze Enter");
        //CharacterRenderer.material = StateMat;
        CharacterAgent.StopMovement();
        Target = SMOwner.Target;
    }

    public override void OnExit()
    {
        //Debug.Log("Freeze Exit");
        //CharacterAgent.Jump();
        CharacterAgent.StopMovement();
    }

    public override void OnUpdate()
    {
        //Debug.Log("Freeze Update");

        // Rotate Character
        //CharacterAgent.YawTurn();
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
        }
    }

    public override void OnShutdown()
    {
        //Debug.Log("Freeze Shutdown");
        CharacterAgent.StopMovement();
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
