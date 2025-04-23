using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : StateBase
{
    [SerializeField] protected GameObject m_Player;
    public override void OnAwake()
    {
        m_Player = GameObject.Find("Player");

    }
}
