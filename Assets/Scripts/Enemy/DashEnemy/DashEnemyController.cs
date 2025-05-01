using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyController : EnemyController
{
    #region ��̵��˻�������
    [Header("��������")]
    [SerializeField] private float m_maxHP;
    [SerializeField] private float damage;
    private float m_currentHP;

    [Header("��Ұ����")]
    public float viewRadius = 5f;//��Ұ����
    public int viewAngleStep = 20;//�����ܶ�
    [Range(0, 360)]
    public float viewAngle = 90f;//��Ұ�Ƕ�
    #endregion

    public bool isAttacking = false;
    public event Action OnDashEnemyDead;
    public event Action OnHurtPlayer;
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
        // ��������෽�������
        Vector3 forward_left = Quaternion.Euler(0, -(viewAngle / 2f), 0) * transform.forward * viewRadius;

        for (int i = 0; i <= viewAngleStep; i++)
        {
            Vector3 v = Quaternion.Euler(0, (viewAngle / viewAngleStep) * i, 0) * forward_left;// ���ݵ�ǰ�Ƕȼ��㷽������
            Vector3 pos = transform.position + v;// ���������յ�

            // ���߼��
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
            OnHurtPlayer?.Invoke();
            Debug.Log("Hurt Player");
        }
    }

   

    public void StartAttack()
    {
        Debug.Log("��ʼ����");
        isAttacking = true;
        m_fsm.TransitionState("DashEnemy_LocateTarget");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 forward_left = Quaternion.Euler(0, -(viewAngle / 2f), 0) * transform.forward * viewRadius;

        for (int i = 0; i <= viewAngleStep; i++)
        {
            Vector3 v = Quaternion.Euler(0, (viewAngle / viewAngleStep) * i, 0) * forward_left;// ���ݵ�ǰ�Ƕȼ��㷽������
            Vector3 pos = transform.position + v;// ���������յ�

            Gizmos.DrawLine(transform.position, pos);
        }
    }
}
