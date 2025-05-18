using JetBrains.Rider.Unity.Editor;
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
    public Rigidbody Rigidbody;
    public BallProvider pool;
    [SerializeField] private float radius;
    [SerializeField] private Animator animator;
    [SerializeField] private MeshRenderer meshRender;
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private BallChainConfig config;

    
    private void OnEnable()
    {
        radius = config.ZumaBallRadius;
    }
    public void SetColor(Color color)
    {
        gameObject.GetComponent<TrailRenderer>().colorGradient = new Gradient() { 
            colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0f)
            }, alphaKeys = gameObject.GetComponent<TrailRenderer>().colorGradient.alphaKeys };
        ballColor = color;
        if (meshRender != null) 
            meshRender.material.color = color;
        if (spriteRender != null)
        {

            spriteRender.color = color;
            

        }


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
    public void SetLayer(int layer)
    {
        this.gameObject.layer = layer;
    }
    public void ResetState()
    {
        // �Ͽ���������
        PreviousBall = null;
        NextBall = null;
    }

    public void PlayDestroyAnimation(System.Action OnComplete)
    {

        if (animator != null)
        {
            //�����߼�
           
            OnComplete?.Invoke();
        }
        else
        {
            OnComplete?.Invoke();
        }
        
    }

    public void ReturnBall()
    {
        pool.ReturnBall(this);
    }

}
