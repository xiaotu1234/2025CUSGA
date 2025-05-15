using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStasteMachine : StateMachine
{
    private List<EnemyState> _enemyState = new List<EnemyState>();
    [SerializeField, Tooltip("勾选会让怪物死亡必定掉落血包")]
    private  bool _isDebugging =false;
    public GameObject sheLiZiPrefab;
    private EnemyManager _enemyManager;
    private int guaranteeCount;
    private float _currentProbability;
    private int _missCount;
    public override void Awake()
    {
        foreach (var state in states)
        {
            if (state is EnemyState enemyState)
            {
                EnemyState newEnemyState = Instantiate(enemyState);
                newEnemyState.m_enemy = this.gameObject;
                _enemyState.Add(newEnemyState);
                
            }
        }
    }

    private void Start()
    {
        _enemyManager = EnemyManager.Instance;
        guaranteeCount = _enemyManager.guaranteeCount;
        foreach (var state in _enemyState)
        {
            state.OnAwake();
        }
    }



    protected override StateBase FindState(string stateName)
    {
        stateName = stateName + "(Clone)";
        for (int i = 0; i < _enemyState.Count; i++)
        {
            Debug.Log(stateName);
            if (_enemyState[i].name == stateName)
                return _enemyState[i];
        }
        return null;
    }

    private void OnDestroy()
    {
        foreach (var state in _enemyState)
        {
            Destroy(state);

        }
        TryDropSheLiZi();
    }

    public void TryDropSheLiZi()
    {
        bool isDrop = CheckDropProbability();
        if (isDrop)
        {
            DrawSheLiZi();
            ResetProbability();
        }
        else
        {
            IncreaseProbability();
        }
    }

    bool CheckDropProbability()
    {
        if (_isDebugging)
        {
            return true;
        }
        _missCount = _enemyManager.GetMissCount();
        _currentProbability = _enemyManager.GetCurrntProbability();
        Debug.Log($"总保底：{guaranteeCount}，当前垫数：{_missCount}，当前概率：{_currentProbability}");

        // 保底强制掉落逻辑[9](@ref)
        if (_missCount >= guaranteeCount) 
        {
            Debug.Log("掉落回血包");
            return true; 
        }

        // 动态概率计算[1,4](@ref)
        float rand = Random.Range(0f, 1f);
        return rand <= _currentProbability;
    }

    void IncreaseProbability()
    {
        _enemyManager.AddMissCount();
        _enemyManager.AddCurrntProbability();
    }

    void ResetProbability()
    {
        _enemyManager.ResetProbability();
        _enemyManager.ResetMissCount();
    }

    private void DrawSheLiZi()
    {
        if (sheLiZiPrefab != null)
            Instantiate(sheLiZiPrefab, transform.position, Quaternion.identity);
        else
            Debug.LogError("没有添加血包预制体");
    }
}
