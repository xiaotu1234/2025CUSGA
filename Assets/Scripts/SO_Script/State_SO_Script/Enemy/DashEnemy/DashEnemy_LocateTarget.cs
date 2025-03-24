using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "DashEnemy_LocateTarget", menuName = "ScriptableObject/Enemy/DashEnemy/DashEnemy_LocateTarget", order = 0)]
public class DashEnemy_LocateTarget : DashEnemy
{
    private Timer timer;
    [SerializeField] private float locateTime = 2.0f;


    public override void OnEnter()
    {
        timer = new Timer(locateTime);
    }

    public override void OnUpdate()
    {
        bool isOver =  timer.StartTimer();
        if (isOver)
        {

            m_fsm.TransitionState("DashEnemy_Attack");
        }
    }


    public override void OnExit()
    {
        
    }
}
