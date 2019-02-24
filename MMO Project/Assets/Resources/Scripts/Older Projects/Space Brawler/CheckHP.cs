using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHP : MonoBehaviour {

    [SerializeField]
    private Health HP;
	
	// Update is called once per frame
	void Update () {
		if (HP.CheckHealth() <= 0)
        {
            AIController[] TargetableObjects = FindObjectsOfType<AIController>();
            foreach (AIController G in TargetableObjects)
            {
                if (G != null)
                {
                    G.RemoveTargetFromList(gameObject);
                }
            }
            Destroy(gameObject);
        }
	}
}
