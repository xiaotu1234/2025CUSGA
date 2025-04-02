using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "HyperShot", menuName = "ScriptableObject/Skill/HyperShot", order = 0)]

public class HyperShot : Skill
{
    public float shotSpeed;
    public GameObject hyperShotPrefab;

    public override void SkillEffect()
    {
        base.SkillEffect();
        GameObject bullet = Instantiate(hyperShotPrefab, player.firePoint.transform.position, player.firePoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = player.GetComponent<PlayerShooting>().shootDirection * shotSpeed;
    }

}
