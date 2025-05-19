using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    #region Bullet Settings �ӵ�����
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float m_nextFireTime;
    private Vector3 m_fixedDirection; // �̶��������
    private bool m_initialized = false; // �Ƿ��ѳ�ʼ������
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
        // �״����ʱ��ʼ���̶�����
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

    // ���ɻ���firePoint.forward��ˮƽ������򣨽�X��Z�ᣩ
    Vector3 GetRandomHorizontalDirection()
    {
        Vector3 forward = firePoint.forward;
        forward.y = 0;
        forward = forward.normalized;

        if (forward == Vector3.zero)
            forward = Vector3.forward;

        // ������ȫ�����360��ˮƽ����
        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
        return rotation * forward;
    }

    // �ⲿ���ã��������ɹ̶��������
    public void ResetDirection()
    {
        m_initialized = false;
    }
}