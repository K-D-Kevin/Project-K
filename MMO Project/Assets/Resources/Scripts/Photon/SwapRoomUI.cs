using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SwapRoomUI : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject LobbyUI;
    [SerializeField]
    private GameObject RoomUI;

    public override void OnJoinedRoom()
    {
        LobbyUI.SetActive(false);
        RoomUI.SetActive(true);
        base.OnJoinedRoom();
    }
}
