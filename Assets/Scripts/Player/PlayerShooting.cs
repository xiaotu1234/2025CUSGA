using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public LayerMask targetLayer; // 射线检测的目标层（如地面、敌人）
    public float bulletForce = 20f;
    private Camera mainCamera;
    private RaycastHit hit;

    void Start()
    {
        mainCamera = Camera.main;
        // 初始化时忽略子弹与玩家的碰撞
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Bullet_Player"),
            LayerMask.NameToLayer("Player")
        );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        

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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(firePoint.position, (hit.point - firePoint.position).normalized);
    }

}