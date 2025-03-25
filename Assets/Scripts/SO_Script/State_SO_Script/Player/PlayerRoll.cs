using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoll", menuName = "ScriptableObject/Player/PlayerRoll", order = 0)]
public class PlayerRoll : PlayerState
{
    private PlayerController player;

    //���Ʒ����ؼ�����
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
        player.face.transform.localRotation = m_initialRotation_face;//������ת
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
        // ���㵱ǰ��ת���ȣ�0~1��
        float progress = (Time.time - m_rollTimer) / rollTime;

        // �� X ����ת 360�㣨���ڳ�ʼ�Ƕȣ�
        player.face.transform.localRotation = m_initialRotation_face * Quaternion.Euler(-360f * progress, 0, 0);
        player.back.transform.localRotation = m_initialRotation_back * Quaternion.Euler(-360f * progress, 0, 0);
    }

}
