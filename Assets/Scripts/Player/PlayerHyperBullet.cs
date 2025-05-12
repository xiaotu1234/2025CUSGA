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
    private void Start()
    {
        _ballController = BallChainController.Instance;

        
    }

    private void OnEnable()
    {
        AttachingBallChainHandler.OnInChain += InChain;
        Destory = StartCoroutine(ReturnBallWithDelay(ball, lifeTime));

    }

    private void OnDisable()
    {
        AttachingBallChainHandler.OnInChain -= InChain;
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
        }

        if (other.gameObject.CompareTag("ZumaBall"))
        {
            _ballController.TryAttachBall(ball);
        }

        

    }
    private void InChain(Ball inputBall)
    {
        if (inputBall == ball)
        {
            StopCoroutine(Destory);
            isInChain = true;
            tag = "ZumaBall";
        }
    }

    private IEnumerator ReturnBallWithDelay(Ball ball, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        ball.ReturnBall();
    }
}
