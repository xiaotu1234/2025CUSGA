using UnityEngine;
public class PlayerBullet : PlayerBulletBase
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

    

    void OnCollisionEnter(Collision collision)
    {
        // 忽略玩家碰撞
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject); // 示例：击中敌人后销毁敌人
            if (collision.gameObject.GetComponent<Enitity>() != null) 
                collision.gameObject.GetComponent<Enitity>().TakeDamage(damage);
            Destroy(gameObject); // 销毁子弹
        }
            

    }
}
