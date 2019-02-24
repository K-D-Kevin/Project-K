using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour {

    [SerializeField]
    private Transform FollowedObject;
    private Transform T;

	// Use this for initialization
	void Start () {
        T = transform;
	}
	
	// Update is called once per frame
	void Update () {
        T.rotation = FollowedObject.rotation;
	}
}
