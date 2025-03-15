using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : StateBase
{
    [SerializeField] protected GameObject m_Player;

    protected PlayerState(Animator _animtor, string _animBoolName) : base(_animtor, _animBoolName)
    {
    }

    protected virtual void Awake()
    {
        m_Player = GameObject.Find("Player");

    }
}
