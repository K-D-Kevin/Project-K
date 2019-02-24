using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FSMStateManager))]
public class SumoController : MonoBehaviour {

    private FSMStateManager SM;
    private FSMStateManager Target;
    private NavMeshAgent NA;

    private Vector3 Direction;
    private Vector3 Distance;

    private float ChargeDistance = 1;

    private Charge Dash;
    private float DashCooldown = 0;
    private float DashCooldownTimer = 0;
    private bool Dashed = false;

    private bool IsVertical = true;
    private bool IsHorizontal = true;

    [HideInInspector]
    public bool LostCollision = false;

    private RaycastHit RayU;
    private RaycastHit RayD;
    private RaycastHit RayR;
    private RaycastHit RayL;
    private float ShortestWallDistance = 30;
    private bool RightWallShortest = false;
    private bool TopWallShortest = false;

    private float MaxFleeTime = 3;
    private float FleeTimer = 0;

    [SerializeField]
    private LayerMask WallLayer;

    // Use this for initialization
    void Start () {
        SM = GetComponent<FSMStateManager>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<FSMStateManager>();
        SM.Target = Target.gameObject;
        NA = GetComponent<NavMeshAgent>();

        Dash = GetComponent<Charge>();
        DashCooldown = Dash.GetCooldownTime();
        //Debug.Log("Cooldown Dash: " + DashCooldown);
	}
	
	// Update is called once per frame
	void Update () {

        // Test the AI states
        if (SM.m_currentState != FSMStateIDs.StateIds.FSM_NavSumoRef)
        {

            // Find distances for the player and the walls
            Distance = (Target.transform.position - transform.position);
            Direction = Distance.normalized;
            //Debug.Log("Target Distance: " + Distance.magnitude);

            IsVertical = Mathf.Abs(Direction.z) >= 0.75f;
            IsHorizontal = Mathf.Abs(Direction.x) >= 0.75f;
            //Debug.Log(Direction.z);

            Physics.Raycast(transform.position, Vector3.forward, out RayU, 20, WallLayer);
            Physics.Raycast(transform.position, Vector3.back, out RayD, 20, WallLayer);
            Physics.Raycast(transform.position, Vector3.right, out RayR, 20, WallLayer);
            Physics.Raycast(transform.position, Vector3.left, out RayL, 20, WallLayer);

            // Draw Rays
            //Debug.DrawRay(transform.position, Vector3.forward, Color.black);
            //Debug.DrawRay(transform.position, Vector3.back, Color.black);
            //Debug.DrawRay(transform.position, Vector3.right, Color.black);
            //Debug.DrawLine(transform.position, Vector3.left, Color.black);

            ShortestWallDistance = RayU.distance;
            if (RayD.distance < ShortestWallDistance)
            {
                ShortestWallDistance = RayD.distance;
            }
            if (RayR.distance < ShortestWallDistance)
            {
                ShortestWallDistance = RayR.distance;
            }
            if (RayL.distance < ShortestWallDistance)
            {
                ShortestWallDistance = RayL.distance;
            }
            TopWallShortest = RayU.distance <= RayD.distance;
            RightWallShortest = RayR.distance <= RayL.distance;

            if (LostCollision && ShortestWallDistance <= 2)
            {
                SM.DesiredState = FSMStateIDs.StateIds.FSM_NavSumoFlee;
                LostCollision = false;
            }

            // If the AI is fleeing
            if (SM.m_currentState == FSMStateIDs.StateIds.FSM_NavSumoFlee)
            {
                FleeTimer += Time.deltaTime;
                if (FleeTimer >= MaxFleeTime)
                {
                    SM.DesiredState = FSMStateIDs.StateIds.FSM_NavSumoChase;
                    FleeTimer = 0;
                }
                if (Distance.magnitude <= 3f)
                {
                    if (Dashed == false)
                    {
                        if (TopWallShortest)
                        {
                            if (RightWallShortest)
                            {
                                Dash.DashDown(0.5f);
                                Dash.DashLeft(0.5f);
                            }
                            else
                            {
                                Dash.DashDown(0.5f);
                                Dash.DashRight(0.5f);
                            }
                            Dashed = true;
                        }
                        else if (!TopWallShortest)
                        {
                            if (RightWallShortest)
                            {
                                Dash.DashUp(0.5f);
                                Dash.DashLeft(0.5f);
                            }
                            else
                            {
                                Dash.DashUp(0.5f);
                                Dash.DashRight(0.5f);
                            }
                            Dashed = true;
                        }
                    }
                    else
                    {
                        DashCooldownTimer += Time.deltaTime;
                        if (DashCooldownTimer >= DashCooldown)
                        {
                            Dashed = false;
                            DashCooldownTimer = 0;
                        }
                    }
                }

                if (NA.remainingDistance <= 0.2f)
                {
                    SM.DesiredState = FSMStateIDs.StateIds.FSM_NavSumoChase;
                }
            }

            // If the AI is attacking
            if (SM.m_currentState == FSMStateIDs.StateIds.FSM_NavSumoChase)
            {
                if (Distance.magnitude <= 3)
                {
                    if (Dashed == false)
                    {
                        if (IsVertical)
                        {
                            if (Direction.z < 0)
                            {
                                Dash.DashDown(1);
                                Dashed = true;
                            }
                            else if (Direction.x > 0)
                            {
                                Dash.DashUp(1);
                                Dashed = true;
                            }
                        }
                        else if (IsHorizontal)
                        {
                            if (Direction.x < 0)
                            {
                                Dash.DashLeft(1);
                                Dashed = true;
                            }
                            else if (Direction.x > 0)
                            {
                                Dash.DashRight(1);
                                Dashed = true;
                            }
                        }
                        else
                        {
                            if (Direction.x < 0)
                            {
                                Dash.DashLeft(0.5f);
                                Dashed = true;
                            }
                            else if (Direction.x > 0)
                            {
                                Dash.DashRight(0.5f);
                                Dashed = true;
                            }
                            if (Direction.z < 0)
                            {
                                Dash.DashDown(0.5f);
                                Dashed = true;
                            }
                            else if (Direction.x > 0)
                            {
                                Dash.DashUp(0.5f);
                                Dashed = true;
                            }
                        }
                    }
                    else
                    {
                        DashCooldownTimer += Time.deltaTime;
                        if (DashCooldownTimer >= DashCooldown)
                        {
                            Dashed = false;
                            DashCooldownTimer = 0;
                        }
                    }
                }
            }

            // To Test The Ai
            ///*
            if (Input.GetKeyDown(KeyCode.I))
            {
                SM.DesiredState = FSMStateIDs.StateIds.FSM_SumoIdle;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                SM.DesiredState = FSMStateIDs.StateIds.FSM_NavSumoFlee;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                SM.DesiredState = FSMStateIDs.StateIds.FSM_NavSumoChase;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Dash.DashUp(1);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Dash.DashDown(1);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Dash.DashRight(1);
            }
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                Dash.DashLeft(1);
            }
            //*/
        }
    }
}
