using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public override void TransitionState(string stateName)
    {
        
        if (currentState.name == "DashEnemy_Die(Clone)")
        {
            
            return;
        }
        base.TransitionState(stateName);
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

    public void OnDestroySelf()
    {
        foreach (var state in _enemyState)
        {
            Destroy(state);

        }
        if (Application.isPlaying)
        {
            TryDropSheLiZi();
        }
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
        Debug.Log($"总保底：{guaranteeCount}，本次垫数：{_missCount}，本次概率：{_currentProbability}");

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
        if (sheLiZiPrefab != null && Application.isPlaying)
        {
            Bounds bound = _enemyManager.produceEnemyArea.bounds;
            float x = Mathf.Clamp(transform.position.x, bound.min.x, bound.max.x);
            float z = Mathf.Clamp(transform.position.z, bound.min.z, bound.max.z);
            Vector3 position = new Vector3(x, transform.position.y, z);
            Instantiate(sheLiZiPrefab, position, Quaternion.identity);
        }
        else if (sheLiZiPrefab == null )
        {
            Debug.LogWarning("未在EnmeyStateMachie中添加血包预制体");
        }
        else if (!Application.isPlaying)
        {
            Debug.LogWarning("编辑器模式下禁止生成血包");
        }
        else
        {
            Debug.LogError("没有添加血包预制体");
        }
    }
}
