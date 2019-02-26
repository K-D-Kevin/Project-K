using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks {

    [SerializeField]
    private int MaxTeamPlayers = 6;
    private int Team1Players = 0;
    private int Team2Players = 0;
    private int QueuePlayers = 0;

    [SerializeField]
    private Text RoomText;
    [SerializeField]
    private Text RoomType;
    [SerializeField]
    private RectTransform Team1Content;
    [SerializeField]
    private RectTransform Team2Content;
    [SerializeField]
    private RectTransform QueueContent;
    [SerializeField]
    private PlayerRoomUI RPU_Prefab;

    private List<PlayerRoomUI> PlayerRoomUIList = new List<PlayerRoomUI>();

    private List<Player> Team1PlayerList = new List<Player>();
    private List<Player> Team2PlayerList = new List<Player>();
    private List<Player> QueuePlayerList = new List<Player>();

    private Player[] RoomPlayerList;

    private void Awake()
    {
        if (PhotonNetwork.InRoom)
        {
            RoomText.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomNameKey"];
            RoomType.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomTypeKey"];

            RoomPlayerList = PhotonNetwork.PlayerList;
            for (int i = 0; i < RoomPlayerList.Length; i++)
            {
                Player P = RoomPlayerList[i];

                // Set new custom properties for each player
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
                hash.Add("Team", -1);
                hash.Add("ReadyToPlay", false);
                hash.Add("RoomIndentifier", PhotonNetwork.CurrentRoom.Name);
                P.SetCustomProperties(hash);
            }
        }
    }

    void Update()
    {
        CheckPlayerTeam();
        UpdateLists();
    }

    private void CheckPlayerTeam()
    {
        foreach (Player P in RoomPlayerList)
        {
            object value;
            if (P.CustomProperties.TryGetValue("Team", out value))
            {
                if ((int)value == -1)
                {
                    QueuePlayerList.Add(P);
                    if (Team1PlayerList.Contains(P))
                    {
                        Team1PlayerList.Remove(P);
                    }
                    if (Team2PlayerList.Contains(P))
                    {
                        Team2PlayerList.Remove(P);
                    }
                }
                else if ((int)value == 1)
                {
                    Team1PlayerList.Add(P);
                    if (QueuePlayerList.Contains(P))
                    {
                        QueuePlayerList.Remove(P);
                    }
                    if (Team2PlayerList.Contains(P))
                    {
                        Team2PlayerList.Remove(P);
                    }
                }
                else if ((int)value == 2)
                {
                    Team2PlayerList.Add(P);
                    if (QueuePlayerList.Contains(P))
                    {
                        QueuePlayerList.Remove(P);
                    }
                    if (Team1PlayerList.Contains(P))
                    {
                        Team1PlayerList.Remove(P);
                    }
                }
            }
        }
    }

    private void UpdateLists()
    {
        List<PlayerRoomUI> NewPlayerRoomUIList = new List<PlayerRoomUI>();
        for(int i = 1; i <= RoomPlayerList.Length; i++)
        {
            if (PlayerRoomUIList.Count < i)
            {
                PlayerRoomUI temp = Instantiate(RPU_Prefab);
                int Team = (int)RoomPlayerList[i - 1].CustomProperties["Team"];
                RectTransform rt = temp.GetComponent<RectTransform>();
                temp.PlayerIdentifier ="Player " + RoomPlayerList[i - 1].ActorNumber;
                temp.SetName();
                if (Team == -1)
                {
                    rt.SetParent(QueueContent, false);
                }
                else if (Team == 1)
                {
                    rt.SetParent(Team1Content, false);
                }
                else if (Team == 2)
                {
                    rt.SetParent(Team2Content, false);
                }
                NewPlayerRoomUIList.Add(temp);
            }
            else
            {
                PlayerRoomUI temp = PlayerRoomUIList[i - 1];
                int Team = (int)RoomPlayerList[i - 1].CustomProperties["Team"];
                RectTransform rt = temp.GetComponent<RectTransform>();
                temp.PlayerIdentifier = "Player " + RoomPlayerList[i - 1].ActorNumber;
                temp.SetName();
                if (Team == -1)
                {
                    rt.SetParent(QueueContent, false);
                }
                else if (Team == 1)
                {
                    rt.SetParent(Team1Content, false);
                }
                else if (Team == 2)
                {
                    rt.SetParent(Team2Content, false);
                }
            }
        }
        if (NewPlayerRoomUIList.Count > 0)
            PlayerRoomUIList.AddRange(NewPlayerRoomUIList);

        List<PlayerRoomUI> RemovePlayerRoomUIList = new List<PlayerRoomUI>();
        RoomPlayerList = PhotonNetwork.PlayerList;
        if (PlayerRoomUIList.Count > RoomPlayerList.Length)
        {
            foreach (PlayerRoomUI RPU in PlayerRoomUIList)
            {
                int actorNum;
                if (int.TryParse(RPU.PlayerIdentifier.Split(' ')[1], out actorNum))
                {
                    bool RemovePlayer = true;
                    foreach (Player P in RoomPlayerList)
                    {
                        if (P.ActorNumber == actorNum)
                        {
                            RemovePlayer = false;
                            break;
                        }
                    }
                    if (RemovePlayer)
                    {
                        RemovePlayerRoomUIList.Add(RPU);
                    }
                }
            }
        }

        if (RemovePlayerRoomUIList.Count > 0)
        {
            foreach(PlayerRoomUI RPU in RemovePlayerRoomUIList)
            {
                if (PlayerRoomUIList.Contains(RPU))
                {
                    PlayerRoomUIList.Remove(RPU);
                    Destroy(RPU.gameObject, 0.1f);
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        RoomText.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomNameKey"];
        RoomType.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomTypeKey"];
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Set new custom properties for each new player
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("Team", -1);
        hash.Add("ReadyToPlay", false);
        hash.Add("RoomIndentifier", PhotonNetwork.CurrentRoom.Name);
        newPlayer.SetCustomProperties(hash);
        base.OnPlayerEnteredRoom(newPlayer);

        // Update PlayerList
        RoomPlayerList = PhotonNetwork.PlayerList;
    }

    public void ChangeToTeam1()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        int Team = (int)hash["Team"];
        if (Team != 1)
        {
            hash.Remove("Team");
            hash.Add("Team", 1);

            PhotonNetwork.LocalPlayer.CustomProperties = hash;
        }
        bool IsReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["ReadyToPlay"];
        if (!IsReady)
        {
            hash.Remove("ReadyToPlay");
            hash.Add("ReadyToPlay", true);

            PhotonNetwork.LocalPlayer.CustomProperties = hash;
        }
    }

    public void ChangeToTeam2()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        int Team = (int)hash["Team"];
        if (Team != 2)
        {
            hash.Remove("Team");
            hash.Add("Team", 2);

            PhotonNetwork.LocalPlayer.CustomProperties = hash;
        }
        bool IsReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["ReadyToPlay"];
        if (!IsReady)
        {
            hash.Remove("ReadyToPlay");
            hash.Add("ReadyToPlay", true);

            PhotonNetwork.LocalPlayer.CustomProperties = hash;
        }
    }

    public void ChangeToQueue()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        int Team = (int)hash["Team"];
        if (Team != -1)
        {
            hash.Remove("Team");
            hash.Add("Team", -1);

            PhotonNetwork.LocalPlayer.CustomProperties = hash;
        }
        bool IsReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["ReadyToPlay"];
        if (IsReady)
        {
            hash.Remove("ReadyToPlay");
            hash.Add("ReadyToPlay", false);

            PhotonNetwork.LocalPlayer.CustomProperties = hash;
        }
    }
}
