using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, step);
    }

    public override void OnExit()
    {

    }
}
