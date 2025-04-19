using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackBullet : MonoBehaviour
{
    #region Bullet Settings 子弹设置
    //public float speed = 30f;
    public float lifeTime = 3f;
    public float globalFixedHeight = 2f; // 全局固定高度
    public float detectionRadius = 5f; // 检测敌人的圆形范围半径
    private Rigidbody rb;
    private Transform targetEnemy; // 当前锁定的敌人
    private float speed;
    private PlayerController player;
    public int damage;
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
        // 如果没有锁定敌人，尝试检测附近敌人
        if (targetEnemy == null)
        {
            FindNearestEnemy();
        }
        // 如果锁定敌人，调整移动方向
        else
        {
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            rb.velocity = direction *  speed;
        }
    }
    // 检测最近的敌人
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

    void OnCollisionEnter(Collision collision)
    {
        // 忽略玩家碰撞
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // 示例：击中敌人后销毁敌人
            Destroy(gameObject); // 销毁子弹
        }
    }
}
