using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHyperBullet : PlayerBulletBase
{
    #region Bullet Settings 子弹设置
    //public float speed = 30f;
    public float globalFixedHeight = 2f; // 全局固定高度
    public float lifeTime = 10f;
    private bool isInChain = false;
    [SerializeField] private Ball ball;
    private BallChainController _ballController;
    #endregion
    private Coroutine Destory;
    public static event Action OnNotMatch;
    private void Start()
    {
        
        
    }

    

    private void OnEnable()
    {
        Destory = StartCoroutine(ReturnBallWithDelay(ball, lifeTime));
        _ballController = BallChainController.Instance;
        PlayerManager.Instance.player.OnColorChanged += DragTail;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.player.OnColorChanged -= DragTail;
    }

    private void DragTail(Color color)
    {
        gameObject.GetComponent<TrailRenderer>().colorGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0f)
            },
            alphaKeys = gameObject.GetComponent<TrailRenderer>().colorGradient.alphaKeys
        };
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (isInChain)
            return;

        // 忽略玩家碰撞
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject);
            if (other.gameObject.GetComponent<Enitity>() != null)
                other.gameObject.GetComponent<Enitity>().TakeDamage(damage);
            if (other.gameObject.GetComponent<EnemyController>() != null)
                other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }

        if (other.gameObject.CompareTag("ZumaBall"))
        {
            if(_ballController == null || ball == null)
            Debug.LogWarning($"_ballController is null?: {_ballController == null}, ball is null?: {ball == null}");
            else { 
                if (!_ballController.TryAttachBall(ball))
                {
                    Debug.Log("尝试消除");
                    StopCoroutine(Destory);
                    ball.PlayDestroyAnimation(() =>
                    {
                        ball.ReturnBall();
                        OnNotMatch?.Invoke();
                    });

                }
            }

        }
        

    }
    

    private IEnumerator ReturnBallWithDelay(Ball ball, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        ball.ReturnBall();
    }
}
