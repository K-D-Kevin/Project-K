using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : FSMState
{

    private Agent CharacterAgent;
    [SerializeField]
    private Material StateMat;
    private MeshRenderer CharacterRenderer;

    private Camera Cam;
    private float MouseInputX = 0;
    private float MouseInputY = 0;
    [SerializeField]
    private float MouseSensitivity = 180 / Mathf.PI;
    private Vector3 RotateCharacter;
    private Vector3 RotateCam;

    [SerializeField]
    private LayerMask GroundLayer;
    private Transform Feet;

    public override void Init()
    {
        //Debug.Log("Init");
        m_stateId = FSMStateIDs.StateIds.FSM_BasePlayer;
        CharacterAgent = SMOwner.GetComponent<Agent>();
        CharacterRenderer = SMOwner.GetComponent<MeshRenderer>();
        Cam = SMOwner.Cam;
        Feet = SMOwner.GetFeet();
        Physics.gravity = new Vector3(0, -9.81f * 1, 0);
    }

    public override void OnEnter()
    {
        //Debug.Log("Enter");
        CharacterRenderer.material = StateMat;
        //CharacterAgent.StopMovement();
        Cam.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnExit()
    {
        //Debug.Log("Exit");
        //CharacterAgent.Jump();
        //CharacterAgent.StopMovement();
        Cam.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        CharacterAgent.Strafe(Input.GetAxis("Horizontal"));
        CharacterAgent.MoveLinear(Input.GetAxis("Vertical"));
        if (Input.GetAxis("Jump") > 0.1f && Physics.Raycast(Feet.position, -Feet.up, 0.2f, GroundLayer))
            CharacterAgent.Jump();

        MouseInputX = Input.GetAxisRaw("Mouse X") * MouseSensitivity;
        MouseInputY = Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
        RotateCam += new Vector3(-MouseInputY, 0, 0);
        RotateCharacter += new Vector3(0, MouseInputX, 0);
        RotateCam = new Vector3(Mathf.Clamp(RotateCam.x, -90, 90), 0, 0);

        Cam.transform.localRotation = Quaternion.Euler(RotateCam);
        SMOwner.transform.localRotation = Quaternion.Euler(RotateCharacter);
    }

    public override void OnShutdown()
    {
        //Debug.Log("Shutdown");
        //CharacterAgent.StopMovement();
        //CharacterAgent.Jump();
    }

    public override void GetStateManager(FSMStateManager sm)
    {
        SMOwner = sm;
    }

    public override void FSMOnCollisionEnter(Collision c)
    {
    }
}
