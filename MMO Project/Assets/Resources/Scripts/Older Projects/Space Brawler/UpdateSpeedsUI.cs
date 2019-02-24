using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSpeedsUI : MonoBehaviour {

    /*
    [SerializeField]
    private ShipController Player;
    [SerializeField]
    private Text LinearText;
    [SerializeField]
    private Text StrafeText;
    [SerializeField]
    private Text RiseText;
    [SerializeField]
    private Text RollText;
    [SerializeField]
    private Text PitchText;
    [SerializeField]
    private Text YawText;
    private float Value = 0;

    [SerializeField]
    private Health Hp;

    [SerializeField]
    private Text HpText;
    [SerializeField]
    private Text ResourceText;


    // Update is called once per frame
    void Update () {
		if (Player != null)
        {
            // Update Forward Back Speed
            Value = Mathf.Round(Player.MoveVector.z * 100) / 100;
            LinearText.text = "" + Value + " m/s";

            // Update Strafe Speed
            Value = Mathf.Round(Player.MoveVector.x * 100) / 100;
            StrafeText.text = "" + Value + " m/s";

            // Update Rise Speed
            Value = Mathf.Round(Player.MoveVector.y * 100) / 100;
            RiseText.text = "" + Value + " m/s";

            // Update Roll Speed
            Value = Mathf.Round(Player.RotateVector.z * 100 * 180 / Mathf.PI) / 100;
            RollText.text = "" + Value + " deg/s";

            // Update Pitch Speed
            Value = Mathf.Round(Player.RotateVector.x * 100 * 180 / Mathf.PI) / 100;
            PitchText.text = "" + Value + " deg/s";

            // Update Yaw Speed
            Value = Mathf.Round(Player.RotateVector.y * 100 * 180 / Mathf.PI) / 100;
            YawText.text = "" + Value + " deg/s";

            // Update Health
            HpText.text = "" + Hp.CheckHealth();

            // Update Resources
            ResourceText.text = "" + Hp.CheckResources();
        }
	}
    */
}
