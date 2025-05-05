using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHyperBullet : PlayerBulletBase
{
    #region Bullet Settings �ӵ�����
    //public float speed = 30f;
    public float lifeTime = 3f;
    public float globalFixedHeight = 2f; // ȫ�̶ֹ��߶�
    #endregion

    void Start()
    {
        Destroy(gameObject, lifeTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        // ���������ײ
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
