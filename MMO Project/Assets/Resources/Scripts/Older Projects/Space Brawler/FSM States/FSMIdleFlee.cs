﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMIdleFlee : FSMState
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
        m_stateId = FSMStateIDs.StateIds.FSM_IdleFlee;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Target = SMOwner.Target;
    }

    public override void OnEnter()
    {
        //Debug.Log("Freeze Enter");
        CharacterRenderer.material = StateMat;
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
