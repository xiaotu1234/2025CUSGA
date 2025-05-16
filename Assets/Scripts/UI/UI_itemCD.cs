using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_itemCD : MonoBehaviour
{
    [SerializeField] private Image _mask; // 用于显示冷却效果的Image组件
    private bool _isCoolingDown = false;
    [SerializeField] private float _cooldownDuration = 5f; // 冷却时间，例如5秒
    // Start is called before the first frame update
    void Start()
    {
        _mask.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCooldown()
    {
        if (!_isCoolingDown)
        {
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        _isCoolingDown = true;
        float timer = 0f;
        while (timer < _cooldownDuration)
        {
            // 更新遮罩的填充量
            _mask.fillAmount = timer / _cooldownDuration;
            timer += Time.deltaTime;
            yield return null; // 等待下一帧
        }
        _mask.fillAmount = 0;
        _isCoolingDown = false;
    }
}

