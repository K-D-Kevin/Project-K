using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTagChase : FSMState
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
        //Debug.Log("CHASE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_TagChase;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;
    }

    public override void OnEnter()
    {
        //Debug.Log("CHASE Enter");
        CharacterRenderer.material = StateMat;
        CharacterAgent.StopMovement();
        Target = SMOwner.Target;
        CharacterAgent.ResetMaxLinearSpeed(6);
        CharacterAgent.ResetMaxTurnSpeed(250);
        CharacterAgent.ResetMaxTurnForce(240);
    }

    public override void OnExit()
    {
        //Debug.Log("CHASE Exit");
        CharacterAgent.Jump();
    }

    public override void OnUpdate()
    {
        //Debug.Log("Tag CHASE Update");

        // Chase Character
        // Calculate direction
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
                    CharacterAgent.StopTurnMovement();
                    CharacterAgent.Turn(dotToTarget);
                }
                else if (dotToTarget < -FieldOfView * FOVHelper)
                {
                    CharacterAgent.StopTurnMovement();
                    CharacterAgent.Turn(dotToTarget);
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


            // Move Character
            if (!CharacterAgent.UseSpeedAsForce)
            {
                CharacterAgent.MoveLinear(NormDirection.z);
                CharacterAgent.Strafe(NormDirection.x);
            }
            else
            {
                CharacterAgent.MoveLinear(Mathf.Abs(NormDirection.magnitude));
            }

            // reset if tagged the chased
            if (Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_Freeze || Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_PlayerFrozen)
            {
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
            foreach (FSMStateManager state in tempArray)
            {
                if (state.m_currentState == FSMStateIDs.StateIds.FSM_TagFlee || state.m_currentState == FSMStateIDs.StateIds.FSM_UnFreeze || state.m_currentState == FSMStateIDs.StateIds.FSM_PlayerFlee)
                {
                    targetableList.Add(state);
                }
            }
            if (targetableList.Count > 0)
            {
                int Rand = Random.Range(0, targetableList.Count);
                Target = targetableList[Rand].gameObject;
                SMOwner.Target = Target;
            }
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
