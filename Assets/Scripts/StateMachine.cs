using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour 
{
    public List<StateBase> states = new List<StateBase>();
    public StateBase currentState;
    private bool isOver = false;
    public virtual void Awake()
    {
        foreach (var state in states)
        {
           
            state.OnAwake();
        }
    }

    private void OnEnable()
    {
        BossManager.Instance.OnBossDie += () =>
        {
            currentState.OnExit();
            isOver = true;
        };
    }


    protected virtual void Update()
    {
        currentState.OnUpdate();//��Update�н��е�ǰ״̬��OnUpdate����
    }

    public virtual void TransitionState(string stateName)
    {
        if (isOver)
            return;
        if (currentState != null)
            currentState.OnExit();
        currentState = FindState(stateName);
        currentState.OnEnter();//ִ���л���״̬��OnEnter����
    }

    protected virtual StateBase FindState(string stateName)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].name == stateName)
                return states[i];
        }
        return null;
    }

}
