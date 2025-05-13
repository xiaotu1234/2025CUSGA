using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallEnemy_Seek", menuName = "ScriptableObject/Enemy/FallEnemy/FallEnemy_Seek")]
public class FallEmemy_Seek : FallEnemy
{
    private PlayerController player;
    private Transform target;

    [SerializeField] private float attackRange = 1f; // ������������
    [SerializeField] private float moveSpeed = 3f; // �ƶ��ٶ�
    [SerializeField] private Vector3 fallPositonOffset;
    public override void OnAwake()
    {
    }

    public override void OnEnter()
    {
        player = PlayerManager.Instance.player;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (player == null || m_enemy == null) return;
        
        target = player.transform;
        target.position += fallPositonOffset;
        // ���㷽���ƶ�
        Vector3 direction = (target.position - m_enemy.transform.position).normalized;
        m_enemy.transform.position += direction * moveSpeed * Time.deltaTime;

        // ����Ƿ񵽴﹥����Χ
        float distanceToPlayer = Vector3.Distance(m_enemy.transform.position, target.position);
        if (distanceToPlayer <= attackRange)
        {
            // �л�������״̬
            m_enemy.GetComponent<StateMachine>().TransitionState("FallEnemy_Attack");
        }
    }

}
