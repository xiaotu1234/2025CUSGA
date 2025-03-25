using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  FallEnemy : EnemyState
{
    protected override void Awake()
    {
        enemy = GameObject.Find("FallEnemy");
    }
    
}
