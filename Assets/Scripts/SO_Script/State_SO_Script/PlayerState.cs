using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : StateBase
{
    [SerializeField] protected GameObject m_Player;

    protected virtual void Awake()
    {
        m_Player = GameObject.Find("Player");

    }
}
