using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DashEnemy_Attack", menuName = "ScriptableObject/Enemy/DashEnemy/DashEnemy_Attack", order = 0)]
public class DashEnemy_Attack : DashEnemy
{

    [SerializeField] private float m_speed = 1.0f;
    private Vector3 target;
    private DashEnemyController controller;
    private bool isPlayerHurted;
    public float attackMaxContinueTime = 6f;
    private float attackTimer;


    public override void OnAwake()
    {
        base.OnAwake();
        controller = m_enemy.GetComponent<DashEnemyController>();
    }


    public override void OnEnter()
    {
        // ÇÐ»»¶¯»­
        target = new Vector3( m_player.transform.position.x, 
                                m_enemy.transform.position.y ,
                                m_player.transform.position.z);
        isPlayerHurted = false;
        controller.OnHurtPlayer += SetIsPlayerHurted;
        AudioManager.Instance.PlaySFX(0);
        attackTimer = 0;
    }

    public override void OnUpdate()
    {
        DashToPlayer();
        attackTimer += Time.deltaTime;
        if (attackTimer > attackMaxContinueTime)
            m_fsm.TransitionState("DashEnemy_LocateTarget");
    }

    private void DashToPlayer()
    {
        float step = m_speed * Time.deltaTime;
        if (m_enemy.transform.position != target && !isPlayerHurted)
            m_enemy.transform.position = Vector3.MoveTowards(m_enemy.transform.position, target, step);
        else
        {
            Debug.Log("Stop Tracing");
            m_fsm.TransitionState("DashEnemy_LocateTarget");
            controller.isAttacking = false;
        }
            
    }

    public override void OnExit()
    {
        controller.OnHurtPlayer -= SetIsPlayerHurted;

    }

  

  
    private void SetIsPlayerHurted()
    {
        Debug.Log("Player is Hurted");
        isPlayerHurted = true;
    }

}
