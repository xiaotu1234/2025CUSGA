using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float m_maxHP;
    protected float m_currentHP;
    // Start is called before the first frame update
    public Skill skillObject;
    protected StateMachine m_fsm;
    public Sprite flickPicture;
    public Sprite normalPicture;

    private SpriteRenderer sr;
    private bool isFlashing = false; // ��ֹ�ظ���˸

    protected virtual void Awake()
    {
        m_currentHP = m_maxHP;
        m_fsm = GetComponent<EnemyStasteMachine>();
    }

    protected virtual void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
        sr = GetComponentInChildren<SpriteRenderer>();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        m_currentHP = Mathf.Max(m_currentHP - damage, 0);
        AudioManager.Instance.PlaySFX(6);
        if (m_currentHP > 0)
        {
            if (!isFlashing) // ���û������˸����ִ����˸
            {
                StartCoroutine(SpriteRFlicker());
            }
        }

        Debug.Log($"伤害敌人，敌人当前血量：{m_currentHP}");
        if (m_currentHP <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {

    }
    IEnumerator SpriteRFlicker()
    {
        isFlashing = true;
        sr.sprite = flickPicture;
        yield return new WaitForSeconds(.1f);
        sr.sprite = normalPicture;
        isFlashing = false;
    }

    public EnemyStasteMachine GetFSM()
    {
        return (EnemyStasteMachine)m_fsm;
    }
}
