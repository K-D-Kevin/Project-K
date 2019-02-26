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

    private List<RoomListing> RoomListings = new List<RoomListing>();
    private List<RoomInfo> RoomList = new List<RoomInfo>();

    public void OnRecievedRoomListUpdate()
    {
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = roomList;
        foreach (RoomInfo RoomInfomation in RoomList)
        {
            int Index = RoomListings.FindIndex(x => x.RoomName == RoomInfomation.Name);
            if (Index == -1)
            {
                if (RoomInfomation.IsVisible && RoomInfomation.PlayerCount < RoomInfomation.MaxPlayers)
                {
                    RoomListing temp = Instantiate(RoomPrefab);
                    RectTransform TempRect = temp.GetComponent<RectTransform>();
                    TempRect.SetParent(RT, false);
                    RoomListings.Add(temp);
                    temp.Updated = true;
                    //temp.UpdateInfo(Room.);

                    Index = RoomListings.Count - 1;
                }
            }

            if (Index != -1)
            {
                RoomListing RL = RoomListings[Index];
                RL.SetRoomIdentifier(RoomInfomation.Name);
                string RoomName = (string)RoomInfomation.CustomProperties["RoomNameKey"];
                string RoomType = (string)RoomInfomation.CustomProperties["RoomTypeKey"];

                int RoomPing = PhotonNetwork.GetPing();
                RL.UpdateInfo(RoomName, RoomType, RoomPing, RoomInfomation.PlayerCount, RoomInfomation.MaxPlayers);
            }
        }
        base.OnRoomListUpdate(roomList);

        RemoveOldRooms();
    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();
        
        foreach (RoomListing rl in RoomListings)
        {
            if (!rl.Updated)
            {
                removeRooms.Add(rl);
            }
            else
            {
                rl.Updated = false;
            }
        }

        foreach (RoomListing rl in removeRooms)
        {
            removeRooms.Remove(rl);
            Destroy(rl.gameObject);
        }
    }
}
