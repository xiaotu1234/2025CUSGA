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

    private void OnTriggerEnter(Collider other)
    {
        shoot(other);
    }
}
