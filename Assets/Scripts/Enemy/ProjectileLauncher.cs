using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    #region Bullet Settings ×Óµ¯ÉèÖÃ
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float m_nextFireTime;
    #endregion

    void Update()
    {
        if (Time.time >= m_nextFireTime)
        {
            Shoot();
            m_nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
        Destroy(bullet, 5f);
    }
}

