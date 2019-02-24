using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgentControllerPacMan : MonoBehaviour {

    private GameObject Target = null;

    public float AngularSpeed = 45f;
    public float FOV = 90f;
    public float ThreatDistance = 10f;
    public float ProximityThreatDistance = 1.5f;

    public GameObject GetTarget()
    {
        return Target;
    }

    public void ScanForTarget()
    {
        // find all Objects - check distance and angle
        // Raycast from our look at ti see what we see

    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
