using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDie", menuName = "ScriptableObject/Player/PlayerDie", order = 0)]
public class PlayerDie : PlayerState
{
    private PlayerController player;
    [SerializeField] private float respawnDelay = 2f; // 死亡后延迟重生时间
    private bool hasResetGame = false; // 防止重复重置
    public GameObject retryUI;
    public override void OnEnter()
    {
        retryUI = SceneManager.Instance.retryUI;
        player = PlayerManager.Instance.player;
        // 禁用射击和移动组件
        player.gameObject.GetComponent<PlayerShooting>().enabled = false;

        // 播放死亡动画/特效
        //player.gameObject.animator.SetTrigger("Die");

        // 启动协程执行游戏重置
        SceneManager.Instance.StartCoroutine(ExecuteDeathSequence());

        hasResetGame = false; // 重置标记
    }
    private IEnumerator ExecuteDeathSequence()
    {
        Time.timeScale = 0.1f;
        //在这里加死亡动画逻辑
        //todo:
        // 第一阶段：显示死亡效果
        yield return new WaitForSecondsRealtime(respawnDelay * 0.5f);

        // 暂停游戏
        Time.timeScale = 0f;

        // 显示重新挑战UI
        ShowRetryUI();

    }
    private void ShowRetryUI()
    {

        retryUI.SetActive(true);

        // 获取重试按钮并添加点击事件
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
        // 移除按钮事件
        if (retryUI != null)
        {
            var retryButton = retryUI.GetComponentInChildren<UnityEngine.UI.Button>();
            if (retryButton != null)
            {
                retryButton.onClick.RemoveAllListeners();
            }
        }

        retryUI.SetActive(false);

        // 恢复游戏时间
        Time.timeScale = 1f;

        // 重置游戏状态
        if (!hasResetGame)
        {
            ResetGameState();
            hasResetGame = true;
        }
    }

    private void ResetGameState()
    {
        // 1. 清除并重置
        EnemyManager.Instance.DestroyAllEnemies();
        DestroyAllBullet();
        DestroyAllSheLiZi();
        EnemyManager.Instance.isRetry = true;
        // 2. 重置boss阶段
        BossManager.Instance.ResetBoss();
        // 重新启用组件
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
        // 获取指定类型的所有组件实例
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        PlayerBulletBase[] playerBullets = FindObjectsOfType<PlayerBulletBase>();
        foreach (Bullet bullet in bullets)
        {
            // 获取脚本所在的游戏对象
            GameObject obj = bullet.gameObject;

            // 销毁游戏对象
            Destroy(obj);
        }
        foreach (PlayerBulletBase bullet in playerBullets)
        {
            // 获取脚本所在的游戏对象
            GameObject obj = bullet.gameObject;

            // 销毁游戏对象
            Destroy(obj);
        }
    }
    public void DestroyAllSheLiZi()
    {
        // 获取指定类型的所有组件实例
        SheLiZi[] scripts = FindObjectsOfType<SheLiZi>();

        foreach (SheLiZi shelizi in scripts)
        {
            // 获取脚本所在的游戏对象
            GameObject obj = shelizi.gameObject;

            // 销毁游戏对象
            Destroy(obj);
        }
    }
}
