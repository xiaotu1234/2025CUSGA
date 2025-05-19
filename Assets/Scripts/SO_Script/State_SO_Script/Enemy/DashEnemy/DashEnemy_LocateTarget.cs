using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "DashEnemy_LocateTarget", menuName = "ScriptableObject/Enemy/DashEnemy/DashEnemy_LocateTarget", order = 0)]
public class DashEnemy_LocateTarget : DashEnemy
{
    private Timer timer;
    [SerializeField] private float locateTime = 2.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    private LineRenderer lineRenderer;

    public override void OnEnter()
    {
        timer = new Timer(locateTime);
        SetupLineRenderer();
        lineRenderer.enabled = false;
    }

    public override void OnUpdate()
    {
        if (EnemyManager.Instance.isStarted) 
            lineRenderer.enabled = true;

        bool isOver =  timer.StartTimer();
        FacePlayer();
        UpdateLinePosition();
        if (isOver)
        {
            m_fsm.TransitionState("DashEnemy_Attack");
        }
    }


    public override void OnExit()
    {
        // 状态退出时禁用LineRenderer
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    private void FacePlayer()
    {
        Debug.Log($"m_player == null: {m_player == null}, m_enemy == null: {m_enemy == null}");
        Vector3 direction = m_player.transform.position - m_enemy.transform.position;
        direction.y = 0;
        m_enemy.transform.rotation = Quaternion.Slerp(m_enemy.transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        rotationSpeed * Time.deltaTime);
       
    }
    private void SetupLineRenderer()
    {
        // 检查敌人对象是否已有LineRenderer组件，没有则添加
        lineRenderer = m_enemy.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = m_enemy.AddComponent<LineRenderer>();

            // 配置LineRenderer属性
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 2;
        }
    }
    // 新增：更新线的位置
    private void UpdateLinePosition()
    {
        if (m_player != null && lineRenderer != null)
        {
            lineRenderer.SetPosition(0, m_enemy.transform.position);
            lineRenderer.SetPosition(1, m_player.transform.position);
        }
    }
}
