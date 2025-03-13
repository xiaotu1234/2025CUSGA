using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public LayerMask targetLayer; // 射线检测的目标层（如地面、敌人）
    public float bulletForce = 20f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // 初始化时忽略子弹与玩家的碰撞
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Bullet"),
            LayerMask.NameToLayer("Player")
        );
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("111");
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, targetLayer))
        {
            Vector3 shootDirection = (hit.point - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
            
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootDirection * bulletForce;
            }
        }
    }
}