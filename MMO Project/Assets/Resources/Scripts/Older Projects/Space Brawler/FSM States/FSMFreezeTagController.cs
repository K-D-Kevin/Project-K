using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMFreezeTagController : MonoBehaviour {

    [SerializeField]
    private FSMStateManager SM;
    [SerializeField]
    private bool UseTagChaseFlee = true; // To use the Old system i set up before AI class on 11-10-2018

    private FreezeTagManager FM;
    private bool isIt = true;

    // Freeze Tag Objects
    List<FSMStateManager> targetableList = new List<FSMStateManager>();
    List<FSMStateManager> otherTargetingList = new List<FSMStateManager>();
    List<FSMStateManager> UnFreezingAi = new List<FSMStateManager>();
    List<FSMStateManager> FrozenAi = new List<FSMStateManager>();
    List<FSMStateManager> ChaseAi = new List<FSMStateManager>();
    List<FSMStateManager> FleeAi = new List<FSMStateManager>();
    private int AmountTargeting = 0;

    // Flee && Unfreezing
    [SerializeField]
    private float RunFromChaseDistance = 10f;
	
	// Update is called once per frame
	void Update () {
		if (!UseTagChaseFlee)
        {
            if (isIt)
            {
                // On collision with a flee / unfreeze AI will set target to null
                if (SM.Target == null)
                {
                    SelectTarget();
                }
            }
            else
            {
                // will reset on collision
                if (SM.Target == null)
                {
                    SelectTarget();
                }
                if (SM.Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_TagChase)
                {
                    SM.DesiredState = FSMStateIDs.StateIds.FSM_TagFlee;
                }
                else if (SM.Target.GetComponent<FSMStateManager>().m_currentState == FSMStateIDs.StateIds.FSM_Freeze)
                {
                    SM.DesiredState = FSMStateIDs.StateIds.FSM_UnFreeze;
                }
            }
        }
	}

    public void SetIt()
    {
        //Debug.Log("Point 4");
        if (UseTagChaseFlee)
            SM.DesiredState = FSMStateIDs.StateIds.FSM_TagChase;
    }

    public void SetFlee()
    {
        isIt = false;
        //Debug.Log("Point 5");
        if (UseTagChaseFlee)
            SM.ChangeState(FSMStateIDs.StateIds.FSM_TagFlee);
    }

    private void ScanFreezeTagObjects()
    {
        FSMStateManager[] tempArray = FindObjectsOfType<FSMStateManager>();
        if (UseTagChaseFlee)
        {
            foreach (FSMStateManager state in tempArray)
            {
                if (state.m_currentState == FSMStateIDs.StateIds.FSM_TagChase)
                {
                    ChaseAi.Add(state);
                }
                else if (state.m_currentState == FSMStateIDs.StateIds.FSM_UnFreeze)
                {
                    UnFreezingAi.Add(state);
                }
                else if (state.m_currentState == FSMStateIDs.StateIds.FSM_Freeze)
                {
                    FrozenAi.Add(state);
                }
                else if (state.m_currentState == FSMStateIDs.StateIds.FSM_TagFlee)
                {
                    FleeAi.Add(state);
                }
            }
            // remove self from lists
            if (SM.m_currentState == FSMStateIDs.StateIds.FSM_TagChase)
            {
                if (ChaseAi.Contains(SM))
                    ChaseAi.Remove(SM);
            }
            else if (SM.m_currentState == FSMStateIDs.StateIds.FSM_TagFlee)
            {
                if (FleeAi.Contains(SM))
                    FleeAi.Remove(SM);
            }
            else if (SM.m_currentState == FSMStateIDs.StateIds.FSM_UnFreeze)
            {
                if (UnFreezingAi.Contains(SM))
                    UnFreezingAi.Remove(SM);
            }
        }
    }

    private int FindNumberCoTargetting(FSMStateManager potentialTarget)
    {
        int targetNum = 0;
        if (isIt)
        {
            foreach (FSMStateManager sm in ChaseAi)
            {
                if (sm.Target == potentialTarget.gameObject)
                {
                    targetNum++;
                }
            }
        }
        else
        {
            foreach (FSMStateManager sm in UnFreezingAi)
            {
                if (sm.Target == potentialTarget.gameObject)
                {
                    targetNum++;
                }
            }
        }
        return targetNum;
    }

    private void SelectTarget()
    {
        // Scan Targets
        FindTargetables();
        // Weighting will be distance 40% and 60% for how many people are targetting the same target for It Ai

        // Weighting will be distance 30% and 20% how many people are targetting the same target for unfreezing Ai and 50% if the target is a ChaseAI
        if (UseTagChaseFlee)
        {
            if (isIt)
            {
                // a max distance score is right next to the AI and a max number targeting score is 0
                int cnt = 0;
                int bestTarget = 0;
                float tallyScore = 0f;
                foreach (FSMStateManager sm in targetableList)
                {
                    int targetingCount = FindNumberCoTargetting(sm);
                    float distance = Mathf.Abs((sm.transform.position - transform.position).magnitude);
                    float distanceScore = distance < 100 ? 40 * (1 - distance / 100)
                        : distance > 100 ? 0f
                        : 40f;
                    float targetScore = 60 / (targetingCount + 1);
                    float score = distanceScore + targetScore;
                    if (score > tallyScore)
                    {
                        bestTarget = cnt;
                        tallyScore = score;
                    }
                    cnt++;
                }
                SM.Target = targetableList[bestTarget].gameObject;
            }
            else
            {
                int cnt = 0;
                int bestTarget = 0;
                float tallyScore = 0f;
                foreach (FSMStateManager sm in targetableList)
                {
                    int targetingCount = FindNumberCoTargetting(sm);
                    float distance = Mathf.Abs((sm.transform.position - transform.position).magnitude);
                    float distanceScore = distance < 100 ? 30 * (1 - distance / 100)
                        : distance > 100 ? 0f
                        : 30f;
                    float targetScore = 20 / (targetingCount + 1);
                    float HarmfulScore = 0;
                    if (sm.m_currentState == FSMStateIDs.StateIds.FSM_TagChase && distance < RunFromChaseDistance)
                        HarmfulScore = 50f;
                    float score = distanceScore + targetScore + HarmfulScore;
                    if (score > tallyScore)
                    {
                        bestTarget = cnt;
                        tallyScore = score;
                    }
                    cnt++;
                }
                SM.Target = targetableList[bestTarget].gameObject;
            }
        }
    }  

    private void FindTargetables()
    {
        targetableList = new List<FSMStateManager>();
        ScanFreezeTagObjects();
        if (UseTagChaseFlee)
        {
            if (isIt)
            {
                targetableList.AddRange(UnFreezingAi);
                targetableList.AddRange(FleeAi);
            }
            else
            {
                targetableList.AddRange(FrozenAi);
                targetableList.AddRange(ChaseAi);
            }
        }
    }

    public void SetFreezeTagManager(FreezeTagManager fm)
    {
        FM = fm;
    }
}
