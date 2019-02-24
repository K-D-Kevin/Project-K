using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    private float MaxHealth = 100;
    public float hp;

    [SerializeField]
    private float BaseResources = 50;
    private float Resources;

    [SerializeField]
    private float CollisionDamage = 15;

    private Health HitObject;
    private Health ObjectThatHitMe;

	// Use this for initialization
	void Start () {
        hp = MaxHealth;
        Resources = BaseResources;
	}

    private void Update()
    {
        if (hp <= 0)
        {
            if (ObjectThatHitMe != null)
            {
                ObjectThatHitMe.AddResources(Resources);
                ObjectThatHitMe.RemoveWhoIHit();
                ObjectThatHitMe.removeWhoHitMe();
            }
        }
    }

    public float CheckHealth()
    {
        return hp;
    }
    public void Damage(float dmg)
    {
        hp -= dmg;
    }

    public bool HealthZero()
    {
        return hp <= 0;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetWhoIHit(Health WhoIHit)
    {
        HitObject = WhoIHit;
    }

    public void SetWhoHitMe(Health WhoHitMe)
    {
        ObjectThatHitMe = WhoHitMe;
    }

    public Health WhoHitMe()
    {
        return ObjectThatHitMe;
    }

    public Health WhoDidIHit()
    {
        return HitObject;
    }

    public void AddResources(float resource)
    {
        Resources += resource;
    }

    public void RemoveResources(float resource)
    {
        Resources -= resource;
    }

    public float CheckResources()
    {
        return Resources;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health HPOther = collision.gameObject.GetComponent<Health>();
        HPOther.Damage(CollisionDamage);
    }

    public void RemoveWhoIHit()
    {
        HitObject = null;
    }

    public void removeWhoHitMe()
    {
        ObjectThatHitMe = null;
    }
}
