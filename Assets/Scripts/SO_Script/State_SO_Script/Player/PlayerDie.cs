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
    public override void OnEnter()
    {
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
        // 第一阶段：显示死亡效果
        yield return new WaitForSecondsRealtime(respawnDelay * 0.5f);

        // 第二阶段：执行游戏重置（但不立即复活）
        if (!hasResetGame)
        {
            ResetGameState();
            hasResetGame = true;
            // 第三阶段：等待一段时间后允许玩家重新控制
            yield return new WaitForSecondsRealtime(respawnDelay * 0.5f);

            // 切换到重生状态或闲置状态
            player.stateMachine.TransitionState("PlayerMove");
            hasResetGame = false;
        }
        
    }
    private void ResetGameState()
    {
        // 1. 清除并重置
        EnemyManager.Instance.DestroyAllEnemies();

        // 2. 重置boss阶段
        BossManager.Instance.ResetBoss();
        
    }
    public override void OnExit()
    {
        // 重新启用组件
        player.gameObject.GetComponent<PlayerShooting>().enabled = true;

        player.Reburn();

    }

    public override void OnUpdate()
    {

    }
}
