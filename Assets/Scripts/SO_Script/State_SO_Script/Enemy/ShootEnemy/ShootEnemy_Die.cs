using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShootEnemy_Die", menuName = "ScriptableObject/Enemy/ShootEnemy/ShootEnemy_Die", order = 0)]
public class ShootEnemy_Die : ShootEnemy
{
    public float keepTime = 6;
    private float keepTimer;
    public override void OnEnter()
    {
        keepTimer = 0;

        if (m_animator != null)
        {
            // ¶¯»­Âß¼­
        }


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
