using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour {

    [SerializeField]
    private float Damage = 10;

    [SerializeField]
    private float Speed = 50;

    [SerializeField]
    private Rigidbody RB;

    private Health WhoFiredMe;
    private Health WhoIHit;

    private float MinLifeTime = 0.02f;
    private float LifeTimer = 0;
    private bool CanDie = false;

    // Update is called once per frame
    void Update () {
        if (!CanDie)
        {
            CanDie = true;
        }
        
        RB.velocity = transform.forward * Speed;
	}

    private void OnCollisionEnter(Collision collision)
    {
        // Apply Damage
        if (CanDie)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NeutralAI" || collision.gameObject.tag == "AgressiveAi" || collision.gameObject.tag == "Astroid" || collision.gameObject.tag == "Comet")
            {
                // Damage
                WhoIHit = collision.gameObject.GetComponent<Health>();
                if (WhoFiredMe != WhoIHit)
                {
                    if (WhoIHit != null)
                    {
                        WhoIHit.SetWhoHitMe(WhoFiredMe);
                        WhoIHit.Damage(Damage);
                        WhoIHit.SetWhoIHit(WhoIHit);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Apply Damage
        if (CanDie)
        {
            if (other.tag == "Player" || other.tag == "NeutralAI" || other.tag == "AgressiveAi" || other.gameObject.tag == "Astroid" || other.tag == "Comet")
            {
                // Damage
                WhoIHit = other.GetComponent<Health>();
                if (WhoFiredMe != WhoIHit)
                {
                    if (WhoIHit != null)
                    {
                        WhoIHit.SetWhoHitMe(WhoFiredMe);
                        WhoIHit.Damage(Damage);
                        WhoIHit.SetWhoIHit(WhoIHit);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void SetDamage(float dmg)
    {
        Damage = dmg;
    }

    public float GetDamage()
    {
        return Damage;
    }

    public void SetWhoFiredMe(Health me)
    {
        WhoFiredMe = me;
    }

    public Health WhoIHitObject()
    {
        return WhoIHit;
    }
}
