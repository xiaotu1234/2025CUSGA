using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float bulletForce = 20f;
    private float nextFire;
    public bool isAttacking = true;

    void Start()
    {
        // 初始化时忽略子弹与玩家的碰撞
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Bullet_Player"),
            LayerMask.NameToLayer("Player")
        );
        Debug.Log(firePoint.transform.position);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isAttacking = !isAttacking;
        }
        if(isAttacking)
            Shoot(bulletForce, fireRate );
    }

    void Shoot(float bulletSpeed, float fireRate)
    {
        if (Time.time > nextFire)//让子弹发射有间隔
        {
            nextFire = Time.time + fireRate;//Time.time表示从游戏开发到现在的时间，会随着游戏的暂停而停止计算。
            GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = firePoint.transform.forward * bulletSpeed;
        }


    }
}