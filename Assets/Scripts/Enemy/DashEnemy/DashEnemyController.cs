using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyController : MonoBehaviour
{
    #region 冲刺敌人基本属性
    [Header("基本属性")]
    [SerializeField] private float m_maxHP;
    [SerializeField] private float damage;
    private float m_currentHP;

    [Header("视野属性")]
    public float viewRadius = 5f;//视野距离
    public int viewAngleStep = 20;//射线密度
    [Range(0, 360)]
    public float viewAngle = 90f;//视野角度
    #endregion

    public bool isAttacking = false;
    public event Action OnDashEnemyDead;
    private StateMachine m_fsm;

    private void Awake()
    {
        m_currentHP = m_maxHP;
        m_fsm = GetComponent<StateMachine>();
    }


    private void Update()
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
        // 计算最左侧方向的向量
        Vector3 forward_left = Quaternion.Euler(0, -(viewAngle / 2f), 0) * transform.forward * viewRadius;

        for (int i = 0; i <= viewAngleStep; i++)
        {
            Vector3 v = Quaternion.Euler(0, (viewAngle / viewAngleStep) * i, 0) * forward_left;// 根据当前角度计算方向向量
            Vector3 pos = transform.position + v;// 计算射线终点

            // 在Scene中绘制线条(仅方便观察，Game视图中不可见)
            Debug.DrawLine(transform.position, pos, Color.red);

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



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            isAttacking = false;
            Debug.Log("Hurt Player");
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
