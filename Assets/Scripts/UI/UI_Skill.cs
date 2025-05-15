using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : MonoBehaviour
{
    public Sprite hypeShotSprite;
    public Sprite trackBulletSprite;
    private Image skill_UI;
    private TextMeshProUGUI skill_Text;
    private PlayerController player;
    void Start()
    { 
        player = PlayerManager.Instance.player;
        skill_UI = GetComponentInChildren<Image>();
        skill_Text = GetComponentInChildren<TextMeshProUGUI>();
        ChangeSkillUI(player.skill);
    }

    // Update is called once per frame
    void Update()
    {
        if(skill_UI.sprite = hypeShotSprite)
            skill_UI.color = player.GetColor();
    }
    public void ChangeSkillUI(Skill skill)
    {
        if(skill is HyperShot)
        {
            if (hypeShotSprite != null) 
                skill_UI.sprite = hypeShotSprite;
            skill_Text.text = "HypeShot";
        }
        else if(skill is TrackBullet)
        {
            if (trackBulletSprite != null)
            {
                skill_UI.sprite = trackBulletSprite;
            }
            skill_Text.text = "TrackBullet";
        }
        else
        {
            skill_Text.text = "NULL";
        }
    }
}
