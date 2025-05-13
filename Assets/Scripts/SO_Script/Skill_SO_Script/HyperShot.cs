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
    private BallChainController _controller;
    private Rigidbody _rb;
    private Transform firePoint;
    private List<Color> Colors;

    public override void Initialize()
    {
        base.Initialize();
        _controller = BallChainController.Instance;
        firePoint = player.firePoint.transform;
        Colors = _controller.ballColors;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        int index = UnityEngine.Random.Range(0, Colors.Count);
        var color = Colors[index];
        Ball shootBall = _controller.GetShootBall(firePoint.position, firePoint.rotation);
        shootBall.SetColor(color);
        if (shootBall.Rigidbody != null)
            _rb = shootBall.Rigidbody;
        else
            Debug.LogError("Ball脚本没有引用RB");
        _rb.velocity = player.GetComponent<PlayerShooting>().shootDirection * shotSpeed;
        
    }

}
