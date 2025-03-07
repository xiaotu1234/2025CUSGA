using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    [Header("Health Settings")]
    public int maxHealth = 5;
    public float invulnerabilityTime = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private int currentHealth;
    private bool isInvulnerable;

    private float lastXPosition;
    public Animator animator;
    private int direction = 0;//向前是0

    [Header("Parry Settings")]
    public float parryDuration = 0.2f;    // 弹反有效持续时间
    public float parryCooldown = 1f;      // 弹反冷却时间
    public LayerMask bulletLayer;         
    public GameObject parryEffect;        // 弹反特效

    private Animator anim;
    private bool canParry = true;
    private bool isParrying;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
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
        if (Input.GetMouseButtonDown(1) && canParry)
        {
            StartCoroutine(ParryAction());
        }
    }
    System.Collections.IEnumerator ParryAction()
    {
        // 进入弹反状态
        canParry = false;
        isParrying = true;
        gameObject.tag = "newTag";

        // 触发动画
        anim.SetTrigger("Parry");
        if (parryEffect != null)
            Instantiate(parryEffect, transform.position + Vector3.up, Quaternion.identity);

        // 弹反有效时间
        yield return new WaitForSeconds(parryDuration);

        // 结束弹反状态
        isParrying = false;
        gameObject.tag = "Player";

        // 冷却时间
        yield return new WaitForSeconds(parryCooldown - parryDuration);
        canParry = true;
    }

    void OnTriggerEnter(Collider other)
    {
       
        // 检测子弹碰撞
        if (isParrying && other.gameObject.CompareTag("Bullet"))
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
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        Debug.Log(controller.isGrounded);
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Debug.Log($"血: {currentHealth}");
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
    


if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityCooldown());
        }
    }

    IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
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
            if (direction==1)
            {
                animator.SetTrigger("turnface");
                direction = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isFace", false);
            if (direction == 0)
            {
                animator.SetTrigger("turnback");
                direction = 1;
            }
        }


    }
}