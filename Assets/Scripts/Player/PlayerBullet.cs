using UnityEngine;
public class PlayerBullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 3f;
public class BulletController : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 3f;
    public float globalFixedHeight = 2f; // 全局固定高度

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 移动并强制固定高度
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.position = new Vector3(
            transform.position.x,
            globalFixedHeight,
            transform.position.z
        );
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
}