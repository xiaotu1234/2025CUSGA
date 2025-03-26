using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : Enitity
{
    #region Movement Settings �ƶ�����
    [Header("Movement Settings")]
    private CharacterController m_controller;
    #endregion

    public float invulnerabilityTime = 1f;

    #region AnimationSettings ��������
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

    //���face��backͼ��
    public GameObject face;
    public GameObject back;

    #region State ״̬������
    [HideInInspector] public PlayerRoll rollState;
    [HideInInspector] public PlayerMove moveState;
    [HideInInspector] public PlayerIdle idleState;
    #endregion

    #region ������ȴ�������
    [SerializeField] private float m_rollCooldown = 2;
    private float m_lastRollTime;
    #endregion

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachine>();
        rollState = new PlayerRoll();
        moveState = new PlayerMove();
        idleState = new PlayerIdle();
        stateMachine.currentState = idleState;
        back.SetActive(false);
        m_lastRollTime = -m_rollCooldown;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        //moveAnimation();
        HandleParryInput();
        HandleRoll();
    }
    void HandleRoll()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)&&(m_lastRollTime+m_rollCooldown)<Time.time)
        {
            m_lastRollTime= Time.time;
            stateMachine.TransitionState(rollState);
        }
    }
    public int GetCurrentHealth()
    {
        return m_currentHealth;
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
        anim.SetTrigger("Parry");
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

        // �����Ӿ�Ч��
        bullet.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        dir = new Vector3(x, 0, z);
        //if (dir != Vector3.zero)
        //{
        //    transform.LookAt(transform.position + new Vector3(0,0,z));
        //}
        
        m_controller.Move(dir * moveSpeed * Time.deltaTime);
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
        anim.SetTrigger("hurt");


        if (m_currentHealth <= 0)
            Die();
        else
        {
            StartCoroutine(SpriteRFlicker());
            StartCoroutine(InvulnerabilityCooldown());
        }
    }
    IEnumerator SpriteRFlicker()
    {
        for(int i = 0;i<invulnerabilityTime/.2f;i++)
        {
            face.GetComponent<SpriteRenderer>().color = Color.red;
            back.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(.1f);
            face.GetComponent<SpriteRenderer>().color = Color.white;
            back.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
        
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
        // ���Ӹ������Ϸ�����߼�
    }
    //public void moveAnimation()
    //{
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("isFace", true);
            if (m_direction==1)
            {
                back.SetActive(false);
                face.SetActive(true);
                anim.SetTrigger("turnface");
                m_direction = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("isFace", false);
            if (m_direction == 0)
            {
                back.SetActive(true);
                face.SetActive(false);
                anim.SetTrigger("turnback");
                m_direction = 1;
            }
        }


    }

    public CharacterController GetController() { return m_controller; }
    public int GetFaceDirection()
    {
        return m_direction;
    }
}