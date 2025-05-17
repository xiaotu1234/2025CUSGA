using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCD : MonoBehaviour
{
    private Image ui_cd;
    private PlayerController player;
    void Start()
    {
        ui_cd = GetComponent<Image>();
        player = PlayerManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        ui_cd.fillAmount = player.skillTimer/player.skill.skillCooldown;
    }
}
