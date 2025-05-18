using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashEnemyController : EnemyController
{
    #region ��̵��˻�������
    [Header("��������")]
    [SerializeField] private float damage;

    [Header("��Ұ����")]
    public float viewRadius = 5f;//��Ұ����
    public int viewAngleStep = 20;//�����ܶ�
    [Range(0, 360)]
    public float viewAngle = 90f;//��Ұ�Ƕ�
    #endregion

    public bool isAttacking = false;
    public event Action OnDashEnemyDead;
    public event Action OnHurtPlayer;
    public Color color;
    private bool isDead = false;

    //�߻��ӵı���
    public GameObject colorimg;
    public Animator dieanimator;


    protected override void Start()
    {
        base.Start();
        if (color == null)
            Debug.LogError("��̵��˵���ɫδ��ֵ");
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
