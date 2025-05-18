using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_BossBarManager : MonoBehaviour
{

    public List<GameObject> healthPoints = new List<GameObject>();
    public SegmentedBossHealth phaseBar;
    private BossManager _bossManager;
    private int _currentPhase = 0;
    private void Awake()
    {
        _bossManager = BossManager.Instance;
    }
    private void OnEnable()
    {
        _bossManager.OnEnterPhase2 += ChangeToPhase2;
        _bossManager.OnEnterPhase3 += ChangeToPhase3;
        _bossManager.OnBossDie += WhenBossDie;
        
    }
    private void OnDisable()
    {
        _bossManager.OnEnterPhase2 -= ChangeToPhase2;
        _bossManager.OnEnterPhase3 -= ChangeToPhase3;
        _bossManager.OnBossDie -= WhenBossDie;
    }

    private void ChangeToPhase2()
    {
        _currentPhase = 2;
        foreach (var healthPoint in healthPoints)
        {
            healthPoint.gameObject.SetActive(true);
        }
        phaseBar.gameObject.SetActive(true);
    }

    private void ChangeToPhase3()
    {
        _currentPhase = 3;
        GameObject healthPoint0 = healthPoints.Find(result => result.name == "HealthPoint0");
        Destroy(healthPoint0);
    }

    private void WhenBossDie()
    {
        
        GameObject healthPoint1 = healthPoints.Find(result => result.name == "HealthPoint1");
        Destroy(healthPoint1);
        
    }

    private void WaitForDestory()
    {
        Destroy(phaseBar.gameObject);
        Destroy(this);
    }

}
