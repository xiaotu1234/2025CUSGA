using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHyperBullet : PlayerBulletBase
{
    #region Bullet Settings �ӵ�����
    //public float speed = 30f;
    public float globalFixedHeight = 2f; // ȫ�̶ֹ��߶�
    public float lifeTime = 10f;
    [SerializeField] private Ball ball;
    private BallChainController _ballController;
    #endregion
    private Coroutine Destory;

    private void Start()
    {
    }

    private void OnEnable()
    {
        Debug.Log("�����active");
        _ballController = BallChainController.Instance;
        Debug.Log($"_ballController �Ƿ�Ϊ��{_ballController == null}.");
        AttachingBallChainHandler.OnInChain += InChain;
        Destory = StartCoroutine(ReturnBallWithDelay(ball, lifeTime));

    }

    private void OnDisable()
    {
        AttachingBallChainHandler.OnInChain -= InChain;
    }


   
    private void OnTriggerEnter(Collider other)
    {
        

        // ���������ײ
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
            Debug.Log("���Բ���");
            if (ball == null)
                Debug.LogError("ball == null");
            //else if (_ballController == null)
            //    Debug.LogError("_ballController == null");
            else 
            { 
                _ballController = BallChainController.Instance;
                _ballController.TryAttachBall(ball);
            }
        }

        

    }
    private void InChain(Ball inputBall)
    {
        if (inputBall == ball)
        {
            StopCoroutine(Destory);
            tag = "ZumaBall";
            inputBall.SetLayer("Bullet_Player");
        }
    }

    private IEnumerator ReturnBallWithDelay(Ball ball, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        _ballController.ReturnBall(ball);
    }
}
