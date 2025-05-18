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
        AudioManager.Instance.PlaySFX(9);
        player = PlayerManager.Instance.player;
        m_rollTimer = Time.time;
        m_initialRotation_face = player.transform.localRotation;
        //m_initialRotation_back = player.back.transform.localRotation;
    }

    public override void OnExit()
    {
        player.transform.localRotation = m_initialRotation_face;//重置旋转
        //player.back.transform.localRotation = m_initialRotation_back;
    }

    public override void OnUpdate()
    {
        if (m_rollTimer + rollTime > Time.time)
        {
            Roll();
        }
        else
            player.stateMachine.TransitionState("PlayerMove");
        //player.stateMachine.TransitionState();
    }

    private void Roll()
    {
        #region ControlMovement
        if (player.GetDir() != Vector3.zero)
        {
            player.GetController().Move(rollSpeed * Time.deltaTime * player.GetDir().normalized);
        }
        else
        {
            if (player.isRight)
            {
                player.GetController().Move(rollSpeed * Time.deltaTime * Vector3.right);
            }
            else
            {
                player.GetController().Move(-rollSpeed * Time.deltaTime * Vector3.right);
            }
        }
        #endregion

        #region ControlRoll
        // 计算当前旋转进度（0~1）
        float progress = (Time.time - m_rollTimer) / rollTime;
        if (player.GetDir().z != 0)
        {
            if (player.GetFaceDirection() == 1&&player.isRight|| player.GetFaceDirection() == 0 && !player.isRight) 
            {
                player.transform.localRotation = m_initialRotation_face * Quaternion.Euler(360f * progress, 0, 0);
                //player.back.transform.localRotation = m_initialRotation_back * Quaternion.Euler(-360f * progress, 0, 0);
            }
            else
            {
                player.transform.localRotation = m_initialRotation_face * Quaternion.Euler(-360f * progress, 0, 0);
                //player.back.transform.localRotation = m_initialRotation_back * Quaternion.Euler(360f * progress, 0, 0);
            }
        }
        else
        {
            if (player.GetFaceDirection() == 1)
            {
                // 绕 z 轴旋转 360°（基于初始角度）
                player.transform.localRotation = m_initialRotation_face * Quaternion.Euler(0, 0, -360f * progress);
                //player.back.transform.localRotation = m_initialRotation_back * Quaternion.Euler(0, 0, 360f * progress);
            }
            else
            {
                // 绕 z 轴旋转 360°（基于初始角度）
                player.transform.localRotation = m_initialRotation_face * Quaternion.Euler(0, 0, -360f * progress);
                //player.back.transform.localRotation = m_initialRotation_back * Quaternion.Euler(0, 0, -360f * progress);
            }
        }
        #endregion
    }

}
