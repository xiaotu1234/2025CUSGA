using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashEnemyController : EnemyController
{
    #region 冲刺敌人基本属性
    [Header("基本属性")]
    [SerializeField] private float damage;

    [Header("视野属性")]
    public float viewRadius = 5f;//视野距离
    public int viewAngleStep = 20;//射线密度
    [Range(0, 360)]
    public float viewAngle = 90f;//视野角度
    #endregion

    public bool isAttacking = false;
    public event Action OnDashEnemyDead;
    public event Action OnHurtPlayer;
    public Color color;
    private bool isDead = false;

    //策划加的变量
    public GameObject colorimg;
    public Animator dieanimator;


    protected override void Start()
    {
        base.Start();
        if (color == null)
            Debug.LogError("冲刺敌人的颜色未赋值");
        colorimg.GetComponent<SpriteRenderer>().color = color;
        m_fsm.TransitionState("DashEnemy_LocateTarget");
    }

    protected override void Update()
    {
        DrawFieldOfView();
        if (m_currentHP <= 0) 
        { 
            OnDashEnemyDead?.Invoke();
            return;
        }


    }
    private void DrawFieldOfView()
    {
        if (isDead) { 
            return;
        }
        // 计算最左侧方向的向量
        Vector3 forward_left = Quaternion.Euler(0, -(viewAngle / 2f), 0) * transform.forward * viewRadius;

        for (int i = 0; i <= viewAngleStep; i++)
        {
            Vector3 v = Quaternion.Euler(0, (viewAngle / viewAngleStep) * i, 0) * forward_left;// 根据当前角度计算方向向量
            Vector3 pos = transform.position + v;// 计算射线终点

            // 射线检测
            Ray ray = new Ray(transform.position, v);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, viewRadius))
            {
                if (hitInfo.collider.tag == "Player")
                {
                    
                    if (!isAttacking)
                    {
                        
                        StartAttack();
                    }
                }
            }
        }
    }

    protected override void Die()
    {
        dieanimator.SetTrigger("die");
        base.Die();
        m_fsm.TransitionState("DashEnemy_Die");
        this.gameObject.tag = "SkillBall";
        isDead = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            if(isDead) { return; }
                OnHurtPlayer?.Invoke();
                Debug.Log("Hurt Player");
                other.GetComponent<PlayerController>().TakeDamage((int)damage);
        }
    }

   

    public void StartAttack()
    {
        Debug.Log("开始攻击");
        isAttacking = true;
        m_fsm.TransitionState("DashEnemy_LocateTarget");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 forward_left = Quaternion.Euler(0, -(viewAngle / 2f), 0) * transform.forward * viewRadius;

        for (int i = 0; i <= viewAngleStep; i++)
        {
            Vector3 v = Quaternion.Euler(0, (viewAngle / viewAngleStep) * i, 0) * forward_left;// 根据当前角度计算方向向量
            Vector3 pos = transform.position + v;// 计算射线终点

            Gizmos.DrawLine(transform.position, pos);
        }
    }
}
