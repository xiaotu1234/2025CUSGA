using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject trackBulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float bulletForce = 20f;
    private float nextFire;
    public bool isAttacking = false;
    public Vector3 shootDirection=Vector3.right;

    public bool normalShoot;

    [SerializeField] private LayerMask ground;
    

    void Start()
    {
        // 初始化时忽略子弹与玩家的碰撞
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Bullet_Player"),
            LayerMask.NameToLayer("Player")
        );
        // 初始化时忽略子弹与子弹的碰撞
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Bullet_Player"),
            LayerMask.NameToLayer("Bullet_Player")
        );
        normalShoot = true;
    }

    void Update()
    {
        NormalShootDirection();
        if (isAttacking)
        { 
            if (normalShoot)
            {
                Shoot(bulletForce, fireRate, bulletPrefab);
            }

            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    isAttacking = !isAttacking;
            //}
            else
            {
                //添加追踪子弹技能的逻辑
                Shoot(bulletForce, fireRate, trackBulletPrefab);
            }
        }

    }

    private void NormalShootDirection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            shootDirection = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            shootDirection = -Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            shootDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            shootDirection = Vector3.right;
        }
        if (Input.GetKey(KeyCode.UpArrow) ||
           Input.GetKey(KeyCode.DownArrow) ||
           Input.GetKey(KeyCode.LeftArrow) ||
           Input.GetKey(KeyCode.RightArrow))
            isAttacking = true;
        else
            isAttacking = false;
    }


    void Shoot(float bulletSpeed, float fireRate, GameObject bulletPre)
    {
        if (Time.time > nextFire)//让子弹发射有间隔
        {
            nextFire = Time.time + fireRate;//Time.time表示从游戏开发到现在的时间，会随着游戏的暂停而停止计算。
            GameObject bullet = Instantiate(bulletPre, firePoint.transform.position, firePoint.transform.rotation);
            #region
            //Vector3 mouseScreenPos = Input.mousePosition;

            //Vector3 shootDirection;
            ////计算射击方向（从发射点到鼠标点击位置）
            //if (Mathf.Abs(mouseScreenPos.x- Screen.width/2)> Mathf.Abs(mouseScreenPos.y - Screen.height / 2))
            //{
            //    if (mouseScreenPos.x - Screen.width / 2 > 0)
            //        shootDirection = Vector3.right;
            //    else
            //        shootDirection = -Vector3.right;
            //}
            //else
            //{
            //    if (mouseScreenPos.y - Screen.height / 2 > 0)
            //        shootDirection = Vector3.forward;
            //    else
            //        shootDirection = -Vector3.forward;
            //}
            #endregion
            bullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletSpeed;
        }   
    }
}