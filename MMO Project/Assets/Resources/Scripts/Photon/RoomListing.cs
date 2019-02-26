using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomListing : MonoBehaviour {

    public struct RoomInfo
    {
        public string RoomName;
        public string RoomIdentifier;
        public string RoomType;
        public float RoomPing;
        public int RoomCount;
        public int RoomMaxCount;
    }

    public RoomInfo RoomListingInfo;

    [SerializeField]
    private Text RoomNameText;
    public string RoomName
    {
        get { return RoomListingInfo.RoomName; }
        set { RoomListingInfo.RoomName = value; }
    }

    [SerializeField]
    private Text RoomTypeText;
    public string RoomType
    {
        get { return RoomListingInfo.RoomType; }
        set { RoomListingInfo.RoomType = value; }
    }

    [SerializeField]
    private Text RoomPingText;
    public float RoomPing
    {
        get { return RoomListingInfo.RoomPing; }
        set { RoomListingInfo.RoomPing = value; }
    }

    [SerializeField]
    private Text RoomCountText;
    public int RoomCount
    {
        get { return RoomListingInfo.RoomCount; }
        set { RoomListingInfo.RoomCount = value; }
    }
    public int RoomMaxCount
    {
        get { return RoomListingInfo.RoomMaxCount; }
        set { RoomListingInfo.RoomMaxCount = value; }
    }

    public bool Updated
    {
      get;
      set;
    }

    [PunRPC]
    public void RPC_UpdateRoom(string name, string type, float ping, int count, int maxCount)
    {
        UpdateInfo(name, type, ping, count, maxCount);
    }

    public void UpdateRoom()
    {
        RoomNameText.text = RoomListingInfo.RoomName;
        RoomTypeText.text = RoomListingInfo.RoomType;
        RoomPingText.text = "" + Mathf.Round(RoomListingInfo.RoomPing);
        RoomCountText.text = "" + RoomListingInfo.RoomCount + " / " + RoomListingInfo.RoomMaxCount;
    }

    public void UpdateInfo(string name, string type ,float ping, int count, int maxCount)
    {
        RoomListingInfo.RoomName = name;
        RoomListingInfo.RoomPing = ping;
        RoomListingInfo.RoomType = type;
        RoomListingInfo.RoomCount = count;
        RoomListingInfo.RoomMaxCount = maxCount;
        UpdateRoom();
    }

    public void SetRoomIdentifier(string identifier)
    {
        RoomListingInfo.RoomIdentifier = identifier;
    }

    public RoomInfo GetRoomInfo()
    {
        return RoomListingInfo;
    }

    public void SetLobbyNetworkRoom()
    {
        LobbyNetwork LN = FindObjectOfType<LobbyNetwork>();
        LN.SetRoom(this);
    }
}
