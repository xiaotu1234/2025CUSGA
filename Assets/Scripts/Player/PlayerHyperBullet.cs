using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHyperBullet : PlayerBulletBase
{
    #region Bullet Settings 子弹设置
    //public float speed = 30f;
    public float lifeTime = 3f;
    public float globalFixedHeight = 2f; // 全局固定高度
    #endregion

    void Start()
    {
        Destroy(gameObject, lifeTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        // 忽略玩家碰撞
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject);
            if (other.gameObject.GetComponent<Enitity>() != null)
                other.gameObject.GetComponent<Enitity>().TakeDamage(damage);
        }

    }
}
