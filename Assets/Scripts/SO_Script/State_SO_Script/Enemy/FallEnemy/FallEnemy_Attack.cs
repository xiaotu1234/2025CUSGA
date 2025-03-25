using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnemy_Attack : FallEnemy
{
    private Animator animator;
    private bool hasDamagedPlayer = false;
    private GameObject impactAreaIndicator;

    protected override void Awake()
    {
        base.Awake();
        animator = enemy.GetComponent<Animator>();
        impactAreaIndicator = enemy.transform.Find("ImpactAreaIndicator").gameObject;
    }

    public override void OnEnter()
    {
        // 启动变大和倒地的动画
        animator.SetTrigger("EnlargeAndFall");
        hasDamagedPlayer = false;
        impactAreaIndicator.SetActive(true);
    }

    public override void OnUpdate()
    {
        // 检测动画进度
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Fall") && stateInfo.normalizedTime >= 0.5f && !hasDamagedPlayer)
        {
            // 在动画进行到一半时对Player造成伤害
            DamagePlayer();
            hasDamagedPlayer = true;
        }
        else if (stateInfo.normalizedTime >= 1.0f)
        {
            // 动画结束时退出状态
            OnExit();
        }
    }

    public override void OnExit()
    {
        // 关闭砸击范围提示
        impactAreaIndicator.SetActive(false);
        // 切换到其他状态
        // 例如：EnemyStateMachine.ChangeState(new SomeOtherState());
    }

    private void DamagePlayer()
    {
        // 对Player造成伤害的逻辑
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, 5.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // 假设Player有一个TakeDamage方法
                //hitCollider.GetComponent<Player>().TakeDamage(10);

            }
        }
    }
}

