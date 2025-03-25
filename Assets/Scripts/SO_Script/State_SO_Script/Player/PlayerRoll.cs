using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoll", menuName = "ScriptableObject/Player/PlayerRoll", order = 0)]
public class PlayerRoll : PlayerState
{
    private PlayerController player;

    //控制翻滚关键参数
    public float rollTime = .2f;
    public float rollSpeed = 20;


    private float m_rollTimer;
    private float m_currentRotation;
    private Quaternion m_initialRotation_face;
    private Quaternion m_initialRotation_back;

    public override void OnEnter()
    {
        player = PlayerManager.Instance.player;
        m_rollTimer = Time.time;
        m_initialRotation_face = player.face.transform.localRotation;
        m_initialRotation_back = player.back.transform.localRotation;
    }

    public override void OnExit()
    {
        player.face.transform.localRotation = m_initialRotation_face;//重置旋转
        player.back.transform.localRotation = m_initialRotation_back;
    }

    public override void OnUpdate()
    {
        if (m_rollTimer + rollTime > Time.time)
        {
            Roll();
        }
        else
            player.stateMachine.TransitionState(player.moveState);
        //player.stateMachine.TransitionState();
    }

    private void Roll()
    {
        if (player.GetDir() != Vector3.zero)
        {
            player.GetController().Move(rollSpeed * Time.deltaTime * player.GetDir());
        }
        else
        {
            if (player.GetFaceDirection() == 0)
            {
                player.GetController().Move(-rollSpeed * Time.deltaTime * Vector3.forward);
            }
            else
            {
                player.GetController().Move(rollSpeed * Time.deltaTime * Vector3.forward);
            }
        }
        // 计算当前旋转进度（0~1）
        float progress = (Time.time - m_rollTimer) / rollTime;

        // 绕 X 轴旋转 360°（基于初始角度）
        player.face.transform.localRotation = m_initialRotation_face * Quaternion.Euler(-360f * progress, 0, 0);
        player.back.transform.localRotation = m_initialRotation_back * Quaternion.Euler(-360f * progress, 0, 0);
    }

}
