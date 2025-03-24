using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DashEnemy : EnemyState
{ 
    protected GameObject player;
    protected DashEnemyController controller;
    protected override void Awake()
    {
        Debug.Log("Awake");
        enemy = GameObject.Find("DashEnemy");
        player = GameObject.Find("Player");
        controller = enemy.GetComponent<DashEnemyController>();
        if (controller == null ) 
            Debug.Log("Not Get ConTroller");
    }
}

