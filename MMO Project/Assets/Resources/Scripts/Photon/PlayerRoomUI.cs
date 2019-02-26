using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRoomUI : MonoBehaviour {

    [SerializeField]
    private Text NameText;

    private string PlayerName;
    public string PlayerIdentifier
    {
        get
        {
            return PlayerName;
        }
        set
        {
            PlayerName = value;
        }
    }

    public void SetName(string Name)
    {
        NameText.text = Name;
    }

    public void SetName()
    {
        NameText.text = PlayerName;
    }
}
