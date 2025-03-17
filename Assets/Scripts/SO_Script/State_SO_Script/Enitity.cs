using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//敌人与玩家基类
public class Enitity : MonoBehaviour
{
    //动画相关
    public Animator anim;
    public StateController stateMachine;

    #region 移动相关
    public float moveSpeed;
    public float jumpForce;
    public float gravity;
    private Vector3 dir;
    private Rigidbody rb;
    private Vector3 m_velocity;
    private Transform m_transform;
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
}
