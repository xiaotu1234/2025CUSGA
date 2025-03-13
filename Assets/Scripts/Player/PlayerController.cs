using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Movement Settings 移动设置
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = -20f;
    private CharacterController m_controller;
    private Vector3 m_velocity;
    #endregion

    #region Health Settings 血量设置
    [Header("Health Settings")]
    public int maxHealth = 5;
    public float invulnerabilityTime = 1f;
    private int m_currentHealth;
    #endregion 

    #region AnimationSettings 动画设置
    public Animator animator;
    private int m_direction = 0;//向前是0
    #endregion

    #region Parry Settings 反弹设置
    [Header("Parry Settings")]
    public float parryDuration = 0.2f;    // 弹反有效持续时间
    public float parryCooldown = 1f;      // 弹反冷却时间
    public LayerMask bulletLayer;         
    public GameObject parryEffect;        // 弹反特效

    private bool m_canParry = true;
    private bool m_isParrying;
    #endregion 

    private bool m_isInvulnerable; //无敌状态

    private float m_lastXPosition;


    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_currentHealth = maxHealth;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        moveAnimation();
        HandleParryInput();
    }
    void HandleParryInput()
    {
        if (Input.GetMouseButtonDown(1) && m_canParry)
        {
            StartCoroutine(ParryAction());
        }
    }
    System.Collections.IEnumerator ParryAction()
    {
        // 进入弹反状态
        m_canParry = false;
        m_isParrying = true;
        gameObject.tag = "newTag";

        // 触发动画
        animator.SetTrigger("Parry");
        if (parryEffect != null)
            Instantiate(parryEffect, transform.position + Vector3.up, Quaternion.identity);

        // 弹反有效时间
        yield return new WaitForSeconds(parryDuration);

        // 结束弹反状态
        m_isParrying = false;
        gameObject.tag = "Player";

        // 冷却时间
        yield return new WaitForSeconds(parryCooldown - parryDuration);
        m_canParry = true;
    }

    void OnTriggerEnter(Collider other)
    {
       
        // 检测子弹碰撞
        if (m_isParrying && other.gameObject.CompareTag("Bullet_Enemy"))
        {
            ReflectBullet(other.gameObject);
           
        }
    }

    void ReflectBullet(GameObject bullet)
    {
        if (bullet == null) return;

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            // 反转子弹的朝向
            bullet.transform.forward = -bullet.transform.forward;
            // 反转刚体的速度方向
            bulletRb.velocity = -bulletRb.velocity;
        }

        // 修改子弹属性
        // bullet.speed *= 1.2f;  // 增加反弹速度
        // bullet.damage *= 2;    // 增加反弹伤害
        // bullet.ownerTag = "Enemy"; // 修改伤害目标

        // 添加视觉效果
        bullet.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        m_controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleJump()
    {

        if ( Input.GetKeyDown(KeyCode.Space))
        {
            m_velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        if (!m_controller.isGrounded)
        {
            m_velocity.y += gravity * Time.deltaTime;
        }
        else if (m_velocity.y < 0)
        {
            m_velocity.y = -2f;
        }

        m_controller.Move(m_velocity * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (m_isInvulnerable) return;

        m_currentHealth = Mathf.Max(m_currentHealth - damage, 0);
        Debug.Log($"血: {m_currentHealth}");
        animator.SetTrigger("hurt");
/*        Transform[] allChildren = GetComponentsInChildren<Transform>();

        // 遍历所有子物体
        foreach (Transform child in allChildren)
        {
            // 尝试获取子物体的 SpriteRenderer 组件
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Debug.Log("222");
                // 如果存在 SpriteRenderer 组件，则将其颜色设置为红色
                spriteRenderer.color = Color.red;
            }
        }*/
    


        if (m_currentHealth <= 0)
            Die();         
        else                                   
            StartCoroutine(InvulnerabilityCooldown());               
    }

    IEnumerator InvulnerabilityCooldown()
    {
        m_isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        m_isInvulnerable = false;
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // 添加复活或游戏结束逻辑
    }
    public void moveAnimation()
    {
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("isFace", true);
            if (m_direction==1)
            {
                animator.SetTrigger("turnface");
                m_direction = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isFace", false);
            if (m_direction == 0)
            {
                animator.SetTrigger("turnback");
                m_direction = 1;
            }
        }


    }
}