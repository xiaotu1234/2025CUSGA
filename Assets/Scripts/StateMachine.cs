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
        currentState.OnUpdate();//��Update�н��е�ǰ״̬��OnUpdate����
    }

    public void TransitionState(StateBase nextState)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();//ִ���л���״̬��OnEnter����
    }


}
