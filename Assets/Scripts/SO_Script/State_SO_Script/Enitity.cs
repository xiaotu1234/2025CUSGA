using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//��������һ���
public class Enitity : MonoBehaviour
{
    //�������
    public Animator anim;
    public StateController stateMachine;

    #region �ƶ����
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
