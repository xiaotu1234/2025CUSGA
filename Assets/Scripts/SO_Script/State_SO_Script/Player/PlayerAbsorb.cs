using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbsorb", menuName = "ScriptableObject/Player/PlayerAbsorb", order = 0)]
public class PlayerAbsorb : PlayerState
{

    [Header("��������")]
    public float absorbRange = 5f;          // ���շ�Χ
    public float absorbAngle = 90f;         // ���νǶȣ���λ���ȣ�
    public float absorbForce = 10f;         // ��������
    public float destroyDistance = 10f;      // ���ٵ��˵ľ���

    //[Header("״̬����ʱ��")]
    //public float absorbDuration = 3f;
    //private float timer;

    private List<Transform> absorbedEnemies = new List<Transform>(); // �����յĵ����б�
    private PlayerController player;
    private LineRenderer lineRenderer; // ���ڻ������ε�����Ⱦ��
    private Vector3 currentDirection; // ��ǰ���η���
    private Color _color;                   //�洢������ɫ

    public override void OnEnter()
    {
        absorbedEnemies.Clear();
        //timer = 0;
        player = PlayerManager.Instance.player;

        #region �������η���
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

        // ���� LineRenderer����������ڣ�
        if (player.GetComponent<LineRenderer>() == null)
        {
            lineRenderer = player.gameObject.AddComponent<LineRenderer>();
        }
        else
        {
            lineRenderer = player.GetComponent<LineRenderer>();
        }

        // ���� LineRenderer ����
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.loop = false;

        DetectEnemiesInSector(); // ����״̬ʱ������η�Χ�ڵĵ���
    }

    public override void OnExit()
    {
        // �˳�״̬ʱ����б�
        absorbedEnemies.Clear();
        // �Ƴ� LineRenderer
        if (lineRenderer != null)
        {
            Object.Destroy(lineRenderer);
        }
    }

    public override void OnUpdate()
    {
        //timer += Time.deltaTime;
        // ÿ֡�������λ���
        DrawSector();

        Absorb();
        //if (timer > absorbDuration)
        if (Input.GetKeyUp(KeyCode.E)) 
            player.stateMachine.TransitionState("PlayerMove");
    }

    private void Absorb()
    {
        //ִ�������߼�
        if (absorbedEnemies.Count > 0)
        {
            // ������������
            for (int i = absorbedEnemies.Count - 1; i >= 0; i--)
            {
                if (absorbedEnemies[i] == null)
                {
                    absorbedEnemies.RemoveAt(i);
                    continue;
                }

                Transform enemy = absorbedEnemies[i];
                Vector3 directionToPlayer = (player.transform.position - enemy.position).normalized;

                // ʩ�����������ɸ��� Rigidbody.AddForce ������������Ӱ�죩
                enemy.position += directionToPlayer * absorbForce * Time.deltaTime;

                // �������Ƿ�С��������ֵ
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

                            Debug.Log($"���ճɹ�,��ֵ��ɫ�ɹ�{player.skill != null}");

                        }
                        //������Ч
                        AudioManager.Instance.PlayerSFX(0);
                    }
                    if (player.UI_Skill == null)
                        Debug.LogError("PkayerController��UI_Skillû��ֵ");
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
        int segments = 30; // ���ηֶ�����Խ��Խƽ����
        lineRenderer.positionCount = segments + 3; // ������ = Բ����(segments+1) + ��뾶 + �Ұ뾶

        Vector3 origin = player.transform.position;

        // ��뾶�ߣ���Բ�ĵ��������Ե��
        Vector3 leftEdge = origin + Quaternion.Euler(0, -absorbAngle * 0.5f, 0) * currentDirection * absorbRange;
        lineRenderer.SetPosition(0, origin); // ��㣺Բ��
        lineRenderer.SetPosition(1, leftEdge); // ��뾶�յ�

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Lerp(-absorbAngle * 0.5f, absorbAngle * 0.5f, (float)i / segments);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * currentDirection;
            Vector3 pos = origin + dir * absorbRange;
            lineRenderer.SetPosition(i + 1, pos); // �������α�Ե��
        }
        // �Ұ뾶�ߣ��������ұ�Ե�ص�Բ�ģ�
        Vector3 rightEdge = origin + Quaternion.Euler(0, absorbAngle * 0.5f, 0) * currentDirection * absorbRange;
        lineRenderer.SetPosition(segments + 2, origin); // �յ㣺Բ��
    }

    // ���μ����ˣ�����Tag��
    private void DetectEnemiesInSector()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, absorbRange);
        foreach (var collider in hitColliders)
        {
            // ���Tag�Ƿ�Ϊ����
            if (collider.CompareTag("SkillBall"))
            {
                Vector3 directionToEnemy = (collider.transform.position - player.transform.position).normalized;
                float angleToEnemy = Vector3.Angle(currentDirection, directionToEnemy);

                // �ж��Ƿ������νǶ���
                if (angleToEnemy <= absorbAngle * 0.5f)
                {
                    absorbedEnemies.Add(collider.transform);
                }
            }
        }
    }


   
}
