using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnemy_Attack : EnemyState
{
    private Animator animator;
    private bool hasDamagedPlayer = false;
    private GameObject impactAreaIndicator;

    protected override void Awake()
    {
        base.Awake();
        animator = Enemy.GetComponent<Animator>();
        impactAreaIndicator = Enemy.transform.Find("ImpactAreaIndicator").gameObject;
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
            // ��������ʱ�˳�״̬
            OnExit();
        }
    }

    public override void OnExit()
    {
        // �ر��һ���Χ��ʾ
        impactAreaIndicator.SetActive(false);
        // �л�������״̬
        // ���磺EnemyStateMachine.ChangeState(new SomeOtherState());
    }

    private void DamagePlayer()
    {
        // ��Player����˺����߼�
        Collider[] hitColliders = Physics.OverlapSphere(Enemy.transform.position, 5.0f);
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

