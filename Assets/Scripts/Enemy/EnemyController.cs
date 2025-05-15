using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float m_maxHP;
    protected float m_currentHP;
    // Start is called before the first frame update
    public Skill skillObject;
    public Color color;
    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
        if (color == null)
            Debug.LogError("���˵�color����Ϊnull");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        m_currentHP = Mathf.Max(m_currentHP - damage, 0);
        if (m_currentHP <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {

    }
}
