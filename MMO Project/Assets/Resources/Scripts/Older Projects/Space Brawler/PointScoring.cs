using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScoring : MonoBehaviour {

    [SerializeField]
    private GameObject PrimaryTarget = null;
    [SerializeField]
    private GameObject SecondaryTarget = null;
    [SerializeField]
    private GameObject TertiaryTarget = null;
    [SerializeField]
    private GameObject ExtraTarget = null;

    [SerializeField]
    private float PrimaryWeight = 1;
    [SerializeField]
    private float SecondaryWeight = 1;
    [SerializeField]
    private float TertiaryWeight = 1;
    [SerializeField]
    private float ExtraWeight = 0;

    private float PrimaryScore = 0;
    private float SecondaryScore = 0;
    private float TertiaryScore = 0;
    private float ExtraScore = 0;

    private Transform T;

    private void Start()
    {
        T = transform;
    }

    // Update is called once per frame
    void Update () {
		if (PrimaryTarget != null)
        {
            PrimaryScore = PrimaryWeight * (PrimaryTarget.transform.position - T.position).magnitude;

            if (SecondaryTarget != null)
            {
                SecondaryScore = SecondaryWeight * (SecondaryTarget.transform.position - T.position).magnitude;

                if (TertiaryTarget != null)
                {
                    TertiaryScore = TertiaryWeight * (TertiaryTarget.transform.position - T.position).magnitude;
                    
                    if (ExtraTarget != null)
                    {
                        ExtraScore = ExtraWeight * (ExtraTarget.transform.position - T.position).magnitude;
                    }
                }
            }
        }
        //Debug.Log(gameObject.name + ", Scores: " + PrimaryScore + ", " + SecondaryScore + ", " + TertiaryScore + ", " + ExtraScore);
        //float LowestScore = PrimaryScore + SecondaryScore + TertiaryScore + ExtraScore;
        //Debug.Log(gameObject.name + ", Total Scores: " + LowestScore);
    }

    public void SetTargets(GameObject Primary = null, GameObject Secondary = null, GameObject Tertiary = null, GameObject Extra = null, bool ResetTargets = false)
    {
        if (!(Primary == null))
        {
            PrimaryTarget = Primary;
        }
        else if (ResetTargets && Primary == null)
        {
            PrimaryTarget = null;
        }

        if (!(Secondary == null))
        {
            SecondaryTarget = Secondary;
        }
        else if (ResetTargets && Secondary == null)
        {
            SecondaryTarget = null;
        }

        if (!(Tertiary == null))
        {
            TertiaryTarget = Tertiary;
        }
        else if (ResetTargets && Tertiary == null)
        {
            TertiaryTarget = null;
        }

        if (!(Extra == null))
        {
            ExtraTarget = Extra;
        }
        else if (ResetTargets && Extra == null)
        {
            ExtraTarget = null;
        }
    }

    public void SetWeighting(float Primary = -1f, float Secondary = -1f, float Tertiary = -1f, float Extra = -1f)
    {
        if (!(Primary == -1f))
        {
            PrimaryWeight = Primary;
        }
        if (!(Secondary == -1f))
        {
            SecondaryWeight = Secondary;
        }
        if (!(Tertiary == -1f))
        {
            TertiaryWeight = Tertiary;
        }
        if (Extra != -1f)
        {
            ExtraWeight = Extra;
        }
    }

    public float GetPrimaryScore()
    {
        return PrimaryScore;
    }

    public float GetSecondaryScore()
    {
        return SecondaryScore;
    }

    public float GetTertiaryScore()
    {
        return TertiaryScore;
    }

    public float GetExtraScore()
    {
        return ExtraScore;
    }
}
