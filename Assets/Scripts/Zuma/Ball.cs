using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using UnityEngine;
using Zuma.Curve;

public class Ball : MonoBehaviour
{
    public Color ballColor; // 球的颜色
    public Ball frontBall = null; // 前一个球
    public Ball backBall = null; // 后一个球 
    [ReadOnly(true)]
    public float radius;
    [ReadOnly(true)]
    public int index;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BallChainConfig config;

    private void OnEnable()
    {
        radius = config.ZumaBallRadius;
    }
    public void SetColor(Color color)
    {
        ballColor = color;
        meshRenderer.material.color = color;
    }
    public void SetIndex(int index)
    {
        this.index = index;
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public void Activate(Vector3 position, Quaternion rotation)
    {
        transform.localScale = new Vector3(radius, radius, radius); 
        transform.SetPositionAndRotation(position, rotation);

        gameObject.SetActive(true);
    }
}
