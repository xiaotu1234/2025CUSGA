using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zuma.Curve;

public class ZumaBall : MonoBehaviour
{
    public Color ballColor; // �����ɫ
    public GameObject previousBall = null; // ǰһ����
    public GameObject nextBall = null; // ��һ����
    [HideInInspector] 
    public float radius;
}
