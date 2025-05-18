using UnityEngine;
using System.Collections;
using System;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : Enitity
{
    #region 事件
    public event Action<Color> OnColorChanged;
    #endregion



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

    #region"Zuma"
    [Header("超级球设定")]
    [HideInInspector] public BallProvider playerBallProvider;
    [SerializeField, Tooltip("超级球预制体")] 
    private GameObject _ballPrefab;
    [SerializeField, Tooltip("超级球可以存在的最大数量")] 
    private int _ballCount = 5;
    #endregion

    private bool m_isInvulnerable; //�޵�״̬
    private float m_lastXPosition;

    //���face��backͼ��
    public GameObject face;
    public GameObject back;
    public GameObject firePoint;
    public bool isRight = true;
    private SpriteRenderer _faceSpritRenderer;
    private SpriteRenderer _backSpritRenderer;
    private SpriteRenderer _faceColorSpritRenderer;

    #region Skill Setting 技能设置
    public Skill skill;
    [HideInInspector] public float skillTimer;
    #endregion

    #region HealHealth Setting 血量恢复设置 
    public float healTime;
    private float healTimer;
    public float healSpeed;
    private float healSpeedTimer;
    #endregion

    #region ������ȴ�������
    public float rollCooldown = 2;
    [HideInInspector] public float lastRollTime;
    #endregion

    private Vector3 checkpointPosition;
    [SerializeField] private Color _color;

    public Vector3 reburnPosition;

    [HideInInspector]public Transform flipAxle;

    private void Awake()
    {
        playerBallProvider = new BallProvider(_ballPrefab, _ballCount);
        _faceColorSpritRenderer = face.transform.Find("player_face_color").GetComponent<SpriteRenderer>();
        _faceSpritRenderer = face.GetComponent<SpriteRenderer>();
        _backSpritRenderer = back.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        stateMachine = GetComponent<StateMachine>();
        back.SetActive(false);
        lastRollTime = -rollCooldown;
        stateMachine.TransitionState("PlayerMove");
        healTimer = -healTime;
        healSpeedTimer = 0;
        flipAxle = transform.Find("FlipAxle");
        skillTimer = 0;
        playerBallProvider.CreatePoolBall();
    }

    void Update()
    {
        HealHealth();

        HandleParryInput();
    }

    private void OnDestroy()
    {
        playerBallProvider.CleanupPool();
    }
    

    void OnTriggerEnter(Collider other)
    {

        // ����ӵ���ײ
        if (m_isParrying && other.gameObject.CompareTag("Bullet_Enemy"))
        {
            ReflectBullet(other.gameObject);

        }
    }


    public int GetCurrentHealth()
    {
        return m_currentHealth;
    }
    private void HealHealth()
    {
        if (m_currentHealth < maxHealth && healTime + healTimer < Time.time)
        {
            healSpeedTimer += Time.deltaTime;
            if(healSpeedTimer>=healSpeed)
            {
                healSpeedTimer = 0;
                m_currentHealth++;
            }
        }
    }

    public void Flip()
    {
        if (GetDir().x < 0 && isRight||GetDir().x > 0 && !isRight)
        {
            transform.RotateAround(flipAxle.position, flipAxle.up, 180f);
            isRight = !isRight;
            //player.anim.gameObject.transform.Rotate(0, 180, 0, Space.Self);
            //player.back.transform.Rotate(0, 180, 0, Space.Self);
        }
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


    public override void TakeDamage(int damage)
    {
        if (m_isInvulnerable) return;

        m_currentHealth = Mathf.Max(m_currentHealth - damage, 0);
        healTimer = Time.time;
        Debug.Log($"Ѫ: {m_currentHealth}");
        //anim.SetTrigger("hurt");
        AudioManager.Instance.PlaySFX(2);

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
        Color originColor = _faceColorSpritRenderer.color;
        for(int i = 0;i<invulnerabilityTime/.2f;i++)
        {
            _faceSpritRenderer.color = Color.red;
            _backSpritRenderer.color = Color.red;
            _faceColorSpritRenderer.color = Color.red; 
            yield return new WaitForSeconds(.1f);
            _faceSpritRenderer.color = Color.white;
            _backSpritRenderer.color = Color.white;
            _faceColorSpritRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
        _faceColorSpritRenderer.color = originColor;
        
    }

    IEnumerator InvulnerabilityCooldown()
    {
        m_isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        m_isInvulnerable = false;
    }

    public override void Die()
    {
        Debug.Log("Player Died!");
        stateMachine.TransitionState("PlayerDie");
        
        // ���Ӹ������Ϸ�����߼�
    }
    public void Reburn()
    {
        // 重置玩家位置到检查点
        this.gameObject.transform.position = reburnPosition;

        // 重置生命值
        m_currentHealth = maxHealth;
    }

    public CharacterController GetController() { return m_controller; }
    public int GetFaceDirection()
    {
        return m_direction;
    }
    public void SetFaceDirection(int dir)
    {
        m_direction = dir;
    }

    public void SetColor(Color color)
    {
        _color = color;
        OnColorChanged?.Invoke(_color);
    }

    public Color GetColor() { return _color; }
    public void SetCurrentHealth(int health)
    {
        m_currentHealth = health;
    }

    public void AddCurrentHealth(int health)
    {
        AudioManager.Instance.PlaySFX(4);
        m_currentHealth = Mathf.Clamp(m_currentHealth + health, 0, maxHealth);
        Debug.Log($"为玩家恢复血量，恢复值为{health}, 最大血量：{maxHealth}，当前血量：{m_currentHealth}");
    }
    
    

}