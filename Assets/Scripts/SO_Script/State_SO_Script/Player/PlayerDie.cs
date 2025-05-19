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
    public GameObject retryUI;
    public override void OnEnter()
    {
        retryUI = SceneManager.Instance.retryUI;
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
        Time.timeScale = 0.1f;
        //����������������߼�
        //todo:
        // ��һ�׶Σ���ʾ����Ч��
        yield return new WaitForSecondsRealtime(respawnDelay * 0.5f);

        // ��ͣ��Ϸ
        Time.timeScale = 0f;

        // ��ʾ������սUI
        ShowRetryUI();

    }
    private void ShowRetryUI()
    {

        retryUI.SetActive(true);

        // ��ȡ���԰�ť����ӵ���¼�
        if (retryUI != null)
        {
            var retryButton = retryUI.GetComponentInChildren<UnityEngine.UI.Button>();
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryButtonClicked);
            }
        }
    }
    private void OnRetryButtonClicked()
    {
        // �Ƴ���ť�¼�
        if (retryUI != null)
        {
            var retryButton = retryUI.GetComponentInChildren<UnityEngine.UI.Button>();
            if (retryButton != null)
            {
                retryButton.onClick.RemoveAllListeners();
            }
        }

        retryUI.SetActive(false);

        // �ָ���Ϸʱ��
        Time.timeScale = 1f;

        // ������Ϸ״̬
        if (!hasResetGame)
        {
            ResetGameState();
            hasResetGame = true;
        }
    }

    private void ResetGameState()
    {
        // 1. ���������
        EnemyManager.Instance.DestroyAllEnemies();
        DestroyAllBullet();
        DestroyAllSheLiZi();
        EnemyManager.Instance.isRetry = true;
        // 2. ����boss�׶�
        BossManager.Instance.ResetBoss();
        // �����������
        player.gameObject.GetComponent<PlayerShooting>().enabled = true;

        player.Reburn();
        player.stateMachine.TransitionState("PlayerMove");

    }
    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
    public void DestroyAllBullet()
    {
        // ��ȡָ�����͵��������ʵ��
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        PlayerBulletBase[] playerBullets = FindObjectsOfType<PlayerBulletBase>();
        foreach (Bullet bullet in bullets)
        {
            // ��ȡ�ű����ڵ���Ϸ����
            GameObject obj = bullet.gameObject;

            // ������Ϸ����
            Destroy(obj);
        }
        foreach (PlayerBulletBase bullet in playerBullets)
        {
            // ��ȡ�ű����ڵ���Ϸ����
            GameObject obj = bullet.gameObject;

            // ������Ϸ����
            Destroy(obj);
        }
    }
    public void DestroyAllSheLiZi()
    {
        // ��ȡָ�����͵��������ʵ��
        SheLiZi[] scripts = FindObjectsOfType<SheLiZi>();

        foreach (SheLiZi shelizi in scripts)
        {
            // ��ȡ�ű����ڵ���Ϸ����
            GameObject obj = shelizi.gameObject;

            // ������Ϸ����
            Destroy(obj);
        }
    }
}
