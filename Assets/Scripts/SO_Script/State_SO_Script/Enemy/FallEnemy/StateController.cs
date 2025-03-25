using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    //给对象挂载一个控制器，通过调用ChangeState来实现动画切换
    public StateBase currentState { get; private set; }


    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.OnEnter();
    }

    public void ChangeState(EnemyState _newState)
    {
        currentState.OnExit();
        currentState = _newState;
        currentState.OnEnter();
    }
}
