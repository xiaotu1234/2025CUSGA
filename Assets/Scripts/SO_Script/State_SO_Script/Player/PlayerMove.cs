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
    private float skillTimer = 0;
    #endregion

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
            if (skillTimer<=0)
            {
                //执行对应的技能效果
                skillTimer = Time.time;
                player.skill.SkillEffect();
                skillTimer = player.skill.skillCooldown;
            }
        }
        skillTimer -= Time.deltaTime;
        
    }
    private void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && (player.lastRollTime + player.rollCooldown) < Time.time)
        {
            player.lastRollTime = Time.time;
            player.stateMachine.TransitionState("PlayerRoll");
        }
    }
    public void moveAnimation()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            //player.anim.SetBool("isFace", true);
            if (player.GetFaceDirection() == 1)
            {
                player.back.SetActive(false);
                player.face.SetActive(true);
                player.anim.SetTrigger("turnface");
                player.SetFaceDirection(0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            //player.anim.SetBool("isFace", false);
            if (player.GetFaceDirection() == 0)
            {
                player.back.SetActive(true);
                player.face.SetActive(false);
                player.anim.SetTrigger("turnback");
                player.SetFaceDirection(1);
            }
        }
        Flip();

    }

    private void Flip()
    {
        if (Input.GetKeyDown(KeyCode.A) && player.isRight)
        {
            player.face.transform.Rotate(0, 180, 0, Space.Self);
            player.back.transform.Rotate(0, 180, 0, Space.Self);
            player.isRight = false;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !player.isRight)
        {
            player.face.transform.Rotate(0, 180, 0, Space.Self);
            player.back.transform.Rotate(0, 180, 0, Space.Self);
            player.isRight = true;
        }
    }

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
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
