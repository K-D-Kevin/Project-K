using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charge : MonoBehaviour {

    private Rigidbody RB;

    [SerializeField]
    private float ChargeForce = 10;
    [SerializeField]
    private float CollisionForce = 50;

    [SerializeField]
    private float LossKnockBackPercent = 70;
    [SerializeField]
    private float TieKnockBackPercent = 50;
    [SerializeField]
    private float WinKnockbackPercent = 30;

    [SerializeField]
    private float InputBuffer = 0.05f;
    private float InputTimer = 0f;
    private bool StartTimer = false;

    [SerializeField]
    private float ChargeCooldown = 3f;
    private float CooldownTimer = 0;
    private bool StartCooldown = false;

    [SerializeField]
    private bool IsPlayer = true;

    private bool ChargeUp = false;
    private bool ChargeDown = false;
    private bool ChargeRight = false;
    private bool ChargeLeft = false;

    private float KnockBackTime = .2f;
    private float KnockBackTimer = 0f;
    private bool StartKnockBack = false;

    // Use this for initialization
    void Start () {
        RB = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -100, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (IsPlayer)
        {
            if (!StartCooldown)
            {
                if (StartTimer) // once buffer starts - find any additional inputs during buffer
                {
                    InputTimer += Time.deltaTime;

                    if (Input.GetKeyDown(KeyCode.Q) && !ChargeRight)
                    {
                        ChargeLeft = true;
                    }
                    if (Input.GetKeyDown(KeyCode.E) && !ChargeLeft)
                    {
                        ChargeRight = true;
                    }
                    if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift)) && !ChargeDown)
                    {
                        ChargeUp = true;
                    }
                    if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftControl)) && !ChargeUp)
                    {
                        ChargeDown = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) // Start a buffer to get inputs
                {
                    if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKeyDown(KeyCode.E))
                    {
                        ChargeLeft = true;
                        StartTimer = true;
                    }
                    if (Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Q))
                    {
                        ChargeRight = true;
                        StartTimer = true;
                    }
                    if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift)) && !(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftControl)))
                    {
                        ChargeUp = true;
                        StartTimer = true;
                    }
                    if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftControl)) && !(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift)))
                    {
                        ChargeDown = true;
                        StartTimer = true;
                    }
                }

                if (InputTimer >= InputBuffer) // output after buffer
                {
                    StartTimer = false;
                    InputTimer = 0;

                    if (ChargeUp)
                    {
                        if (ChargeLeft)
                        {
                            DashLeft(0.5f);
                            DashUp(0.5f);
                        }
                        else if (ChargeRight)
                        {
                            DashRight(0.5f);
                            DashUp(0.5f);
                        }
                        else
                        {
                            DashUp(1);
                        }
                        StartCooldown = true;
                    }
                    else if (ChargeDown)
                    {
                        if (ChargeLeft)
                        {
                            DashLeft(0.5f);
                            DashDown(0.5f);
                        }
                        else if (ChargeRight)
                        {
                            DashRight(0.5f);
                            DashDown(0.5f);
                        }
                        else
                        {
                            DashDown(1);
                        }
                        StartCooldown = true;
                    }
                    else if (ChargeRight)
                    {
                        DashRight(1);
                        StartCooldown = true;
                    }
                    else if (ChargeLeft)
                    {
                        DashLeft(1);
                        StartCooldown = true;
                    }
                    ResetBools();
                }
            }
            else
            {
                CooldownTimer += Time.deltaTime;
                if (CooldownTimer >= ChargeCooldown)
                {
                    StartCooldown = false;
                    CooldownTimer = 0;
                }
            }
        }

        if (StartKnockBack)
        {
            KnockBackTimer += Time.deltaTime;
            if (KnockBackTimer > KnockBackTime)
            {
                StartKnockBack = false;
                KnockBackTimer = 0;
            }
        }
        //Debug.Log("Speeds: " + gameObject.name + " " + RB.velocity.magnitude);

    }

    public void DashUp(float Ratio = 1)
    {
        RB.AddForce(Vector3.forward * ChargeForce * Ratio, ForceMode.Impulse);
    }

    public void DashDown(float Ratio = 1)
    {
        RB.AddForce(-Vector3.forward * ChargeForce * Ratio, ForceMode.Impulse);
    }

    public void DashRight(float Ratio = 1)
    {
        RB.AddForce(Vector3.right * ChargeForce * Ratio, ForceMode.Impulse);
    }

    public void DashLeft(float Ratio = 1)
    {
        RB.AddForce(-Vector3.right * ChargeForce * Ratio, ForceMode.Impulse);
    }

    private void ResetBools()
    {
        ChargeUp = false;
        ChargeDown = false;
        ChargeRight = false;
        ChargeLeft = false;
    }

    public float GetCooldownTime()
    {
        return ChargeCooldown;
    }

    public float GetCooldownTimer()
    {
        return CooldownTimer;
    }

    public Rigidbody GetRigidbody()
    {
        return RB;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AISumo" && gameObject.tag == "Player")
        {
            if (!StartKnockBack)
            {
                Rigidbody RBOther = collision.gameObject.GetComponent<Rigidbody>();
                SumoController SCOther = collision.gameObject.GetComponent<SumoController>();
                NavMeshAgent NA = collision.gameObject.GetComponent<NavMeshAgent>();
                Vector3 Direction = (collision.gameObject.transform.position - transform.position).normalized;
                float SpeedInDirectionPersonal = Mathf.Abs(RB.velocity.magnitude);
                float SpeedInDirectionOther = Mathf.Abs(NA.velocity.magnitude);

                //Debug.Log("Speeds: " + gameObject.name + " " + SpeedInDirectionPersonal + " " + collision.gameObject.name + " " + SpeedInDirectionOther);
                //float Force = 0;
                if (SpeedInDirectionPersonal > SpeedInDirectionOther)
                {
                    RB.velocity = Vector3.zero;
                    RB.AddForce(-Direction * CollisionForce * WinKnockbackPercent / 100, ForceMode.Impulse);
                    //Force = ChargeForce * WinKnockbackPercent / 100;
                    RBOther.AddForce(Direction * CollisionForce * (LossKnockBackPercent + 20) / 100, ForceMode.Impulse); // 20 is the fudge factor
                    //Debug.Log(gameObject.name + " Was Hit with (Win)");
                    SCOther.LostCollision = true;
                }
                else if (SpeedInDirectionPersonal < SpeedInDirectionOther)
                {
                    RB.velocity = Vector3.zero;
                    RB.AddForce(-Direction * CollisionForce * LossKnockBackPercent / 100, ForceMode.Impulse);
                    //Force = ChargeForce * LossKnockBackPercent / 100;
                    RBOther.AddForce(Direction * CollisionForce * WinKnockbackPercent / 100, ForceMode.Impulse);
                    //Debug.Log(gameObject.name + " Was Hit with (Loss)");
                }
                else
                {
                    RB.velocity = Vector3.zero;
                    RB.AddForce(-Direction * CollisionForce * TieKnockBackPercent / 100, ForceMode.Impulse);
                    RBOther.AddForce(Direction * CollisionForce * TieKnockBackPercent / 100, ForceMode.Impulse);
                    //Force = ChargeForce * TieKnockBackPercent / 100;
                    //Debug.Log(gameObject.name + " Was Hit with (Tie)");
                }
            }
        }
        else if (collision.gameObject.tag == "AISumo" && gameObject.tag == "AISumo")
        {
            if (!StartKnockBack)
            {
                Rigidbody RBOther = collision.gameObject.GetComponent<Rigidbody>();
                NavMeshAgent NA = collision.gameObject.GetComponent<NavMeshAgent>();
                NavMeshAgent NAPersonal = gameObject.GetComponent<NavMeshAgent>();
                Vector3 Direction = (collision.gameObject.transform.position - transform.position).normalized;
                float SpeedInDirectionPersonal = Mathf.Abs(NAPersonal.velocity.magnitude);
                float SpeedInDirectionOther = Mathf.Abs(NA.velocity.magnitude);

                //Debug.Log("Speeds: " + gameObject.name + " " + SpeedInDirectionPersonal + " " + collision.gameObject.name + " " + SpeedInDirectionOther);
                //float Force = 0;
                if (SpeedInDirectionPersonal > SpeedInDirectionOther)
                {
                    RB.velocity = Vector3.zero;
                    RB.AddForce(-Direction * CollisionForce * WinKnockbackPercent / 100, ForceMode.Impulse);
                    //Force = ChargeForce * WinKnockbackPercent / 100;
                    //Debug.Log(gameObject.name + " Was Hit with (Win)");
                }
                else if (SpeedInDirectionPersonal < SpeedInDirectionOther)
                {
                    RB.velocity = Vector3.zero;
                    RB.AddForce(-Direction * CollisionForce * LossKnockBackPercent / 100, ForceMode.Impulse);
                    //Force = ChargeForce * LossKnockBackPercent / 100;
                    //Debug.Log(gameObject.name + " Was Hit with (Loss)");
                }
                else
                {
                    RB.velocity = Vector3.zero;
                    RB.AddForce(-Direction * CollisionForce * TieKnockBackPercent / 100, ForceMode.Impulse);
                    //Force = ChargeForce * TieKnockBackPercent / 100;
                    //Debug.Log(gameObject.name + " Was Hit with (Tie)");
                }
            }
        }
        if (gameObject.tag == "AISumo" && collision.gameObject.tag == "Player")
        {
            //if (!StartKnockBack)
            //{
            //    Rigidbody RBOther = collision.gameObject.GetComponent<Rigidbody>();
            //    NavMeshAgent NA = gameObject.GetComponent<NavMeshAgent>();
            //    Vector3 Direction = (collision.gameObject.transform.position - transform.position).normalized;
            //    float SpeedInDirectionPersonal = Mathf.Abs(NA.velocity.magnitude);
            //    float SpeedInDirectionOther = Mathf.Abs(RBOther.velocity.magnitude);

            //    //Debug.Log("Speeds: " + gameObject.name + " " + SpeedInDirectionPersonal + " " + collision.gameObject.name + " " + SpeedInDirectionOther);
            //    float Force = 0;
            //    if (SpeedInDirectionPersonal > SpeedInDirectionOther)
            //    {
            //        RB.velocity = Vector3.zero;
            //        RB.AddForce(-Direction * ChargeForce * WinKnockbackPercent / 100, ForceMode.Impulse);
            //        Force = ChargeForce * WinKnockbackPercent / 100;
            //        Debug.Log(gameObject.name + " Was Hit with (Win)");
            //    }
            //    else if (SpeedInDirectionPersonal < SpeedInDirectionOther)
            //    {
            //        RB.velocity = Vector3.zero;
            //        RB.AddForce(-Direction * ChargeForce * LossKnockBackPercent / 100, ForceMode.Impulse);
            //        Force = ChargeForce * LossKnockBackPercent / 100;
            //        Debug.Log(gameObject.name + " Was Hit with (Loss)");
            //    }
            //    else
            //    {
            //        RB.velocity = Vector3.zero;
            //        RB.AddForce(-Direction * ChargeForce * TieKnockBackPercent / 100, ForceMode.Impulse);
            //        Force = ChargeForce * TieKnockBackPercent / 100;
            //        Debug.Log(gameObject.name + " Was Hit with (Tie)");
            //    }
            //}
        }
    }
}
