using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    /*
    // --------SHIP CONTROL SCHEME-------------//
    // Left / Right joysticks && Control Panel
    // -----------Left Joystick--------------
    // LR Motion causes the ship to rotate
    // FB Motion cuases the ship to Rotate forwards or backwards
    // Primary Thumbstick down causes the ship to shoot the left gun
    // Primary Thumbstick causes the ship to aim the left gun
    // Secondary Button cause the the ship to roll left
    // Primary Button causes the ship to rise upwards
    // -----------Right Joystick-------------
    // LR Motion causes the ship to move (STRAFE) left or right
    // FB Motion cuases the ship to move forwards or backwards
    // Primary Thumbstick down causes the ship to shoot the right gun
    // Primary Thumbstick causes the ship to aim the right gun
    // Secondary Button cause the the ship to roll right
    // Primary Button causes the ship to decline downwards
    // ----------- Control Panel-------------
    // The control panel has a list of button the player can press that upgrade stats
    // each button has a corresponding light indicating the number of times the player has upgraded stats
    // if the player has enough resourses to upgrade and the max upgrades hasn't been achieved, another light will light up
    // ----------- Stats -------------------
    // Speed - The speed buff given to the player
    // Rate of Fire - the attack speed buff
    // Projectile Damage - the damage the player does with his weapons
    // Collision Damage - the damage the player does when the player collides with another object
    // Resource Gather Speed - the amount of resource gathered
    // Repair Speed -  the speed the ship gets auto repaired
    // Max Health - the maximum amount of ship health


    // Controls
    [SerializeField]
    private bool InvertY = false;

    // Movement Speed
    [SerializeField]
    private float ForwardSpeed = 10;
    [HideInInspector]
    public float ForwardBackS;

    [SerializeField]
    private float SideSpeed = 8.5f;
    [HideInInspector]
    public float SideS;

    [SerializeField]
    private float UpDownSpeed = 8;
    [HideInInspector]
    public float UpDownS;

    [SerializeField]
    private float ReverseSpeed = 7;

    [SerializeField]
    private float RotationSpeed = 50;
    [HideInInspector]
    public float RotationFBS;
    [HideInInspector]
    public float RotationLRS;

    [SerializeField]
    private float RollSpeed = 50;
    [HideInInspector]
    public float RollS;

    // Attack
    // Guns
    [SerializeField]
    private AutoAim LeftGun;
    [SerializeField]
    private AutoAim RightGun;

    [SerializeField]
    private float RateOfFire = 1; // Rate of fire per second
    private float ROF;
    private float LeftTimer = 0;
    private float RightTimer = 0;

    [SerializeField]
    private float ProjectileDamage = 1; // Damage done by shooting
    private float P_DMG;

    [SerializeField]
    private float CollisionDamage = 1; // Damage done by colliding
    private float C_DMG;

    [SerializeField]
    private Transform LeftGunTransform;
    [SerializeField]
    private Transform RightGunTransform;
    [SerializeField]
    private LaserProjectile ProjectilePrefab;

    // Joysticks
    [SerializeField]
    private ShipJoystick ShipLeftJoystick;
    [SerializeField]
    private ShipJoystick ShipRightJoystick;

    // Joestick Dead Zone
    [SerializeField]
    private float JoyDeadZone = 5f;
    [SerializeField]
    private float MaxmimumAngle = 45f;
    // Modifiers
    // Modifiers change based on the amount the joysticks are rotated based on relevent direction
    private float FBSpeedModifier = 0;
    private float SSpeedModifier = 0;
    private float UDSpeedModifier = 0;
    private float RotXSpeedModifier = 0;
    private float RotZSpeedModifier = 0;
    private float RollSpeedModifier = 0;

    // Multiplyers
    // Multiplyers change based on the amount of times the stat has been upgraded from in game play
    private float SpeedMultiplyer = 1;
    private float P_DMG_Multiplyer = 1;
    private float C_DMG_Multiplyer = 1;

    // Ship Components
    private Transform T;
    private Rigidbody RB;
    private Health Hp;
    //[HideInInspector]
    public Vector3 MoveVector = Vector3.zero;
    //[HideInInspector]
    public Vector3 RotateVector = Vector3.zero;

    // Use this for initialization
    void Start () {
        T = transform;
        RB = GetComponent<Rigidbody>();
        ROF = RateOfFire;
        P_DMG = ProjectileDamage;
        Hp = GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {

        // ----------Calculate Movement-------------- //

        // -------------Left Joystick---------------- //
        // Rotate Forward and Back
        float XAngle = ShipLeftJoystick.DifferenceEulerAngles.x < JoyDeadZone ? 0.0f
            : ShipLeftJoystick.DifferenceEulerAngles.x > 360f - JoyDeadZone ? 0.0f
            : (ShipLeftJoystick.DifferenceEulerAngles.x < (360f - MaxmimumAngle)) && ShipLeftJoystick.DifferenceEulerAngles.x > 180f ? -MaxmimumAngle + JoyDeadZone
            : ShipLeftJoystick.DifferenceEulerAngles.x > MaxmimumAngle && ShipLeftJoystick.DifferenceEulerAngles.x < 180f ? MaxmimumAngle - JoyDeadZone
            : ShipLeftJoystick.DifferenceEulerAngles.x > 180f ? ShipLeftJoystick.DifferenceEulerAngles.x - 360f + JoyDeadZone
            : ShipLeftJoystick.DifferenceEulerAngles.x - JoyDeadZone;
        RotXSpeedModifier = XAngle / (MaxmimumAngle - JoyDeadZone);
        //Debug.Log("Left Joystick X(Angle(*) / Mod): (" + XAngle + "(" + ShipLeftJoystick.transform.eulerAngles.x + "), / " + RotXSpeedModifier + ")");

        RotationFBS = RotXSpeedModifier * RotationSpeed * SpeedMultiplyer * Time.deltaTime;

        // Rotate Left and Right
        float ZAngle = ShipLeftJoystick.DifferenceEulerAngles.z < JoyDeadZone ? 0.0f
            : ShipLeftJoystick.DifferenceEulerAngles.z > 360f - JoyDeadZone ? 0.0f
            : (ShipLeftJoystick.DifferenceEulerAngles.z < (360f - MaxmimumAngle)) && ShipLeftJoystick.DifferenceEulerAngles.z > 180f ? -MaxmimumAngle + JoyDeadZone
            : ShipLeftJoystick.DifferenceEulerAngles.z > MaxmimumAngle && ShipLeftJoystick.DifferenceEulerAngles.z < 180f ? MaxmimumAngle - JoyDeadZone
            : ShipLeftJoystick.DifferenceEulerAngles.z > 180f ? ShipLeftJoystick.DifferenceEulerAngles.z - 360f + JoyDeadZone
            : ShipLeftJoystick.DifferenceEulerAngles.z - JoyDeadZone;
        RotZSpeedModifier = -ZAngle / (MaxmimumAngle - JoyDeadZone);
        //Debug.Log("Left Joystick Z(Angle(*) / Mod): (" + ZAngle + "(" + ShipLeftJoystick.transform.eulerAngles.z + "), / " + RotZSpeedModifier + ")");

        RotationLRS = RotZSpeedModifier * RotationSpeed * SpeedMultiplyer * Time.deltaTime;


        // -------------Right Joystick---------------- //
        // Move Forward and Back
        XAngle = ShipRightJoystick.DifferenceEulerAngles.x < JoyDeadZone ? 0.0f
            : ShipRightJoystick.DifferenceEulerAngles.x > 360f - JoyDeadZone ? 0.0f
            : (ShipRightJoystick.DifferenceEulerAngles.x < (360f - MaxmimumAngle)) && ShipRightJoystick.DifferenceEulerAngles.x > 180f ? -MaxmimumAngle + JoyDeadZone
            : ShipRightJoystick.DifferenceEulerAngles.x > MaxmimumAngle && ShipRightJoystick.DifferenceEulerAngles.x < 180f  ? MaxmimumAngle - JoyDeadZone
            : ShipRightJoystick.DifferenceEulerAngles.x > 180f ? ShipRightJoystick.DifferenceEulerAngles.x - 360f + JoyDeadZone
            : ShipRightJoystick.DifferenceEulerAngles.x - JoyDeadZone;
        FBSpeedModifier = XAngle / (MaxmimumAngle - JoyDeadZone);
        //Debug.Log("Right Joystick X(Angle(*) / Mod): (" + XAngle + "(" + ShipRightJoystick.transform.eulerAngles.x + "), / " + FBSpeedModifier + ")");

        if (FBSpeedModifier > 0)
            ForwardBackS = FBSpeedModifier * SpeedMultiplyer * ForwardSpeed * Time.deltaTime;
        else
            ForwardBackS = FBSpeedModifier * SpeedMultiplyer * ReverseSpeed * Time.deltaTime;


        // Move Left and Right
        ZAngle = ShipRightJoystick.DifferenceEulerAngles.z < JoyDeadZone ? 0.0f
            : ShipRightJoystick.DifferenceEulerAngles.z > 360f - JoyDeadZone ? 0.0f
            : (ShipRightJoystick.DifferenceEulerAngles.z < (360f - MaxmimumAngle)) && ShipRightJoystick.DifferenceEulerAngles.z > 180f ? -MaxmimumAngle + JoyDeadZone
            : ShipRightJoystick.DifferenceEulerAngles.z > MaxmimumAngle && ShipRightJoystick.DifferenceEulerAngles.z < 180f ? MaxmimumAngle - JoyDeadZone
            : ShipRightJoystick.DifferenceEulerAngles.z > 180f ? ShipRightJoystick.DifferenceEulerAngles.z - 360f + JoyDeadZone
            : ShipRightJoystick.DifferenceEulerAngles.z - JoyDeadZone;
        SSpeedModifier = -ZAngle / (MaxmimumAngle - JoyDeadZone);
        //Debug.Log("Right Joystick Z(Angle(*) / Mod): (" + ZAngle + "(" + ShipRightJoystick.transform.eulerAngles.z + "), / " + SSpeedModifier + ")");


        SideS = SSpeedModifier * SpeedMultiplyer * SideSpeed * Time.deltaTime;

        // Rise and Lower Ship
        if (ShipLeftJoystick.Button1Pressed && !ShipRightJoystick.Button1Pressed)
            UDSpeedModifier = -1;
        else if (!ShipLeftJoystick.Button1Pressed && ShipRightJoystick.Button1Pressed)
            UDSpeedModifier = 1;
        else
            UDSpeedModifier = 0;

        UpDownS = UDSpeedModifier * SpeedMultiplyer * UpDownSpeed * Time.deltaTime;

        // Roll Ship Left and Right
        if (ShipLeftJoystick.Button2Pressed && !ShipRightJoystick.Button2Pressed)
            RollSpeedModifier = 1;
        else if (!ShipLeftJoystick.Button2Pressed && ShipRightJoystick.Button2Pressed)
            RollSpeedModifier = -1;
        else
            RollSpeedModifier = 0;

        RollS = RollSpeedModifier * SpeedMultiplyer * RollSpeed * Time.deltaTime;

        if (ShipLeftJoystick.Shoot)
        {
            LeftTimer -= Time.deltaTime;
            if (LeftTimer <= 0 && LeftGun.LineOfSight)
            {
                LaserProjectile temp = Instantiate(ProjectilePrefab, LeftGunTransform);
                temp.SetWhoFiredMe(Hp);
                temp.transform.parent = null;
                temp.SetDamage(P_DMG);
                LeftTimer = 1 / ROF;
            }
        }
        if (ShipRightJoystick.Shoot)
        {
            RightTimer -= Time.deltaTime;
            if (RightTimer <= 0 && RightGun.LineOfSight)
            {
                LaserProjectile temp = Instantiate(ProjectilePrefab, RightGunTransform);
                temp.SetWhoFiredMe(Hp);
                temp.transform.parent = null;
                temp.SetDamage(P_DMG);
                RightTimer =  1 / ROF;
            }
        }

    }

    private void FixedUpdate()
    {
        // -------------Move Ship---------------- //
        // Move

        if (ShipLeftJoystick.Trigger2Pressed)
        {
            //if (InvertY)
            //{
            //    RotationFBS = -RotationFBS;
            //}
            RotateVector = ShipLeftJoystick.Trigger1Axis >= 0.5 ? new Vector3(RotationFBS, RotationLRS, RollS) * (ShipLeftJoystick.Trigger1Axis - 0.5f) / 0.5f
                : Vector3.zero;
            RB.angularVelocity = transform.TransformDirection(RotateVector);
             
            //Debug.Log("Translate (X / Y / Z): (" + SideS + " / " + UpDownS + " / " + ForwardBackS + ")");
            // Transform Based
            ////T.Translate(SideS, UpDownS, ForwardBackS);
            // Force Based
            //RB.AddRelativeTorque(RotateVector, ForceMode.Force);
        }
        if (ShipRightJoystick.Trigger2Pressed)
        {
            //if (InvertY)
            //{
            //    ForwardBackS = -ForwardBackS;
            //}
            MoveVector = ShipRightJoystick.Trigger1Axis >= 0.5 ? new Vector3(SideS, UpDownS, ForwardBackS) * (ShipRightJoystick.Trigger1Axis - 0.5f) / 0.5f
                : Vector3.zero;
            RB.velocity = transform.TransformDirection(MoveVector);

            //Debug.Log("Torque: (" + RotationFBS + ", " + RotationLRS + ", " + RollS + ")");        
            // Rotate
            // Transform Based
            ///T.Rotate(RotationFBS, RotationLRS, RollS);
            // Force Based
            //RB.AddRelativeForce(MoveVector, ForceMode.Force);
            //Debug.Log("Forces: (" + SideS + ", " + UpDownS + ", " + ForwardBackS + ")");
        }
    }
    */
}
