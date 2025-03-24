using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour 
{
    public List<StateBase> states = new List<StateBase>();
    public StateBase currentState;




    protected virtual void Update()
    {
        currentState.OnUpdate();//在Update中进行当前状态的OnUpdate方法
    }

    public void TransitionState(string stateName)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = FindState(stateName);
        currentState.OnEnter();//执行切换后状态的OnEnter方法
    }

    protected StateBase FindState(string stateName)
    {
        for (int i = 0; i <= states.Count; i++)
        {
            if (states[i].name == stateName)
                return states[i];
        }
        return null;
    }

}
