using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
	// Use this for initialization
	void Awake () {
        Instance = this;
        name = "Temp#" + Random.Range(1000, 99999);
    }
}
