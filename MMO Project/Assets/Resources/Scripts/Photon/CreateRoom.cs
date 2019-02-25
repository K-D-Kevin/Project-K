using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private InputField RoomInputField;
    private string RoomName;
    private string RoomIdentifier;

    [SerializeField]
    private RoomLayoutGroup RLG;

    public void CreatePhotonRoom()
    {
        RoomName = RoomInputField.text;
        RoomIdentifier = PlayerNetwork.Instance.name + "'s Room";
        if (RoomName == "")
        {
            RoomName = PlayerNetwork.Instance.name + "'s Room";
        }
        if (PhotonNetwork.CreateRoom(RoomIdentifier))
        {
            print("Create Room Request Sent.");
        }
        else
        {
            print("Create Room Request Failed to Send.");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] CodeAndMessege)
    {
        print("Room Failed Error: " + CodeAndMessege[1]);
    }

    public override void OnCreatedRoom()
    {
        print("Room Created.");
        if (PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.JoinRoom(RoomIdentifier);
    }

    public override void OnJoinedRoom()
    {
        print("Room Joined.");
        if (PhotonNetwork.IsMasterClient)
        {
            Room Current = PhotonNetwork.CurrentRoom;
            Current.MaxPlayers = 12;
            RoomIdentifier = Current.Name;
        }
        
        
        base.OnJoinedRoom();
    }
}
