using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();

        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = PlayerManager.Instance.player.maxHealth;
        slider.value = PlayerManager.Instance.player.GetCurrentHealth();
    }
}
