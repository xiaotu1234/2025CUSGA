using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Movement Settings �ƶ�����
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = -20f;
    private CharacterController m_controller;
    private Vector3 m_velocity;
    #endregion

    #region Health Settings Ѫ������
    [Header("Health Settings")]
    public int maxHealth = 5;
    public float invulnerabilityTime = 1f;
    private int m_currentHealth;
    #endregion 

    #region AnimationSettings ��������
    public Animator animator;
    private int m_direction = 0;//��ǰ��0
    #endregion

    #region Parry Settings ��������
    [Header("Parry Settings")]
    public float parryDuration = 0.2f;    // ������Ч����ʱ��
    public float parryCooldown = 1f;      // ������ȴʱ��
    public LayerMask bulletLayer;         
    public GameObject parryEffect;        // ������Ч

    private bool m_canParry = true;
    private bool m_isParrying;
    #endregion 

    private bool m_isInvulnerable; //�޵�״̬

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
        // ���뵯��״̬
        m_canParry = false;
        m_isParrying = true;
        gameObject.tag = "newTag";

        // ��������
        animator.SetTrigger("Parry");
        if (parryEffect != null)
            Instantiate(parryEffect, transform.position + Vector3.up, Quaternion.identity);

        // ������Чʱ��
        yield return new WaitForSeconds(parryDuration);

        // ��������״̬
        m_isParrying = false;
        gameObject.tag = "Player";

        // ��ȴʱ��
        yield return new WaitForSeconds(parryCooldown - parryDuration);
        m_canParry = true;
    }

    void OnTriggerEnter(Collider other)
    {
       
        // ����ӵ���ײ
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
        Debug.Log($"Ѫ: {m_currentHealth}");
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
        // ��Ӹ������Ϸ�����߼�
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