using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.LocalPlayer.NickName = PlayerNetwork.Instance.PlayerName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "V# 0.0.1";

        print("Connecting to Server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        print(cause);
    }

    private void OnConnectedToMaster()
    {
        print("Connected to Master.");

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
