using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindChargeCharacter : MonoBehaviour {

    [SerializeField]
    private bool IsPlayer = true;

    [SerializeField]
    private Text ChargeText;

    private Charge PlayerCharge;
	
	// Update is called once per frame
	void Update () {
        if (IsPlayer)
        {
            if (PlayerCharge == null)
            {
                PlayerCharge = GameObject.FindGameObjectWithTag("Player").GetComponent<Charge>();
            }
            else
            {
                float floatText = Mathf.Round((PlayerCharge.GetCooldownTime() - PlayerCharge.GetCooldownTimer()) * 100) / 100;
                ChargeText.text = "" + floatText;
            }
        }
	}
}
