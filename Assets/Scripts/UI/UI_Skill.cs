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
    private void Awake()
    { 
        player = PlayerManager.Instance.player;
        skill_UI = GetComponentInChildren<Image>();
        skill_Text = GetComponentInChildren<TextMeshProUGUI>(); 
    }

    private void OnEnable()
    {
        PlayerAbsorb.OnChangeSkill += ChangeSkillUI;
        player.OnColorChanged += UI_ChangeSkillColor;
    }

    

    private void OnDisable()
    {
        PlayerAbsorb.OnChangeSkill -= ChangeSkillUI;
        player.OnColorChanged -= UI_ChangeSkillColor;
    }
    private void ChangeSkillUI(string skillName)
    {
        if(skillName is "HyperShot")
        {
            if (hypeShotSprite != null) 
                skill_UI.sprite = hypeShotSprite;
            skill_Text.text = "HypeShot";
        }
        else if(skillName is "TrackBullet")
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
    private void UI_ChangeSkillColor(Color color)
    {
        skill_UI.color = color;
    }

}
