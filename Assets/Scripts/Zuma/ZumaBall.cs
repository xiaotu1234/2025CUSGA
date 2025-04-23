using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zuma.Curve;

public class ZumaBall : MonoBehaviour
{
    public Color ballColor; // 球的颜色
    public GameObject previousBall = null; // 前一个球
    public GameObject nextBall = null; // 后一个球
    [HideInInspector] 
    public float radius;
}
