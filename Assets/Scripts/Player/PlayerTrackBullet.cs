using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackBullet : PlayerBulletBase
{
    #region Bullet Settings �ӵ�����
    //public float speed = 30f;
    public float lifeTime = 3f;
    public float globalFixedHeight = 2f; // ȫ�̶ֹ��߶�
    public float detectionRadius = 5f; // �����˵�Բ�η�Χ�뾶
    private Rigidbody rb;
    private Transform targetEnemy; // ��ǰ�����ĵ���
    private float speed;
    private PlayerController player;
    #endregion
    void Start()
    {
        player = PlayerManager.Instance.player;
        speed = player.GetComponent<PlayerShooting>().bulletForce;
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        // ���û���������ˣ����Լ�⸽������
        if (targetEnemy == null)
        {
            FindNearestEnemy();
        }
        // ����������ˣ������ƶ�����
        else
        {
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            rb.velocity = direction *  speed;
        }
    }
    // �������ĵ���
    private void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, detectionRadius);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            targetEnemy = closestEnemy;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        shoot(other);
    }
}
