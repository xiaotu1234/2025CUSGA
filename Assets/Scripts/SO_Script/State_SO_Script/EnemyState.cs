using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class EnemyState : StateBase
{
    protected GameObject Enemy;

    protected EnemyState(Animator _animtor, string _animBoolName) : base(_animtor, _animBoolName)
    {
    }

    protected virtual void Awake()
    {
        //���ص���Ԥ����
        //m_Enemy = ����Ԥ����
    }

}
