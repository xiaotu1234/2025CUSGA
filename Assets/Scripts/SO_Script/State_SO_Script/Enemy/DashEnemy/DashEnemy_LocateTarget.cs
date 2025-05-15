using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "DashEnemy_LocateTarget", menuName = "ScriptableObject/Enemy/DashEnemy/DashEnemy_LocateTarget", order = 0)]
public class DashEnemy_LocateTarget : DashEnemy
{
    private Timer timer;
    [SerializeField] private float locateTime = 2.0f;
    [SerializeField] private float rotationSpeed = 10.0f;


    public override void OnEnter()
    {
        timer = new Timer(locateTime);
    }

    public override void OnUpdate()
    {
        bool isOver =  timer.StartTimer();
        FacePlayer();
        if (isOver)
        {
            m_fsm.TransitionState("DashEnemy_Attack");
        }
    }


    public override void OnExit()
    {
        
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
}
