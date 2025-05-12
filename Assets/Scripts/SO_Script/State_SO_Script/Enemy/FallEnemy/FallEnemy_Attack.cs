using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallEnemy_Attack", menuName = "ScriptableObject/Enemy/FallEnemy/FallEnemy_Attack")]
public class FallEnemy_Attack : FallEnemy
{
    private Animator animator;
    private bool hasDamagedPlayer = false;
    private GameObject impactAreaIndicator;

    public override void OnAwake()
    {
        base.OnAwake();
        animator = m_enemy.GetComponent<Animator>();
        impactAreaIndicator = m_enemy.transform.Find("ImpactAreaIndicator").gameObject;
    }

    public override void OnEnter()
    {
        // �������͵��صĶ���
        animator.SetTrigger("EnlargeAndFall");
        hasDamagedPlayer = false;
        impactAreaIndicator.SetActive(true);
    }

    public override void OnUpdate()
    {
        // ��⶯������
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Fall") && stateInfo.normalizedTime >= 0.5f && !hasDamagedPlayer)
        {
            // �ڶ������е�һ��ʱ��Player����˺�
            DamagePlayer();
            hasDamagedPlayer = true;
        }
        else if (stateInfo.normalizedTime >= 1.0f)
        {
            // �ر��һ���Χ��ʾ
            impactAreaIndicator.SetActive(false);
        }
    }

    public override void OnExit()
    {

        
    }

    private void DamagePlayer()
    {
        // ��Player����˺����߼�
        Collider[] hitColliders = Physics.OverlapSphere(m_enemy.transform.position, 5.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // ����Player��һ��TakeDamage����
                //hitCollider.GetComponent<Player>().TakeDamage(10);

            }
        }
    }
}

