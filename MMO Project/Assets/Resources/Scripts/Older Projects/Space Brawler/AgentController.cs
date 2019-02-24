using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class AgentController : MonoBehaviour {

    protected Agent A;

    // Use this for initialization
    protected virtual void Start () {
        A = GetComponent<Agent>();
	}
}
