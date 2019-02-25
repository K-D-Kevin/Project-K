using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateRoom : MonoBehaviour {

    [SerializeField]
    private InputField RoomInputField;
    private string RoomName;

    public void CreatePhotonRoom()
    {
        RoomName = RoomInputField.text;
        if (RoomName == "")
        {
            RoomName = PlayerNetwork.Instance.name + "'s Room";
        }
        if (PhotonNetwork.CreateRoom(RoomName))
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

    private void OnCreatedRoom()
    {
        print("Room Created.");
    }
}
