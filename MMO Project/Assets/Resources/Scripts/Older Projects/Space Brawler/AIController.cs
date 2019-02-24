using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FSMStateManager))]
public class AIController : MonoBehaviour {

    // Am i agressive or neutral AI
    [SerializeField]
    private bool IsAggressive = false;

    // Targets
    [SerializeField]
    private GameObject Player;
    private List<GameObject> TargetableObjects = new List<GameObject>();
    public int TargetCount = 0;
    private List<GameObject> TargetableObjectsTemp = new List<GameObject>();
    private GameObject Target;

    // Search Parameters
    [SerializeField]
    private SphereCollider SC;
    [SerializeField]
    private float SearchRange = 50;
    [SerializeField]
    private float FireRange = 20;
    [SerializeField]
    private float FollowRange = 25;
    private Vector3 Direction;
    public float Distance = 0;

    // Fire Parameters
    [SerializeField]
    private float RateOfFire = 6;
    private float ROFTimer = 0;
    [SerializeField]
    private float LaserDamage = 5;
    [SerializeField]
    private LaserProjectile LaserPrefab;
    [SerializeField]
    private Transform FirePosition1;
    [SerializeField]
    private Transform FirePosition2;
    private Transform TargetTransform;

    // ShipComponents
    [SerializeField]
    private Health Hp;
    private FSMStateManager SM;
    private float HpLeft;

    // Use this for initialization
    void Start () {
        SM = GetComponent<FSMStateManager>();
        if (!Player)
            Player = FindObjectOfType<ShipController>().gameObject;
        SC.isTrigger = true;
        SC.radius = SearchRange;
        if (!Hp)
            Hp = GetComponent<Health>();
        Target = SM.Target;
        if (SM.Target != null)
            TargetTransform = Target.transform;
	}
	
	// Update is called once per frame
	void Update () {

        HpLeft = Hp.CheckHealth();
        if (HpLeft <= 0)
        {
            BeforeDestroy();
            Destroy(gameObject);
        }

        TargetCount = TargetableObjects.Count;

        if (Target == null)
        {
            SM.DesiredState = FSMStateIDs.StateIds.FSM_Idle;
            // If the list isn't empty
            if (TargetableObjects.Count > 0)
            {
                // If its an aggressive AI, Find player
                if (IsAggressive && TargetableObjects.Contains(Player))
                {
                    Target = Player;
                }
                else if (IsAggressive)
                {
                    foreach (GameObject G in TargetableObjects)
                    {
                        if (TargetableObjects.Count <= 0)
                        {
                            break;
                        }
                        if (G != null && G.tag == "NeutralAI")
                        {
                            Target = G;
                            break;
                        }
                    }
                }
                // If no player or not aggressive Find another object
                if (!IsAggressive)
                {
                    foreach(GameObject G in TargetableObjects)
                    {
                        if (TargetableObjects.Count <= 0)
                        {
                            break;
                        }
                        if (G != null && (G.tag == "Astroid" || G.tag == "Comet"))
                        {
                            Target = G;
                            break;
                        }
                    }
                    string TargetTag = "";
                    if (Target != null)
                        TargetTag = Target.tag;
                    if (TargetTag == "Player" || TargetTag == "AgressiveAi")
                    {
                        Target = null;
                    }
                }

            }

            SM.Target = Target;
            if (Target == null)
            {
                SM.DesiredState = FSMStateIDs.StateIds.FSM_Idle;
            }
            else
            {
                TargetTransform = Target.transform;
            }
        }
        else
        {
            if (TargetTransform == null)
            {
                Target = null;
                SM.DesiredState = FSMStateIDs.StateIds.FSM_Idle;
            }
            else
            {
                Direction = TargetTransform.position - transform.position;
                Distance = Direction.magnitude;
                if (Distance >= FollowRange)
                {
                    SM.DesiredState = FSMStateIDs.StateIds.FSM_Chase;
                }
                else if (Distance <= FireRange)
                {
                    SM.DesiredState = FSMStateIDs.StateIds.FSM_IdleChase;
                }

                if (Distance <= FollowRange)
                {
                    ROFTimer -= Time.deltaTime;
                    if (ROFTimer <= 0)
                    {
                        LaserProjectile temp = Instantiate(LaserPrefab, FirePosition1);
                        temp.SetWhoFiredMe(Hp);
                        temp.transform.parent = null;
                        temp.SetDamage(LaserDamage);

                        LaserProjectile temp2 = Instantiate(LaserPrefab, FirePosition2);
                        temp2.SetWhoFiredMe(Hp);
                        temp2.transform.parent = null;
                        temp2.SetDamage(LaserDamage);

                        ROFTimer = 1 / RateOfFire;
                    }
                }
            }
        }

        if (Hp.WhoHitMe() != null && Hp.WhoHitMe().gameObject != gameObject)
        {
            Target = Hp.WhoHitMe().gameObject;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!TargetableObjects.Contains(other.gameObject))
        {
            if (other.tag != "Laser")
            {
                TargetableObjects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Laser")
        {
            if (TargetableObjects.Contains(other.gameObject))
            {
                TargetableObjects.Remove(other.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!TargetableObjects.Contains(collision.gameObject.gameObject))
        {
            if (collision.gameObject.tag != "Laser")
            {
                TargetableObjects.Add(collision.gameObject.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Laser")
        {
            if (TargetableObjects.Contains(collision.gameObject.gameObject))
            {
                TargetableObjects.Remove(collision.gameObject.gameObject);
            }
        }
    }

    public void RemoveTargetFromList(GameObject g)
    {
        TargetableObjects.Remove(g);
    }

    public void SetTarget(GameObject Tgt)
    {
        Target = Tgt;
    }

    public void BeforeDestroy()
    {
        foreach (GameObject G in TargetableObjects)
        {
            AIController AIOther = null;
            if (G != null)
            {
                AIOther = G.GetComponent<AIController>();
                if (AIOther != null)
                {
                    AIOther.RemoveTargetFromList(gameObject);
                }
            }
        }
    }
}
