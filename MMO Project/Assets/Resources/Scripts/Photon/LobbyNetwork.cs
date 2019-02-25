using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("Connecting to Server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnConnectedToMaster()
    {
        print("Connected to Master.");
        PhotonNetwork.LocalPlayer.NickName = PlayerNetwork.Instance.name;

        PhotonNetwork.JoinLobby();
    }

    private void OnJoinedLobby()
    {
        print("Connected to Lobby.");
    }

    private void OnFailedToConnectToPhoton()
    {
        print("Connected to Master: Invalid AppID or Network Issues");
    }

    private void OnConnectionFail()
    {
        print("Connected to Master: Invalid Region or Maxed out CCU");
    }


}
