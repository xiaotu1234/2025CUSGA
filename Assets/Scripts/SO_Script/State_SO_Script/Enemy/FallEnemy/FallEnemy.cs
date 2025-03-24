using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  FallEnemy : EnemyState
{
    public override void OnAwake()
    {
        enemy = GameObject.Find("FallEnemy");
    }
    
}
