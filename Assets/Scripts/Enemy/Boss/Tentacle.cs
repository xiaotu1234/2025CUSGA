using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : Enitity
{
    private bool m_isInvulnerable = true;
    private bool m_isHurting = false;
    public GameObject centerPoint;
    public float stayTime;
    public float attackSpeed;
    public int damage;

    private PlayerController player;

    //策划加的动画
    private Animator animator;
    public Animator BossMain;

    public Sprite flickPicture;
    public Sprite normalPicture;

    private SpriteRenderer sr;
    private bool isFlashing = false; // 防止重复闪烁

    #region 隐藏无效变量
    //隐藏无效变量
    [System.NonSerialized]
    [HideInInspector]
    public new float moveSpeed;
    #endregion
    void Start()
    {
        m_currentHealth = maxHealth;
        player = PlayerManager.Instance.player;
        animator = GetComponentInParent<Animator>();
        sr = this.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>();
        sr.enabled = false;
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
        AudioManager.Instance.PlaySFX(7);
        if (m_currentHealth - _damage > 0)
        {
            m_currentHealth -= _damage;
            if (!isFlashing) // 如果没有在闪烁，才执行闪烁
            {
                StartCoroutine(SpriteRFlicker());
            }
        }
        else
            Die();
    }
    IEnumerator SpriteRFlicker()
    {
        isFlashing = true;
        sr.sprite = flickPicture;
        yield return new WaitForSeconds(.1f);
        sr.sprite = normalPicture;
        isFlashing = false;
    }
    public override void Die()
    {
        BossManager.Instance.boss_1.GetComponent<Boss_1_Controller>().RemoveTentacle(this);
        Destroy(this.transform.parent.gameObject);
        BossMain.SetBool("attackChushou", false);
    }
    public void Attack()
    {
        StartCoroutine(RotateAttack());
    }
    private IEnumerator RotateAttack()
    {
        sr.enabled = true;
        animator.SetTrigger("Xiaza");
        BossMain.SetBool("attackChushou", true);
        yield return new WaitForSeconds(0.8f);
        m_isHurting =  true;
        float duration = 0.7f / attackSpeed; // 计算所需时间（例如：attackSpeed=2 → 0.5秒完成）
        float elapsedTime = 0f;
        Quaternion startRotation = centerPoint.transform.rotation;
        // 计算玩家与触手在XZ平面上的方向
        Vector3 directionToPlayer = player.transform.position - centerPoint.transform.position;
        directionToPlayer.y = 0; // 只在XZ平面上计算

        // 计算Y轴应该旋转的角度（面向玩家）
        float targetYRotation = Quaternion.LookRotation(directionToPlayer).eulerAngles.y;
        // 设置目标旋转：X轴-90度，Y轴面向玩家，Z轴保持原样
        //Quaternion targetRotation = Quaternion.Euler(90f, targetYRotation, 0);
        Quaternion targetRotation = Quaternion.Euler(-90f, 0, 0);

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
        m_isInvulnerable = false;

        yield return new WaitForSeconds(stayTime);
        //保证在不可攻击时跟踪子弹无法锁定
        this.gameObject.tag = "Untagged";
        m_isInvulnerable = true;

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
        BossMain.SetBool("attackChushou", false);
        sr.enabled = false;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!m_isHurting)
    //        return;
    //    if (collision.gameObject.GetComponent<PlayerController>() != null)
    //        collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
    //}
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<PlayerController>() != null)
        {
            if (!m_isHurting)
                return;
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        if (other.GetComponent<PlayerBulletBase>() != null)
        {  
            this.TakeDamage(other.GetComponent<PlayerBulletBase>().damage);
            if (other.TryGetComponent<Ball>(out Ball hyperbullet))
            {
                hyperbullet.ReturnBall();
            }else
            {
                Destroy(other);
            }
        }
    }
}
