using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDie", menuName = "ScriptableObject/Player/PlayerDie", order = 0)]
public class PlayerDie : PlayerState
{
    private PlayerController player;
    [SerializeField] private float respawnDelay = 2f; // �������ӳ�����ʱ��
    private bool hasResetGame = false; // ��ֹ�ظ�����
    public override void OnEnter()
    {
        player = PlayerManager.Instance.player;
        // ����������ƶ����
        player.gameObject.GetComponent<PlayerShooting>().enabled = false;

        // ������������/��Ч
        //player.gameObject.animator.SetTrigger("Die");

        // ����Э��ִ����Ϸ����
        SceneManager.Instance.StartCoroutine(ExecuteDeathSequence());

        hasResetGame = false; // ���ñ��
    }
    private IEnumerator ExecuteDeathSequence()
    {
        // ��һ�׶Σ���ʾ����Ч��
        yield return new WaitForSecondsRealtime(respawnDelay * 0.5f);

        // �ڶ��׶Σ�ִ����Ϸ���ã������������
        if (!hasResetGame)
        {
            ResetGameState();
            hasResetGame = true;
            // �����׶Σ��ȴ�һ��ʱ�������������¿���
            yield return new WaitForSecondsRealtime(respawnDelay * 0.5f);

            // �л�������״̬������״̬
            player.stateMachine.TransitionState("PlayerMove");
            hasResetGame = false;
        }
        
    }
    private void ResetGameState()
    {
        // 1. ���������
        EnemyManager.Instance.DestroyAllEnemies();

        // 2. ����boss�׶�
        BossManager.Instance.ResetBoss();
        
    }
    public override void OnExit()
    {
        // �����������
        player.gameObject.GetComponent<PlayerShooting>().enabled = true;

        player.Reburn();

    }

    public override void OnUpdate()
    {

    }
}
