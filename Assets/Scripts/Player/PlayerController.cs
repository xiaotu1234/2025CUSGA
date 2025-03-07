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
    private int direction = 0;//��ǰ��0

    [Header("Parry Settings")]
    public float parryDuration = 0.2f;    // ������Ч����ʱ��
    public float parryCooldown = 1f;      // ������ȴʱ��
    public LayerMask bulletLayer;         
    public GameObject parryEffect;        // ������Ч

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
        // ���뵯��״̬
        canParry = false;
        isParrying = true;
        gameObject.tag = "newTag";

        // ��������
        anim.SetTrigger("Parry");
        if (parryEffect != null)
            Instantiate(parryEffect, transform.position + Vector3.up, Quaternion.identity);

        // ������Чʱ��
        yield return new WaitForSeconds(parryDuration);

        // ��������״̬
        isParrying = false;
        gameObject.tag = "Player";

        // ��ȴʱ��
        yield return new WaitForSeconds(parryCooldown - parryDuration);
        canParry = true;
    }

    void OnTriggerEnter(Collider other)
    {
       
        // ����ӵ���ײ
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
            // ��ת�ӵ��ĳ���
            bullet.transform.forward = -bullet.transform.forward;
            // ��ת������ٶȷ���
            bulletRb.velocity = -bulletRb.velocity;
        }

        // �޸��ӵ�����
        // bullet.speed *= 1.2f;  // ���ӷ����ٶ�
        // bullet.damage *= 2;    // ���ӷ����˺�
        // bullet.ownerTag = "Enemy"; // �޸��˺�Ŀ��

        // ����Ӿ�Ч��
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
        Debug.Log($"Ѫ: {currentHealth}");
        animator.SetTrigger("hurt");
/*        Transform[] allChildren = GetComponentsInChildren<Transform>();

        // ��������������
        foreach (Transform child in allChildren)
        {
            // ���Ի�ȡ������� SpriteRenderer ���
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Debug.Log("222");
                // ������� SpriteRenderer �����������ɫ����Ϊ��ɫ
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
        // ��Ӹ������Ϸ�����߼�
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