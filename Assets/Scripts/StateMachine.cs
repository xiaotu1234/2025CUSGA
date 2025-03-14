using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour 
{
    public List<StateBase> states = new List<StateBase>();
    public StateBase currentState;
    [SerializeField] protected Animator animatior;

    protected virtual void Start()
    {
        animatior = GetComponent<Animator>();
    }


    protected virtual void Update()
    {
        currentState.OnUpdate();//在Update中进行当前状态的OnUpdate方法
    }

    public void TransitionState(StateBase nextState)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();//执行切换后状态的OnEnter方法
    }


}
