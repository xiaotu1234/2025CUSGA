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
        //加载敌人预制体
        //m_Enemy = 敌人预制体
    }

}
