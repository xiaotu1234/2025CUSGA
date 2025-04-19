using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : Enitity
{
    private bool m_isInvulnerable = false;
    private bool m_isHurting = false;
    public GameObject centerPoint;
    public float stayTime;
    public float attackSpeed;
    public int damage;



    #region 隐藏无效变量
    //隐藏无效变量
    [System.NonSerialized]
    [HideInInspector]
    public new float moveSpeed;
    #endregion
    void Start()
    {
        m_currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Attack();
        }
    }
    public override void TakeDamage(int _damage)
    {
        if (m_isInvulnerable)
            return;
        if (m_currentHealth - _damage > 0)
        {
            m_currentHealth -= _damage;
        }

        else
            Die();
    }
    public override void Die()
    {
        BossManager.Instance.boss_1.tentacles.Remove(this);
        Destroy(this.gameObject);
    }
    public void Attack()
    {
        StartCoroutine(RotateAttack());
    }
    private IEnumerator RotateAttack()
    {
        m_isHurting =  true;
        float duration = 1.0f / attackSpeed; // 计算所需时间（例如：attackSpeed=2 → 0.5秒完成）
        float elapsedTime = 0f;
        Quaternion startRotation = centerPoint.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(-90f, 0, 0); // 目标角度

        while (elapsedTime < duration)
        {
            // 插值旋转
            centerPoint.transform.rotation = Quaternion.Lerp(
                startRotation,
                targetRotation,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终角度精确
        centerPoint.transform.rotation = targetRotation;

        //保证在不可攻击时跟踪子弹无法锁定
        this.gameObject.tag = "Enemy";

        yield return new WaitForSeconds(.1f);
        m_isHurting = false;

        yield return new WaitForSeconds(stayTime);
        //保证在不可攻击时跟踪子弹无法锁定
        this.gameObject.tag = "Untagged";

        // 旋转回初始角度（恢复状态）
        elapsedTime = 0f;
        startRotation = centerPoint.transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0, 0); // 初始角度

        while (elapsedTime < duration)
        {
            centerPoint.transform.rotation = Quaternion.Lerp(
                startRotation,
                targetRotation,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        centerPoint.transform.rotation = targetRotation; // 确保精确归位

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!m_isHurting)
            return;
        if (other.GetComponent<PlayerController>() != null)
            other.GetComponent<PlayerController>().TakeDamage(damage);
    }
}
