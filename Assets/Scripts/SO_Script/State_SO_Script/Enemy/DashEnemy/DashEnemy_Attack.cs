using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DashEnemy_Attack", menuName = "ScriptableObject/Enemy/DashEnemy/DashEnemy_Attack", order = 0)]
public class DashEnemy_Attack : DashEnemy
{

    [SerializeField] private float m_speed = 1.0f;
    private Vector3 target;

    

    public override void OnEnter()
    {
        // ÇÐ»»¶¯»­
        target = m_player.transform.position;

    }

    public override void OnUpdate()
    {
        float step = m_speed * Time.deltaTime;
        if (enemy.transform.position == target)
            m_fsm.TransitionState("DashEnemy_Idle");
        else
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, step);
    }

    public override void OnExit()
    {

    }
}
