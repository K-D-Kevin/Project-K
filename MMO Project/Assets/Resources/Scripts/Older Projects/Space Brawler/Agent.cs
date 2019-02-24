using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour {

    // Agent Components
    [SerializeField]
    private Rigidbody RB = null;
    [HideInInspector]
    public Transform T = null;

    // Speeds
    [SerializeField]
    private float MaxLinearSpeed = 5.0f;
    [SerializeField]
    private float ReverseRatio = 1;
    [SerializeField]
    private float MaxStrafeSpeed = 3.0f;
    private float LinearStrafeRatio;
    [SerializeField]
    private float MaxRiseSpeed = 3.0f;
    [SerializeField]
    private float MaxYawTurnSpeed = 60.0f;
    [SerializeField]
    private float MaxPitchTurnSpeed = 60.0f;
    [SerializeField]
    private float MaxRollTurnSpeed = 60.0f;

    // Forces
    [SerializeField]
    private float MaxLinearForce = 5.0f;
    [SerializeField]
    private float MaxStrafeForce = 3.0f;
    [SerializeField]
    private float MaxRiseForce = 3.0f;
    [SerializeField]
    private float MaxYawTurnForce = 60.0f;
    [SerializeField]
    private float MaxPitchTurnForce = 60.0f;
    [SerializeField]
    private float MaxRollTurnForce = 60.0f;


    // Forces
    [SerializeField]
    private float JumpForce = 10f;
    [SerializeField]
    private bool AirControl = true;
    [SerializeField]
    private float AirControlRatio = 1;
    [SerializeField]
    private Transform Feet;
    [SerializeField]
    private LayerMask GroundLayer;
    private bool Grounded = true;

    public bool UseSpeedAsForce = false;
    public bool UseSpeedWhenUsingForces = true; // use maximum speed cause of forces
    public bool UseLocalCoord = true;

    // Tracking Variables
    private float currentLinearSpeed = 0f;
    private float currentStrafeSpeed = 0f;
    private float currentRiseSpeed = 0f;
    private float currentYawTurnSpeed = 0f;
    private float currentPitchTurnSpeed = 0f;
    private float currentRollTurnSpeed = 0f;
    private Vector3 localVelocity;
    private Vector3 localAngularVelocity;
    private Vector3 UselocalVelocity;
    private Vector3 UselocalAngularVelocity;

    // Use this for initialization
    void Start () {
        if (!RB)
            RB = GetComponent<Rigidbody>();
        T = transform;
        localVelocity = transform.InverseTransformDirection(RB.velocity);
        localAngularVelocity = transform.InverseTransformDirection(RB.angularVelocity);
        LinearStrafeRatio = Mathf.Sqrt(1 + Mathf.Pow(MaxStrafeSpeed, 2) / Mathf.Pow(MaxLinearSpeed, 2));
    }
	
	// Update is called once per frame
	void Update () {
        localVelocity = transform.InverseTransformDirection(RB.velocity);
        localAngularVelocity = transform.InverseTransformDirection(RB.angularVelocity);

        Grounded = Physics.Raycast(Feet.position, -Feet.up, 0.2f, GroundLayer);
        if (!Grounded)
        {
            if (AirControl)
            {
                currentStrafeSpeed = currentStrafeSpeed * AirControlRatio;
                if (currentStrafeSpeed > MaxStrafeSpeed)
                    currentStrafeSpeed = MaxStrafeSpeed;
                else if (currentStrafeSpeed < -MaxStrafeSpeed)
                    currentStrafeSpeed = -MaxStrafeSpeed;

                currentLinearSpeed = currentLinearSpeed * AirControlRatio;
                if (currentLinearSpeed > MaxStrafeSpeed)
                    currentLinearSpeed = MaxStrafeSpeed;
                else if (currentLinearSpeed < -MaxStrafeSpeed)
                    currentLinearSpeed = -MaxStrafeSpeed;
            }
            else
            {
                currentStrafeSpeed = localVelocity.x;
                currentLinearSpeed = localVelocity.z;
            }
        }

        if (currentLinearSpeed < 0)
        {
            currentLinearSpeed *= ReverseRatio;
        }

        if (Mathf.Abs(currentLinearSpeed) > 0 && Mathf.Abs(currentStrafeSpeed) > 0)
        {
            currentStrafeSpeed /= LinearStrafeRatio;
            currentLinearSpeed /= LinearStrafeRatio;
        }

        if (!UseSpeedAsForce)
        {
            if (UseLocalCoord)
            {
                // For now gravity is always on our y
                if (currentLinearSpeed < 0)
                {
                    currentLinearSpeed *= ReverseRatio;
                }

                if (Grounded)
                {
                    RB.velocity = T.TransformDirection(new Vector3(currentStrafeSpeed, RB.velocity.y + currentRiseSpeed, currentLinearSpeed));
                    RB.angularVelocity = T.TransformDirection(new Vector3(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed));
                }
                else if (AirControlRatio == 1 && AirControl)
                {
                    RB.velocity = T.TransformDirection(new Vector3(currentStrafeSpeed, RB.velocity.y + currentRiseSpeed, currentLinearSpeed));
                    RB.angularVelocity = T.TransformDirection(new Vector3(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed));
                }
                else
                {
                    // WORK IN PROGRESS
                    // Note think i need to add the direction of the jump and the direction the player is looking at the time of the jump And / OR add in a acceleration stat
                    if (!AirControl)
                    {
                        currentStrafeSpeed = 0;
                        currentLinearSpeed = 0;
                    }
                    else
                    {
                        currentLinearSpeed *= AirControlRatio;
                        currentStrafeSpeed *= AirControlRatio;
                    }
                    RB.velocity += T.TransformDirection(new Vector3(currentStrafeSpeed, currentRiseSpeed, currentLinearSpeed));
                    if (localVelocity.x > MaxStrafeSpeed && currentStrafeSpeed > 0)
                    {
                        RB.velocity = T.TransformDirection(new Vector3(MaxStrafeSpeed, localVelocity.y, localVelocity.z));
                    }
                    else if (localVelocity.x < -MaxStrafeSpeed && currentStrafeSpeed < 0)
                    {
                        RB.velocity = T.TransformDirection(new Vector3(-MaxStrafeSpeed, localVelocity.y, localVelocity.z));
                    }
                    if (localVelocity.z > MaxLinearSpeed && currentLinearSpeed > 0)
                    {
                        RB.velocity = T.TransformDirection(new Vector3(localVelocity.x, localVelocity.y, MaxLinearSpeed));
                    }
                    if (localVelocity.z < -MaxLinearSpeed && currentLinearSpeed < 0)
                    {
                        RB.velocity = T.TransformDirection(new Vector3(localVelocity.x, localVelocity.y, -MaxLinearSpeed));
                    }
                    RB.angularVelocity = T.TransformDirection(new Vector3(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed));
                }
            }
            else
            {
                RB.velocity = new Vector3(currentStrafeSpeed, RB.velocity.y + currentRiseSpeed, currentLinearSpeed);
                RB.angularVelocity = new Vector3(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed);
            }
        }
        else
        {
            if (UseLocalCoord)
            {
                RB.AddRelativeForce(currentStrafeSpeed, currentRiseSpeed, currentLinearSpeed, ForceMode.Acceleration);
                RB.AddRelativeTorque(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed, ForceMode.Acceleration);
            }
            else
            {
                RB.AddForce(currentStrafeSpeed, currentRiseSpeed, currentLinearSpeed, ForceMode.Acceleration);
                RB.AddTorque(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed, ForceMode.Acceleration);
            }
        }
	}

    public void MoveLinear(float ratio = 1f, bool OverrideMax = false)
    {
        if (!UseSpeedAsForce)
        {
            currentLinearSpeed = ratio * MaxLinearSpeed;
            if (!OverrideMax)
            {
                if (currentLinearSpeed > MaxLinearSpeed)
                {
                    currentLinearSpeed = MaxLinearSpeed;
                }
                else if (currentLinearSpeed < -MaxLinearSpeed)
                {
                    currentLinearSpeed = -MaxLinearSpeed;
                }
            }
        }
        else
        {
            currentLinearSpeed = ratio * MaxLinearForce;
            if (!OverrideMax)
            {
                if (currentLinearSpeed > MaxLinearForce)
                {
                    currentLinearSpeed = MaxLinearForce;
                }
                else if (currentLinearSpeed < -MaxLinearForce)
                {
                    currentLinearSpeed = -MaxLinearForce;
                }

                if (Mathf.Abs(localVelocity.z) > MaxLinearSpeed && UseSpeedWhenUsingForces)
                {
                    currentLinearSpeed = 0;
                }
            }
        }
    }

    public void Strafe(float ratio = 1f, bool OverrideMax = false)
    {
        if (!UseSpeedAsForce)
        {
            currentStrafeSpeed = ratio * MaxStrafeSpeed;
            if (!OverrideMax)
            {
                if (currentStrafeSpeed > MaxStrafeSpeed)
                {
                    currentStrafeSpeed = MaxStrafeSpeed;
                }
                else if (currentStrafeSpeed < -MaxStrafeSpeed)
                {
                    currentStrafeSpeed = -MaxStrafeSpeed;
                }
            }
        }
        else
        {
            currentStrafeSpeed = ratio * MaxLinearForce;
            if (!OverrideMax)
            {
                if (currentStrafeSpeed > MaxStrafeForce)
                {
                    currentStrafeSpeed = MaxStrafeForce;
                }
                else if (currentStrafeSpeed < -MaxStrafeForce)
                {
                    currentStrafeSpeed = -MaxStrafeForce;
                }

                if (Mathf.Abs(localVelocity.x) > MaxStrafeSpeed && UseSpeedWhenUsingForces)
                {
                    currentStrafeSpeed = 0;
                }
            }
        }
    }

    public void Rise(float ratio = 1f, bool OverrideMax = false)
    {
        if (!UseSpeedAsForce)
        {
            currentRiseSpeed = ratio * MaxRiseSpeed;
            if (!OverrideMax)
            {
                if (currentRiseSpeed > MaxRiseSpeed)
                {
                    currentRiseSpeed = MaxRiseSpeed;
                }
                else if (currentRiseSpeed < -MaxRiseSpeed)
                {
                    currentRiseSpeed = -MaxRiseSpeed;
                }
            }
        }
        else
        {
            currentRiseSpeed = ratio * MaxRiseForce;
            if (!OverrideMax)
            {
                if (currentRiseSpeed > MaxRiseForce)
                {
                    currentRiseSpeed = MaxRiseForce;
                }
                else if (currentRiseSpeed < -MaxRiseForce)
                {
                    currentRiseSpeed = -MaxRiseForce;
                }

                if (Mathf.Abs(localVelocity.y) > MaxRiseSpeed && UseSpeedWhenUsingForces)
                {
                    currentRiseSpeed = 0;
                }
            }
        }
    }

    public void StopMovement()
    {
        currentLinearSpeed = 0;
        currentStrafeSpeed = 0;
        currentRiseSpeed = 0f;
        currentYawTurnSpeed = 0;
        currentPitchTurnSpeed = 0;
        currentRollTurnSpeed = 0f;

        if (UseSpeedAsForce)
        {
            RB.velocity = new Vector3(currentStrafeSpeed, currentRiseSpeed, currentLinearSpeed);
            RB.angularVelocity = new Vector3(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed);
        }
    }

    public void StopLinearMovement()
    {
        currentLinearSpeed = 0;
        currentStrafeSpeed = 0;
        currentRiseSpeed = 0f;

        if (UseSpeedAsForce)
        {
            RB.velocity = new Vector3(currentStrafeSpeed, currentRiseSpeed, currentLinearSpeed);
        }
    }

    public void StopTurnMovement()
    {
        currentYawTurnSpeed = 0;
        currentPitchTurnSpeed = 0;
        currentRollTurnSpeed = 0f;

        if (UseSpeedAsForce)
        {
            RB.angularVelocity = new Vector3(currentPitchTurnSpeed, currentYawTurnSpeed, currentRollTurnSpeed);
        }
    }

    public void Jump()
    {
        RB.AddForce(JumpForce * T.up, ForceMode.Impulse);
    }

    public void YawTurn(float ratio = 1f, bool OverrideMax = false, bool turnRight = true)
    {
        if (!UseSpeedAsForce)
        {
            currentYawTurnSpeed = ratio * MaxYawTurnSpeed;
            if (!turnRight)
                currentYawTurnSpeed = -ratio * MaxYawTurnSpeed;
            if (!OverrideMax)
            {
                if (currentYawTurnSpeed > MaxYawTurnSpeed)
                {
                    currentYawTurnSpeed = MaxYawTurnSpeed;
                }
                else if (currentYawTurnSpeed < -MaxYawTurnSpeed)
                {
                    currentYawTurnSpeed = -MaxYawTurnSpeed;
                }
            }
        }
        else
        {
            currentYawTurnSpeed = ratio * MaxYawTurnForce;
            if (!OverrideMax)
            {
                if (currentYawTurnSpeed > MaxYawTurnForce)
                {
                    currentYawTurnSpeed = MaxYawTurnForce;
                }
                else if (currentYawTurnSpeed < -MaxYawTurnForce)
                {
                    currentYawTurnSpeed = -MaxYawTurnForce;
                }

                if (Mathf.Abs(localAngularVelocity.y) > MaxYawTurnSpeed && UseSpeedWhenUsingForces)
                {
                    currentYawTurnSpeed = 0;
                }
            }
        }
        //Debug.Log("Turning: " + currentTurnSpeed);
    }
    // Normal Syntax for better ease of use but is the same as YawTurn
    public void Turn(float ratio = 1f, bool OverrideMax = false, bool turnRight = true)
    {
        if (!UseSpeedAsForce)
        {
            currentYawTurnSpeed = ratio * MaxYawTurnSpeed;
            if (!turnRight)
                currentYawTurnSpeed = -ratio * MaxYawTurnSpeed;
            if (!OverrideMax)
            {
                if (currentYawTurnSpeed > MaxYawTurnSpeed)
                {
                    currentYawTurnSpeed = MaxYawTurnSpeed;
                }
                else if (currentYawTurnSpeed < -MaxYawTurnSpeed)
                {
                    currentYawTurnSpeed = -MaxYawTurnSpeed;
                }
            }
        }
        else
        {
            currentYawTurnSpeed = ratio * MaxYawTurnForce;
            if (!OverrideMax)
            {
                if (currentYawTurnSpeed > MaxYawTurnForce)
                {
                    currentYawTurnSpeed = MaxYawTurnForce;
                }
                else if (currentYawTurnSpeed < -MaxYawTurnForce)
                {
                    currentYawTurnSpeed = -MaxYawTurnForce;
                }

                if (Mathf.Abs(localAngularVelocity.y) > MaxYawTurnSpeed && UseSpeedWhenUsingForces)
                {
                    currentYawTurnSpeed = 0;
                }
            }
        }
        //Debug.Log("Turning: " + currentTurnSpeed);
    }

    public void PitchTurn(float ratio = 1f, bool OverrideMax = false, bool turnUp = true)
    {
        if (!UseSpeedAsForce)
        {
            currentPitchTurnSpeed = ratio * MaxPitchTurnSpeed;
            if (!turnUp)
                currentPitchTurnSpeed = -ratio * MaxPitchTurnSpeed;
            if (!OverrideMax)
            {
                if (currentPitchTurnSpeed > MaxPitchTurnSpeed)
                {
                    currentPitchTurnSpeed = MaxPitchTurnSpeed;
                }
                else if (currentPitchTurnSpeed < -MaxPitchTurnSpeed)
                {
                    currentPitchTurnSpeed = -MaxPitchTurnSpeed;
                }
            }
        }
        else
        {
            currentPitchTurnSpeed = ratio * MaxPitchTurnForce;
            if (!OverrideMax)
            {
                if (currentPitchTurnSpeed > MaxPitchTurnForce)
                {
                    currentPitchTurnSpeed = MaxPitchTurnForce;
                }
                else if (currentPitchTurnSpeed < -MaxPitchTurnForce)
                {
                    currentPitchTurnSpeed = -MaxPitchTurnForce;
                }

                if (Mathf.Abs(localAngularVelocity.x) > MaxPitchTurnSpeed && UseSpeedWhenUsingForces)
                {
                    currentYawTurnSpeed = 0;
                }
            }
        }
        //Debug.Log("Turning: " + currentTurnSpeed);
    }

    public void RollTurn(float ratio = 1f, bool OverrideMax = false, bool turnRight = true)
    {
        if (!UseSpeedAsForce)
        {
            currentRollTurnSpeed = ratio * MaxRollTurnSpeed;
            if (!turnRight)
                currentRollTurnSpeed = -ratio * MaxRollTurnSpeed;
            if (!OverrideMax)
            {
                if (currentRollTurnSpeed > MaxRollTurnSpeed)
                {
                    currentRollTurnSpeed = MaxRollTurnSpeed;
                }
                else if (currentRollTurnSpeed < -MaxRollTurnSpeed)
                {
                    currentRollTurnSpeed = -MaxRollTurnSpeed;
                }
            }
        }
        else
        {
            currentRollTurnSpeed = ratio * MaxRollTurnForce;
            if (!OverrideMax)
            {
                if (currentRollTurnSpeed > MaxRollTurnForce)
                {
                    currentRollTurnSpeed = MaxRollTurnForce;
                }
                else if (currentRollTurnSpeed < -MaxRollTurnForce)
                {
                    currentRollTurnSpeed = -MaxRollTurnForce;
                }

                if (Mathf.Abs(localAngularVelocity.z) > MaxRollTurnSpeed && UseSpeedWhenUsingForces)
                {
                    currentRollTurnSpeed = 0;
                }
            }
        }
        //Debug.Log("Turning: " + currentTurnSpeed);
    }

    public void ResetMaxLinearSpeed(float s)
    {
        MaxLinearSpeed = s;
        LinearStrafeRatio = Mathf.Sqrt(1 + Mathf.Pow(MaxStrafeSpeed, 2) / Mathf.Pow(MaxLinearSpeed, 2));
    }

    public float GetLinearSpeed()
    {
        return MaxLinearSpeed;
    }

    public void ResetMaxStrafeSpeed(float s)
    {
        MaxStrafeSpeed = s;
        LinearStrafeRatio = Mathf.Sqrt(1 + Mathf.Pow(MaxStrafeSpeed, 2) / Mathf.Pow(MaxLinearSpeed, 2));
    }

    public float GetStrafeSpeed()
    {
        return MaxStrafeSpeed;
    }

    public void ResetJumpForce(float s)
    {
        JumpForce = s;
    }

    public float GetJumpForce()
    {
        return JumpForce;
    }

    public void ResetMaxTurnSpeed(float s)
    {
        MaxYawTurnSpeed = s;
    }

    public void ResetMaxTurnForce(float f)
    {
        MaxYawTurnForce = f;
    }
}
