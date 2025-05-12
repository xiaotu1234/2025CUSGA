using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    protected PlayerController player;
    public float skillCooldown;
    public virtual void SkillEffect()
    {
        player = PlayerManager.Instance.player;
    }
}
