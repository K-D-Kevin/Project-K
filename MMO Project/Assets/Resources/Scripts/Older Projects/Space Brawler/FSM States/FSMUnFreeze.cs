using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnFreeze : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;
    private GameObject Target;

    [SerializeField]
    private float FieldOfView = 90;
    private float FOVHelper = 1 / (180 * 2);

    //[SerializeField]
    //private float Runrange = 5;

    public override void Init()
    {
        //Debug.Log("CHASE Init");
        m_stateId = FSMStateIDs.StateIds.FSM_UnFreeze;
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
            float dotToTarget = Vector3.Dot(SMOwner.transform.right, NormDirection);
            //Debug.Log("Dot To Target: " + dotToTarget);

            // Rotate towards Character
            if (SMOwner.Target)
            {
                if (dotToTarget > FieldOfView * FOVHelper)
                {
                    CharacterAgent.Turn(dotToTarget);
                }
                else if (dotToTarget < -FieldOfView * FOVHelper)
                {
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
            if (Target.GetComponent<FSMStateManager>().m_currentState != FSMStateIDs.StateIds.FSM_Freeze || Target.GetComponent<FSMStateManager>().m_currentState != FSMStateIDs.StateIds.FSM_PlayerFrozen)
            {
                Target = null;
                SMOwner.Target = null;
                // Look for target
                FSMStateManager[] tempArray = FindObjectsOfType<FSMStateManager>();
                List<FSMStateManager> targetableList = new List<FSMStateManager>();
                foreach (FSMStateManager state in tempArray)
                {
                    if (state.m_currentState == FSMStateIDs.StateIds.FSM_Freeze || state.m_currentState == FSMStateIDs.StateIds.FSM_PlayerFrozen)
                    {
                        targetableList.Add(state);
                    }
                }
                if (targetableList.Count > 0)
                {
                    //Debug.Log("Multiple Targets");
                    int Rand = Random.Range(0, targetableList.Count);
                    Target = targetableList[Rand].gameObject;
                    SMOwner.Target = Target;
                }
            }
        }
        else
        {
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

            if (Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_TagChase)
            {
                SMOwner.DesiredState = FSMStateIDs.StateIds.FSM_TagFlee;
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
