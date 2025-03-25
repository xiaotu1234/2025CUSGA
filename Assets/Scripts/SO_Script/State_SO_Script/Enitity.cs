using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//敌人与玩家基类
public class Enitity : MonoBehaviour
{
    //动画相关
    public Animator anim;
    [HideInInspector] public StateMachine stateMachine;

    [Header("Health Settings")]
    public int maxHealth = 5;
    protected int m_currentHealth;

    #region 移动相关
    public float moveSpeed;
    public float jumpForce;
    public float gravity;
    protected Vector3 dir;
    protected Rigidbody rb;
    protected Vector3 m_velocity;
    protected Transform m_transform;
    #endregion
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void TakeDamage()
    {

    }
    public Vector3 GetDir()
    {
        return dir;
    }
}
