using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbsorb", menuName = "ScriptableObject/Player/PlayerAbsorb", order = 0)]
public class PlayerAbsorb : PlayerState
{

    [Header("吸收设置")]
    public float absorbRange = 5f;          // 吸收范围
    public float absorbAngle = 90f;         // 扇形角度（单位：度）
    public float absorbForce = 10f;         // 吸引力度
    public float destroyDistance = 10f;      // 销毁敌人的距离

    //[Header("状态持续时间")]
    //public float absorbDuration = 3f;
    //private float timer;

    private List<Transform> absorbedEnemies = new List<Transform>(); // 被吸收的敌人列表
    private PlayerController player;
    private LineRenderer lineRenderer; // 用于绘制扇形的线渲染器
    private Vector3 currentDirection; // 当前扇形方向
    private Color _color;                   //存储吸收颜色

    public override void OnEnter()
    {
        absorbedEnemies.Clear();
        //timer = 0;
        player = PlayerManager.Instance.player;

        #region 控制扇形方向
        if (player.GetDir() == Vector3.zero) 
        {
            if (player.isRight)
                currentDirection = player.transform.right;
            else
                currentDirection = -player.transform.right;
        }
        else
        {
            if (Mathf.Abs(player.GetDir().z) > Mathf.Abs(player.GetDir().x))
            {
                if (player.GetDir().z > 0)
                    currentDirection = player.transform.forward;
                else
                    currentDirection = -player.transform.forward;
            }
            else
            {
                if (player.GetDir().x > 0)
                    currentDirection = player.transform.right;
                else
                    currentDirection = -player.transform.right;
            }
            
        }
        #endregion

        // 创建 LineRenderer（如果不存在）
        if (player.GetComponent<LineRenderer>() == null)
        {
            lineRenderer = player.gameObject.AddComponent<LineRenderer>();
        }
        else
        {
            lineRenderer = player.GetComponent<LineRenderer>();
        }

        // 设置 LineRenderer 参数
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.loop = false;

        DetectEnemiesInSector(); // 进入状态时检测扇形范围内的敌人
    }

    public override void OnExit()
    {
        // 退出状态时清空列表
        absorbedEnemies.Clear();
        // 移除 LineRenderer
        if (lineRenderer != null)
        {
            Object.Destroy(lineRenderer);
        }
    }

    public override void OnUpdate()
    {
        //timer += Time.deltaTime;
        // 每帧更新扇形绘制
        DrawSector();

        Absorb();
        //if (timer > absorbDuration)
        if (Input.GetKeyUp(KeyCode.E)) 
            player.stateMachine.TransitionState("PlayerMove");
    }

    private void Absorb()
    {
        //执行吸收逻辑
        if (absorbedEnemies.Count > 0)
        {
            // 持续吸引敌人
            for (int i = absorbedEnemies.Count - 1; i >= 0; i--)
            {
                if (absorbedEnemies[i] == null)
                {
                    absorbedEnemies.RemoveAt(i);
                    continue;
                }

                Transform enemy = absorbedEnemies[i];
                Vector3 directionToPlayer = (player.transform.position - enemy.position).normalized;

                // 施加吸引力（可改用 Rigidbody.AddForce 若敌人受物理影响）
                enemy.position += directionToPlayer * absorbForce * Time.deltaTime;

                // 检查距离是否小于销毁阈值
                float distance = Vector3.Distance(player.transform.position, enemy.position);
                Debug.LogWarning(distance <= destroyDistance);
                if (distance <= destroyDistance)
                {

                    if (enemy.gameObject.GetComponent<EnemyController>() != null && enemy.gameObject.GetComponent<EnemyController>().skillObject != null)
                    {
                        player.skill = enemy.gameObject.GetComponent<EnemyController>().skillObject;
                        if (player.skill.name == "HyperShot")
                        {
                            player.SetColor(enemy.gameObject.GetComponent<DashEnemyController>().color);

                            Debug.Log($"吸收成功,赋值颜色成功{player.skill != null}");

                        }
                        //播放音效
                        AudioManager.Instance.PlayerSFX(0);
                    }
                    if (player.UI_Skill == null)
                        Debug.LogError("PkayerController的UI_Skill没赋值");
                    else
                        player.UI_Skill.GetComponent<UI_Skill>().ChangeSkillUI(player.skill);
                    EnemyManager.Instance.DestroyEnemy(enemy.GetComponent<EnemyController>());
                    absorbedEnemies.RemoveAt(i);
                }
            }
        }
    }

    private void DrawSector()
    {
        int segments = 30; // 扇形分段数（越高越平滑）
        lineRenderer.positionCount = segments + 3; // 顶点数 = 圆弧点(segments+1) + 左半径 + 右半径

        Vector3 origin = player.transform.position;

        // 左半径边（从圆心到扇形左边缘）
        Vector3 leftEdge = origin + Quaternion.Euler(0, -absorbAngle * 0.5f, 0) * currentDirection * absorbRange;
        lineRenderer.SetPosition(0, origin); // 起点：圆心
        lineRenderer.SetPosition(1, leftEdge); // 左半径终点

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Lerp(-absorbAngle * 0.5f, absorbAngle * 0.5f, (float)i / segments);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * currentDirection;
            Vector3 pos = origin + dir * absorbRange;
            lineRenderer.SetPosition(i + 1, pos); // 设置扇形边缘点
        }
        // 右半径边（从扇形右边缘回到圆心）
        Vector3 rightEdge = origin + Quaternion.Euler(0, absorbAngle * 0.5f, 0) * currentDirection * absorbRange;
        lineRenderer.SetPosition(segments + 2, origin); // 终点：圆心
    }

    // 扇形检测敌人（基于Tag）
    private void DetectEnemiesInSector()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, absorbRange);
        foreach (var collider in hitColliders)
        {
            // 检查Tag是否为敌人
            if (collider.CompareTag("SkillBall"))
            {
                Vector3 directionToEnemy = (collider.transform.position - player.transform.position).normalized;
                float angleToEnemy = Vector3.Angle(currentDirection, directionToEnemy);

                // 判断是否在扇形角度内
                if (angleToEnemy <= absorbAngle * 0.5f)
                {
                    absorbedEnemies.Add(collider.transform);
                }
            }
        }
    }


   
}
