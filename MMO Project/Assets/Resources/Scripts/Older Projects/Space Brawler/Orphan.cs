using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orphan : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        transform.parent = null;
    }
}
	
