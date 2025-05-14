using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : Enitity
{
    private bool m_isInvulnerable = false;
    private bool m_isHurting = false;
    public GameObject centerPoint;
    public float stayTime;
    public float attackSpeed;
    public int damage;

    private PlayerController player;

    //�߻��ӵĶ���
    private Animator animator;

    #region ������Ч����
    //������Ч����
    [System.NonSerialized]
    [HideInInspector]
    public new float moveSpeed;
    #endregion
    void Start()
    {
        m_currentHealth = maxHealth;
        player = PlayerManager.Instance.player;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Attack();
        }
    }
    public override void TakeDamage(int _damage)
    {
        if (m_isInvulnerable)
            return;
        if (m_currentHealth - _damage > 0)
        {
            m_currentHealth -= _damage;
        }

        else
            Die();
    }
    public override void Die()
    {
        BossManager.Instance.boss_1.GetComponent<Boss_1_Controller>().tentacles.Remove(this);
        Destroy(this.gameObject);
    }
    public void Attack()
    {
        StartCoroutine(RotateAttack());
    }
    private IEnumerator RotateAttack()
    {
        animator.SetTrigger("Xiaza");
        yield return new WaitForSeconds(0.8f);
        m_isHurting =  true;
        float duration = 1.0f / attackSpeed; // ��������ʱ�䣨���磺attackSpeed=2 �� 0.5����ɣ�
        float elapsedTime = 0f;
        Quaternion startRotation = centerPoint.transform.rotation;
        // ��������봥����XZƽ���ϵķ���
        Vector3 directionToPlayer = player.transform.position - centerPoint.transform.position;
        directionToPlayer.y = 0; // ֻ��XZƽ���ϼ���

        // ����Y��Ӧ����ת�ĽǶȣ�������ң�
        float targetYRotation = Quaternion.LookRotation(directionToPlayer).eulerAngles.y;
        // ����Ŀ����ת��X��-90�ȣ�Y��������ң�Z�ᱣ��ԭ��
        Quaternion targetRotation = Quaternion.Euler(90f, targetYRotation, 0);

        while (elapsedTime < duration)
        {
            // ��ֵ��ת
            centerPoint.transform.rotation = Quaternion.Lerp(
                startRotation,
                targetRotation,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ�����սǶȾ�ȷ
        centerPoint.transform.rotation = targetRotation;

        //��֤�ڲ��ɹ���ʱ�����ӵ��޷�����
        this.gameObject.tag = "Enemy";

        yield return new WaitForSeconds(.1f);
        m_isHurting = false;

        yield return new WaitForSeconds(stayTime);
        //��֤�ڲ��ɹ���ʱ�����ӵ��޷�����
        this.gameObject.tag = "Untagged";

        // ��ת�س�ʼ�Ƕȣ��ָ�״̬��
        elapsedTime = 0f;
        startRotation = centerPoint.transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0, 0); // ��ʼ�Ƕ�

        while (elapsedTime < duration)
        {
            centerPoint.transform.rotation = Quaternion.Lerp(
                startRotation,
                targetRotation,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        centerPoint.transform.rotation = targetRotation; // ȷ����ȷ��λ

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!m_isHurting)
    //        return;
    //    if (collision.gameObject.GetComponent<PlayerController>() != null)
    //        collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
    //}
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<PlayerController>() != null)
        {
            if (!m_isHurting)
                return;
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        if (other.GetComponent<PlayerBulletBase>() != null)
        {  
            this.TakeDamage(other.GetComponent<PlayerBulletBase>().damage);
            if (other.GetComponent<PlayerHyperBullet>() == null) 
                Destroy(other.gameObject);
        }
    }
}
