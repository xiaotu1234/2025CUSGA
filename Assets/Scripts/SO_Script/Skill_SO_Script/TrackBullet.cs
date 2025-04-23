using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrackBullet", menuName = "ScriptableObject/Skill/TrackBullet", order = 0)]

public class TrackBullet : Skill
{
    public float durationTime;
    public override void SkillEffect()
    {
        base.SkillEffect();
        player.GetComponent<PlayerShooting>().StartCoroutine(SetTrackBullet());
    }
    IEnumerator SetTrackBullet()
    {
        player.GetComponent<PlayerShooting>().normalShoot = false;

        yield return new WaitForSeconds(durationTime);
        player.GetComponent<PlayerShooting>().normalShoot = true;
    }
}
