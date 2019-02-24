using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour {

    // Nav Mesh Agent component
    private NavMeshAgent NavAgent;

    [SerializeField]
    private GameObject Target;

	// Use this for initialization
	void Start () {
        NavAgent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Target != null && Input.GetKeyDown(KeyCode.Space))
        {
            TriggerTarget();
        }
	}

    public void SetTarget(GameObject T)
    {
        Target = T;
    }

    public void TriggerTarget()
    {
        NavAgent.SetDestination(Target.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            NavAgent.SetDestination(transform.position);
        }
    }
}
