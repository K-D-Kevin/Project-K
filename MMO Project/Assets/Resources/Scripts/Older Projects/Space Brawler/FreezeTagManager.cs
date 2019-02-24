using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeTagManager : MonoBehaviour {

    [SerializeField]
    private FSMFreezeTagController AIPrefab;
    private FSMFreezeTagController[] AiList;

    [SerializeField]
    private int NumberAiIt = 2;

    [SerializeField]
    private int NumberAiFlee = 8;
    private int NumberOfAi;
    private int NumberOfAiFrozen = 0;

    [SerializeField]
    private Transform[] SpawnPoints;

    [SerializeField]
    private bool SpawnInOrder = false;

    [SerializeField]
    private float TimeTillWin = 5f; // Time in minutes
    private float WinTimer = 0f;

    [SerializeField]
    private bool StartGame = false;

    [SerializeField]
    private Text TimerText;

    [SerializeField]
    private Text WinText;

    [SerializeField]
    private bool SpawnPlayer = false;

    [SerializeField]
    private bool PlayerIt = true;

    [SerializeField]
    private FSMStateManager Player;
    private FSMStateManager PlayerSpawned;

    [SerializeField]
    private Transform PlayerSpawn;

    [SerializeField]
    private Camera WorldCam;
    //private bool Added = false;

    private void Awake()
    {
        NumberOfAi = NumberAiIt + NumberAiFlee;
        if (SpawnPoints.Length < NumberAiFlee + NumberAiIt)
        {
            Debug.LogError("Not Enough Spawn Points");
        }
        if (SpawnInOrder)
        {
            for (int i = 0; i < NumberOfAi; i++)
            {
                FSMFreezeTagController temp = Instantiate(AIPrefab, SpawnPoints[i]);
                temp.transform.parent = null;
                temp.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            List<Transform> SpawnsTaken = new List<Transform>();
            for (int i = 0; i < NumberOfAi; i++)
            {
                int Rand = Random.Range(0, SpawnPoints.Length);
                int cnt = 0;
                while (SpawnsTaken.Contains(SpawnPoints[Rand]))
                {
                    cnt++;
                    Rand = Random.Range(0, SpawnPoints.Length);
                    if (cnt > SpawnPoints.Length)
                    {
                        break;
                    }
                }
                FSMFreezeTagController temp = Instantiate(AIPrefab, SpawnPoints[Rand]);
                temp.transform.parent = null;
                temp.transform.localScale = new Vector3(1, 1, 1);
                SpawnsTaken.Add(SpawnPoints[Rand]);
            }
        }
        AiList = FindObjectsOfType<FSMFreezeTagController>();
    }
    // Use this for initialization
    void Start () {
        TimeTillWin *= 60; // Put win condition to seconds
        if (SpawnPlayer)
        {
            WorldCam.gameObject.SetActive(false);
            PlayerSpawned = Instantiate(Player, PlayerSpawn);
            PlayerSpawned.transform.parent = null;
            PlayerSpawned.transform.localScale = new Vector3(1, 1, 1);
            if (PlayerIt)
            {
                PlayerSpawned.DesiredState = FSMStateIDs.StateIds.FSM_PlayerChase;
                PlayerSpawned.gameObject.name = "Player It";
            }
            else
            {
                PlayerSpawned.DesiredState = FSMStateIDs.StateIds.FSM_PlayerFlee;
                PlayerSpawned.gameObject.name = "Player Flee";
                NumberAiFlee++;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("FTM Frozen Number: " + NumberOfAiFrozen);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (StartGame)
        {
            //Debug.Log("Game Started");
            WinTimer = 0f;
            int ItCnt = 0;
            int FleeCnt = 0;
            for (int i = 0; i < AiList.Length; i++)
            {
                //Debug.Log("Point 1");
                if (i < NumberAiIt)
                {
                    ItCnt++;
                    //Debug.Log("Point 2");
                    AiList[i].SetIt();
                    AiList[i].gameObject.name = "Chase AI - " + (ItCnt);
                    //Debug.Log("Set AI As It");
                }
                else
                {
                    FleeCnt++;
                    //Debug.Log("Point 3");
                    AiList[i].SetFlee();
                    AiList[i].gameObject.name = "Flee AI - " + (FleeCnt);
                    //Debug.Log("Set AI As Flee");
                }
                AiList[i].SetFreezeTagManager(this);
            }
            StartGame = false;
            //Debug.Log("Game Start Done");
        }
        else
        {
            //if (PlayerSpawned.m_currentState == FSMStateIDs.StateIds.FSM_PlayerFrozen && !Added)
            //{
            //    Added = true;
            //    AddToFrozen();
            //}
            //else if (PlayerSpawned.m_currentState == FSMStateIDs.StateIds.FSM_PlayerFlee && Added)
            //{
            //    Added = false;
            //    RemoveFromFrozen();
            //}
            if (WinTimer >= TimeTillWin)
            {
                // Fleeing AI win
                WinText.text = "Flee AI Win";
                WinText.gameObject.SetActive(true);
            }
            else if (NumberOfAiFrozen != NumberAiFlee)
            {
                WinTimer += Time.deltaTime;
            }
            TimerText.text = "Time: " + Mathf.Round(WinTimer * 100) / 100;
            if (NumberOfAiFrozen == NumberAiFlee)
            {
                // It AI Win
                WinText.text = "Chase AI Win";
                WinText.gameObject.SetActive(true);
            }
        }

        
	}

    public void PlayerSpawnIt(bool b = true)
    {
        PlayerIt = b;
        if (PlayerIt)
        {
            PlayerSpawned.DesiredState = FSMStateIDs.StateIds.FSM_PlayerChase;
            PlayerSpawned.gameObject.name = "Player It";
        }
        else
        {
            PlayerSpawned.DesiredState = FSMStateIDs.StateIds.FSM_PlayerFlee;
            PlayerSpawned.gameObject.name = "Player Flee";
            NumberAiFlee++;
        }
    }
    public void StartTheGame()
    {
        StartGame = true;
    }

    public void AddToFrozen()
    {
        NumberOfAiFrozen++;
    }

    public void RemoveFromFrozen()
    {
        NumberOfAiFrozen--;
    }
}
