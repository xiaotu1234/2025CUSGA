using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRoll : PlayerState
{
    private PlayerController player = PlayerManager.Instance.player;
    private float m_rollTime = 1;
    private float m_rollSpeed = 10;
    private float m_rollTimer;
    private float m_currentRotation;
    public override void OnEnter()
    {
        m_rollTimer = Time.time;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (m_rollTimer + m_rollTime < Time.time) 
            Roll();
        else
            OnExit();
    }

    private void Roll()
    {
        if (player.GetDir() != Vector3.zero)
        {
            player.GetController().Move(m_rollSpeed * Time.deltaTime * player.GetDir());
        }
        else
        {
            if (player.GetFaceDirection() == 0)
            {
                player.GetController().Move(-m_rollSpeed * Time.deltaTime * Vector3.forward);
            }
            else
            {
                player.GetController().Move(m_rollSpeed * Time.deltaTime * Vector3.forward);
            }
        }
        // ����ÿ֡��Ҫ��ת�ĽǶ�
        float rotationThisFrame = (360 / m_rollTime) * Time.deltaTime;

        // �� X ����ת
        player.face.transform.Rotate(rotationThisFrame, 0, 0);

        // ���µ�ǰ����ת�ĽǶ�
        m_currentRotation += rotationThisFrame;
    }

    private void Update()
    {
        OnUpdate();
    }
}
