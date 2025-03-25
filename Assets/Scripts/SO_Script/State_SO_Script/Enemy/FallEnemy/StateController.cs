using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    //���������һ����������ͨ������ChangeState��ʵ�ֶ����л�
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
