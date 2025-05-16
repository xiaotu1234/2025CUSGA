using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_itemCD : MonoBehaviour
{
    [SerializeField] private Image _mask; // ������ʾ��ȴЧ����Image���
    private bool _isCoolingDown = false;
    [SerializeField] private float _cooldownDuration = 5f; // ��ȴʱ�䣬����5��
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
            // �������ֵ������
            _mask.fillAmount = timer / _cooldownDuration;
            timer += Time.deltaTime;
            yield return null; // �ȴ���һ֡
        }
        _mask.fillAmount = 0;
        _isCoolingDown = false;
    }
}

