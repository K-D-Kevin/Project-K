using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandom : MonoBehaviour {

    [SerializeField]
    private float MedianSpeed = 10;
    [SerializeField]
    private float SpeedDeviation = 3;

    [SerializeField]
    private Rigidbody RB;

    private Vector3 RandomDirection;
    private float Speed;
    private Vector3 Velocity;

    // Use this for initialization
    void Start () {
        Speed = Random.Range(MedianSpeed - SpeedDeviation, MedianSpeed + SpeedDeviation);
        RandomDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        Velocity = RandomDirection * Speed;
	}
	
	// Update is called once per frame
	void Update () {
        RB.velocity = Velocity;
	}
}
