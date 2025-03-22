using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DashEnemy : EnemyState
{ 
    protected GameObject player;
    protected override void Awake()
    {
        enemy = GameObject.Find("DashEnemy");
        player = GameObject.Find("Player");
    }
}

