using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DashEnemy_Attack", menuName = "ScriptableObject/Enemy/DashEnemy/DashEnemy_Attack", order = 0)]
public class DashEnemy_Attack : DashEnemy
{

    [SerializeField] private float m_speed = 1.0f;
    private Vector3 target;

    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void OnEnter()
    {
        // ÇÐ»»¶¯»­
        target = player.transform.position;

    }

    public override void OnUpdate()
    {
        float step = m_speed * Time.deltaTime;
        if (enemy.transform.position != player.transform.position)
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, step);
        else
            enemy.GetComponent<StateMachine>().TransitionState("DashEnemy_Idle");
    }

    public override void OnExit()
    {
        controller.isAttacking = false;
    }
}
