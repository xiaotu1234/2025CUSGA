using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Color ballColor; // �����ɫ
    public Ball PreviousBall = null; // ǰһ����
    public Ball NextBall = null; // ��һ���� 
    [ReadOnly(true)]
    public float radius;
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
    public void Deactivate()
    {
        
        gameObject.SetActive(false);
     
    }

    
    public void Activate(Vector3 position, Quaternion rotation)
    {
        transform.localScale = new Vector3(2*radius, 2*radius, 2 * radius); 
        transform.SetPositionAndRotation(position, rotation);

        gameObject.SetActive(true);
    }

    public void ResetState()
    {
        // �Ͽ���������
        PreviousBall = null;
        NextBall = null;
    }

}
