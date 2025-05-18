using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallEnemy_Die", menuName = "ScriptableObject/Enemy/FallEnemy/FallEnemy_Die")]

public class FallEnemy_Die : FallEnemy
{
    public float keepTime = 6;
    private float keepTimer;
    public override void OnEnter()
    {
        keepTimer = 0;
    }
    public override void OnUpdate()
    {
        keepTimer += Time.deltaTime;
        if (keepTimer > keepTime)
            EnemyManager.Instance.DestroyEnemy(m_enemy.GetComponent<EnemyController>());
    }


    public override void OnExit()
    {
    }
}
