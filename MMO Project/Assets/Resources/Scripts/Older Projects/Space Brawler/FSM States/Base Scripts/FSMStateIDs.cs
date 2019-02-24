using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStateIDs{

    public enum StateIds
    {
        FSM_Invalid = -1,
        FSM_Idle = 0,
        FSM_Idle2 = 1,
        FSM_Chase = 2,
        FSM_Flee = 3,
        FSM_Freeze = 4,
        FSM_UnFreeze = 5,
        FSM_TagChase = 6,
        FSM_TagFlee = 7,
        FSM_PlayerChase = 8,
        FSM_PlayerFlee = 9,
        FSM_PlayerFrozen = 10,
        FSM_IdleFlee = 11, // No Movement, just turning away from target
        FSM_IdleChase = 12, // No Movement, just turning towards target
        FSM_NavMeshFlee = 13,
        FSM_SumoIdle = 14,
        FSM_NavSumoChase = 15,
        FSM_NavSumoFlee = 16,
        FSM_NavSumoRef = 17,
        FSM_SumoPlayer = 18,
        FSM_BasePlayer = 19,
        FSM_PlayerIdle = 20,
        FSM_Count,
    }
}
