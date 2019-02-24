using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMInputController : MonoBehaviour {

    [SerializeField]
    private FSMStateManager SM;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            SM.DesiredState = FSMStateIDs.StateIds.FSM_Idle;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            SM.DesiredState = FSMStateIDs.StateIds.FSM_Idle2;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            SM.DesiredState = FSMStateIDs.StateIds.FSM_Chase;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            SM.DesiredState = FSMStateIDs.StateIds.FSM_Flee;
        }
    }
}
