using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour {

    [SerializeField]
    private RectTransform GreenBar;
    private Vector2 InitialPos;
    private float InitialWidth;
    private float CurrentHealthRatio = 1;
    private int MaxHp = 100;
    private float Hp = 100;

    [SerializeField]
    private Text HPText;

    // Use this for initialization
    public void InitialStart () {
        InitialWidth = GreenBar.rect.width;
        InitialPos = GreenBar.rect.position;
        UpdateNumbers();
	}

    public void SetHealthRatio(float ratio)
    {
        CurrentHealthRatio = ratio;
        Hp = ratio * MaxHp;
        ResetHealthBar();
        UpdateNumbers();
    }

    public void SetMaxHp(int MaxHealth)
    {
        MaxHp = MaxHealth;
        Hp = MaxHp * CurrentHealthRatio;
        UpdateNumbers();
    }

    public void SetCurrentHp(float health)
    {
        Hp = health;
        CurrentHealthRatio = Hp / MaxHp;
        ResetHealthBar();
        UpdateNumbers();
    }

    private void ResetHealthBar()
    {
        float MovePostion = (1 - CurrentHealthRatio) * InitialWidth / 2;
        GreenBar.localScale = new Vector3(CurrentHealthRatio, 1, 1);
        GreenBar.anchoredPosition = new Vector2(-InitialPos.x + MovePostion, 0);
    }

    private void UpdateNumbers()
    {
        float HpNum = Mathf.Round(Hp);
        HPText.text = "Health: " + HpNum + " / " + MaxHp;
    }
}
