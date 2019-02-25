using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomLayoutGroup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RectTransform RT;

    [SerializeField]
    private RoomListing RoomPrefab;

    private List<RoomInfo> RoomList = new List<RoomInfo>();

    public void OnRecievedRoomList()
    {
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = roomList;
        foreach (RoomInfo Room in RoomList)
        {
            if (Room.IsVisible)
            {
                RoomListing temp = Instantiate(RoomPrefab);
                RectTransform TempRect = temp.GetComponent<RectTransform>();
                TempRect.SetParent(RT);
                temp.SetRoomIdentifier(Room.Name);
                //temp.UpdateInfo(Room.);
            }
        }
        base.OnRoomListUpdate(roomList);
    }
}
