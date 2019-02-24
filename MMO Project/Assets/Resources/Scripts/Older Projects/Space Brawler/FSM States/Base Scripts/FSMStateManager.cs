using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStateManager : MonoBehaviour {

    public List<FSMState> m_states = new List<FSMState>();
    public FSMStateIDs.StateIds m_initialState = FSMStateIDs.StateIds.FSM_Invalid;


    public FSMStateIDs.StateIds m_currentState = FSMStateIDs.StateIds.FSM_Invalid;
    public FSMState currentFSMState;

    public GameObject Target;

    public FSMStateIDs.StateIds DesiredState;
    //{
    //    get;
    //    set;
    //}

    public Transform RightEye;

    public Transform LeftEye;

    public Camera Cam;

    [SerializeField]
    private Transform StateTransform;

    [SerializeField]
    private Transform FeetPos;

    public bool IsvalidState(FSMStateIDs.StateIds stateID)
    {
        //Debug.Log("Point 7");
        bool Valid = stateID != FSMStateIDs.StateIds.FSM_Invalid;
        if (Valid)
        {
            //Debug.Log("Point 9");
            bool ForValid = false;
            for (int i = 0; i < m_states.Count; i++)
            {
                //Debug.Log("Point 10");
                ForValid = m_states[i].GetStateIDs() == stateID;
                if (ForValid)
                    break;
            }
            Valid = ForValid;
        }
        return Valid;
    }
    
	// Use this for initialization
	void Start () {

        DesiredState = FSMStateIDs.StateIds.FSM_Invalid;

        for(int i = 0; i < m_states.Count; i++)
        {
            FSMState temp = Instantiate(m_states[i]);
            if (StateTransform != null)
                temp.transform.parent = StateTransform;
            else
                temp.transform.parent = this.transform;

            temp.GetStateManager(this);
            temp.Init();
            m_states[i] = temp;
        }

        if (IsvalidState(m_initialState))
        {
            m_currentState = m_initialState;
            FindState(m_currentState, true).OnEnter();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (m_currentState != FSMStateIDs.StateIds.FSM_Invalid)
        {

            if (!currentFSMState)
            {
                FindState(m_currentState).OnUpdate();
                //Debug.Log("Updating FSMState");
            }
            else
            {
                currentFSMState.OnUpdate();
                //Debug.Log("OnUpdate FSMState " + gameObject.name + currentFSMState.name);
            }
        }

        if (DesiredState != m_currentState && IsvalidState(DesiredState))
        {
            ChangeState(DesiredState);
            DesiredState = FSMStateIDs.StateIds.FSM_Invalid;
        }
    }

    public void ChangeState(FSMStateIDs.StateIds NextState)
    {
        //Debug.Log("Point 6");
        if (IsvalidState(NextState))
        {
            //Debug.Log("Point 8");
            FindState(m_currentState).OnExit();
            m_currentState = NextState;
            FindState(m_currentState, true).OnEnter();
            //Debug.Log("Point 12");
        }
    }

    public FSMState FindState(FSMStateIDs.StateIds state, bool ChangeCurrent = false)
    {
        //Debug.Log("Point 9");
        FSMState FoundState = m_states[0];
        for (int i = 0; i < m_states.Count; i++)
        {
            //Debug.Log("Point 10");
            m_states[i].Init();
            if (m_states[i].GetStateIDs() == state)
            {
                //Debug.Log("Point 11");
                FoundState = m_states[i];
                break;
            }
        }
        if (ChangeCurrent)
            currentFSMState = FoundState;
        return FoundState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        FindState(m_currentState).FSMOnCollisionEnter(collision);
    }

    public Transform GetFeet()
    {
        return FeetPos;
    }
}
