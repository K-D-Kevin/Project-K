using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState : MonoBehaviour{

    protected FSMStateManager SMOwner; // Owner

    public abstract void Init();

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

    public abstract void OnShutdown();

    public abstract void FSMOnCollisionEnter(Collision c);

    public abstract void GetStateManager(FSMStateManager sm);

    protected FSMStateIDs.StateIds m_stateId;

    public FSMStateIDs.StateIds GetStateIDs()
    {
        return m_stateId; 
    }
}
