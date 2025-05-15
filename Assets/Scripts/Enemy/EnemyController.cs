using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float m_maxHP;
    protected float m_currentHP;
    // Start is called before the first frame update
    public Skill skillObject;

    public Sprite flickPicture;
    public Sprite normalPicture;

    private SpriteRenderer sr;
    private bool isFlashing = false; // 防止重复闪烁

    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        m_currentHP = Mathf.Max(m_currentHP - damage, 0);
        if (!isFlashing) // 如果没有在闪烁，才执行闪烁
        {
            StartCoroutine(SpriteRFlicker());
        }
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
}
