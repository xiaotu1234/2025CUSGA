using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossBarManager : MonoBehaviour
{

    public List<GameObject> healthPoints = new List<GameObject>();
    public UI_BossHealthBar phaseBar;
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
        _bossManager.OnResetBoss += InitializeHealthPoint;
        InitializeHealthPoint();

    }


    private void OnDisable()
    {
        _bossManager.OnEnterPhase2 -= ChangeToPhase2;
        _bossManager.OnEnterPhase3 -= ChangeToPhase3;
        _bossManager.OnBossDie -= WhenBossDie;
        _bossManager.OnResetBoss -= InitializeHealthPoint;
    }
    private void InitializeHealthPoint()
    {
        foreach (var healthPoint in healthPoints)
        {
            healthPoint.GetComponent<Image>().color = Color.red;
        }
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
        healthPoint0.GetComponent<Image>().color = Color.gray;

    }

    private void WhenBossDie()
    {
        
        GameObject healthPoint1 = healthPoints.Find(result => result.name == "HealthPoint1");
        healthPoint1.GetComponent<Image>().color = Color.gray;

    }

    private void WaitForDestory()
    {
        Destroy(phaseBar.gameObject);
        Destroy(this);
    }

}
