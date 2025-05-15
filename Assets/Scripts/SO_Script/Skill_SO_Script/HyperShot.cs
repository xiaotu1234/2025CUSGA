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
    private Color _color;
    private List<Color> _colorList;

    public override void Initialize()
    {
        base.Initialize();
        _controller = BallChainController.Instance;
        _color = PlayerManager.Instance.player.GetColor();
        firePoint = player.firePoint.transform;
        _colorList = _controller.ballColors;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        int index = Random.Range(0, _colorList.Count);
        Ball shootBall = _controller.GetShootBall(firePoint.position, firePoint.rotation);

        if(_controller.isTesting) 
            shootBall.SetColor(_colorList[0]);
        else
            shootBall.SetColor(_color);



        if (shootBall.Rigidbody != null)
            _rb = shootBall.Rigidbody;
        else
            Debug.LogError("Ball脚本没有引用RB");
        _rb.velocity = player.GetComponent<PlayerShooting>().shootDirection * shotSpeed;
        
    }

}
