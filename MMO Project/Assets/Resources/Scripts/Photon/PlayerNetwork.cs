using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName;
	// Use this for initialization
	void Awake () {
        Instance = this;
        PlayerName = "Temp#" + Random.Range(1000, 99999);
    }
}
