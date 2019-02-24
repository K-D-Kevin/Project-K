using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SumoManager : MonoBehaviour {

    private int AiNum = 0;
    private int AiLeft = 0;
    private int RoundNum = 0;
    private int CurrentRound = 0;

    [SerializeField]
    private Canvas Menus;
    [SerializeField]
    private Text VictorText;
    [SerializeField]
    private Text PlayerScoreText;
    [SerializeField]
    private Text AiScoreText;
    [SerializeField]
    private Text RoundNumText;

    private int AIScore = 0;
    private int PlayerScore = 0;

    [SerializeField]
    private Transform PlayerSpawn;
    [SerializeField]
    private Transform AISpawn1;
    [SerializeField]
    private Transform AISpawn2;
    [SerializeField]
    private Transform AISpawn3;
    [SerializeField]
    private Transform AIRefSpawn;

    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject AIPrefab;
    [SerializeField]
    private GameObject AIRefPrefab;

    private GameObject PlayerTemp = null;
    private GameObject AiTemp1 = null;
    private GameObject AiTemp2 = null;
    private GameObject AiRef = null;

    private PointScoring[] PointList;

    private void Start()
    {
        PointList = FindObjectsOfType<PointScoring>();
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        AiScoreText.text = "" + AIScore;
        PlayerScoreText.text = "" + PlayerScore;
        RoundNumText.text = "" + CurrentRound;
        if (AiNum != 0 && RoundNum != 0)
        {
            if (AiTemp1 == null && AiTemp2 == null && PlayerTemp == null)
            {
                CurrentRound++;
                if (CurrentRound <= RoundNum)
                {
                    if (AiNum == 1)
                    {
                        AiTemp1 = Instantiate(AIPrefab, AISpawn1);
                    }
                    else
                    {
                        AiTemp1 = Instantiate(AIPrefab, AISpawn2);
                        AiTemp2 = Instantiate(AIPrefab, AISpawn3);
                    }
                    AiLeft = AiNum;
                    PlayerTemp = Instantiate(PlayerPrefab, PlayerSpawn);
                    AiRef = Instantiate(AIRefPrefab, AIRefSpawn);
                }
                else
                {
                    if (PlayerScore > AIScore)
                    {
                        VictorText.gameObject.SetActive(true);
                    }
                    else if (PlayerScore < AIScore)
                    {
                        VictorText.gameObject.SetActive(true);
                        VictorText.text = "AI Wins";
                    }
                    AiNum = 0;
                    RoundNum = 0;
                }

                if (CurrentRound == 1)
                {
                    foreach (PointScoring PS in PointList)
                    {
                        if (AiNum == 1)
                        {
                            PS.SetWeighting(0.4f, 0.4f, 0.2f);
                            PS.SetTargets(PlayerTemp, AiTemp1, AiRef);
                        }
                        else
                        {
                            PS.SetWeighting(0.5f, 0.3f, 0.3f, 0.1f);
                            PS.SetTargets(PlayerTemp, AiTemp1, AiTemp2, AiRef);
                        }
                    }
                }
                else
                {
                    foreach (PointScoring PS in PointList)
                    {
                        if (AiNum == 1)
                        {
                            PS.SetTargets(PlayerTemp, AiTemp1, AiRef);
                        }
                        else
                        {
                            PS.SetTargets(PlayerTemp, AiTemp1, AiTemp2, AiRef);
                        }
                    }
                }
            }
        }
    }

    public void SetAINum(int AiCount)
    {
        AiNum = AiCount;
    }

    public void SetRounds(int rounds)
    {
        RoundNum = rounds;
    }

    public void AddScoreAi()
    {
        
        AIScore++;
        if (AiTemp1 != null)
        {
            Destroy(AiTemp1);
            AiTemp1 = null;
        }
        if (AiTemp2 != null)
        {
            Destroy(AiTemp2);
            AiTemp2 = null;
        }
        Destroy(AiRef);
        AiRef = null;
    }

    public void AddScorePlayer()
    {
        AiLeft--;
        if (AiLeft == 0)
        {
            PlayerScore++;
            if (PlayerTemp != null)
            {
                Destroy(PlayerTemp);
                PlayerTemp = null;
            }
        }
        Destroy(AiRef);
        AiRef = null;
    }

    public int GetAiNum()
    {
        return AiNum;
    }
}
