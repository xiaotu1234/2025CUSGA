using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerMove", menuName = "ScriptableObject/Player/PlayerMove", order = 0)]
public class PlayerMove : PlayerState
{
    #region Movement Settings 移动设置
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 1.3f;
    public float gravity = -100f;

    private CharacterController m_controller;
    private Vector3 m_velocity;
    private Transform m_transform;
    private PlayerController player;
    #endregion

    #region Timer 计时器
    //private float skillTimer = 0;
    #endregion

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;  // 在Unity Inspector中设置地面层级
    public float groundCheckDistance = 0.2f;  // 射线检测距离

    public override void OnAwake()
    {
        base.OnAwake();
        m_controller = m_Player.GetComponent<CharacterController>();
        m_transform = m_Player.transform;
    }

    public override void OnEnter()
    {
        player = PlayerManager.Instance.player;
    }

    public override void OnUpdate()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        moveAnimation();
        HandleRoll();
        HandleAbsorb();
        HandleSkill();
    }
    public override void OnExit()
    {

    }
    private void HandleAbsorb()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.stateMachine.TransitionState("PlayerAbsorb");
        }
    }
    private void HandleSkill()
    {   
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (player.skill == null)
            {
                return;
            }
            if (player.skillTimer<=0)
            {
                //执行对应的技能效果
                player.skillTimer = Time.time;
                player.skill.Initialize();
                player.skill.SkillEffect();
                player.skillTimer = player.skill.skillCooldown;
            }
        }
        player.skillTimer = Mathf.Max(player.skillTimer-Time.deltaTime,0);
        
    }
    private void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (player.lastRollTime + player.rollCooldown) < Time.time)
        {
            player.lastRollTime = Time.time;
            player.stateMachine.TransitionState("PlayerRoll");
        }
    }
    public void moveAnimation()
    {
        player.Flip();

        if (player.GetDir().z < 0) 
        {
            player.anim.SetBool("isFace", true);
            if (player.GetFaceDirection() == 1)
            {
                player.back.SetActive(false);
                player.face.SetActive(true);
                player.anim.SetTrigger("turnface");
                player.SetFaceDirection(0);
            }
        }
        else if (player.GetDir().z > 0)
        {
            player.anim.SetBool("isFace", false);
            if (player.GetFaceDirection() == 0)
            {
                player.face.SetActive(false);
                player.back.SetActive(true);
                player.anim.SetTrigger("turnback");
                player.SetFaceDirection(1);
            }
        }

    }

    //private void Flip()
    //{
    //    if (player.GetDir().x < 0 && player.isRight)
    //    {
    //        player.anim.gameObject.transform.Rotate(0, 180, 0, Space.Self);
    //        //player.back.transform.Rotate(0, 180, 0, Space.Self);
    //        player.isRight = false;
    //    }
    //    else if (player.GetDir().x > 0 && !player.isRight)
    //    {
    //        player.anim.gameObject.transform.Rotate(0, 180, 0, Space.Self);
    //        //player.back.transform.Rotate(0, 180, 0, Space.Self);
    //        player.isRight = true;
    //    }
    //}

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        player.dir = new Vector3(x, 0, z);
        //if (dir != Vector3.zero)
        //{
        //    m_transform.LookAt(m_transform.position + dir);
        //}

        m_controller.Move(player.dir * moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            m_velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
    private bool IsGrounded()
    {
        // 从玩家脚部向下发射射线
        Ray ray = new Ray(m_transform.position + Vector3.up * 0.1f, Vector3.down);
        bool hitGround = Physics.Raycast(ray, groundCheckDistance, groundLayer);
        return hitGround;
    }

    private void ApplyGravity()
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
}
