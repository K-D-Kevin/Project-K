using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgentController : AgentController {

    private bool isActive = false;

    [SerializeField]
    private GameObject Target;

    // Distaces
    [SerializeField]
    private float MaxTargetDistance = 15.0f;
    [SerializeField]
    private float MinTargetDistance = 1.0f;
    private Vector3 DistanceToTarget;

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Space))
            isActive = true;
        else
            isActive = false;

        if (isActive)
        {
            DistanceToTarget = Target.transform.position - A.T.position;
            if (Target != null)
            {
                // Face Target
                A.T.LookAt(Target.transform);

                float speedRatio = DistanceToTarget.magnitude < MinTargetDistance ? 0.0f : DistanceToTarget.magnitude / MaxTargetDistance;
                speedRatio = speedRatio > 1.0f ? 1.0f : speedRatio;
                A.MoveLinear(speedRatio);
            }
            else
            {
                isActive = false;
            }

        }
        else
        {
            A.StopMovement();
        }
    }
}
