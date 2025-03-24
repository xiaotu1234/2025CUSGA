using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerMove", menuName = "ScriptableObject/Player/PlayerMove", order = 0)]
public class PlayerMove : PlayerState
{
    #region Movement Settings “∆∂Ø…Ë÷√
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = -20f;
    private Vector3 dir;
    private CharacterController m_controller;
    private Vector3 m_velocity;
    private Transform m_transform;
    #endregion

    public override void OnAwake()
    {
        base.OnAwake();
        m_controller = m_Player.GetComponent<CharacterController>();
        m_transform = m_Player.transform;
    }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }
    public override void OnExit()
    {

    }


    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        dir = new Vector3(x, 0, z);
        if (dir != Vector3.zero)
        {
            m_transform.LookAt(m_transform.position + dir);
        }

        m_controller.Move(dir * moveSpeed * Time.deltaTime);
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
