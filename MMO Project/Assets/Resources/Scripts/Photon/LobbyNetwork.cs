using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyNetwork : MonoBehaviourPunCallbacks {

	// Use this for initialization
	void Start () {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.LocalPlayer.NickName = PlayerNetwork.Instance.PlayerName;
        PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.GameVersion = "1";

        print("Connecting to Server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        print("Connected");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected Cause: " + cause);
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to Master.");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
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
