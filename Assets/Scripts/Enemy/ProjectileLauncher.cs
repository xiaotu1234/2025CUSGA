using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    #region Bullet Settings 子弹设置
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float m_nextFireTime;
    private Vector3 m_fixedDirection; // 固定随机方向
    private bool m_initialized = false; // 是否已初始化方向
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
        // 首次射击时初始化固定方向
        if (!m_initialized)
        {
            m_fixedDirection = GetRandomHorizontalDirection();
            m_initialized = true;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(m_fixedDirection));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = m_fixedDirection * bulletSpeed;
        Destroy(bullet, 5f);
    }

    // 生成基于firePoint.forward的水平随机方向（仅X和Z轴）
    Vector3 GetRandomHorizontalDirection()
    {
        Vector3 forward = firePoint.forward;
        forward.y = 0;
        forward = forward.normalized;

        if (forward == Vector3.zero)
            forward = Vector3.forward;

        // 生成完全随机的360度水平方向
        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
        return rotation * forward;
    }

    // 外部调用：重新生成固定随机方向
    public void ResetDirection()
    {
        m_initialized = false;
    }
}