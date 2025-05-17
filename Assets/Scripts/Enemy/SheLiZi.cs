using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheLiZi : MonoBehaviour

{
    public int healValue = 1;
    [SerializeField] private float _lifeTime = 10;
    [SerializeField] private Animator animator;
    private PlayerController _player;
    private Coroutine CorDestory;
    private void Start()
    {
        _player = PlayerManager.Instance.player;
        CorDestory = StartCoroutine(DestoryWithoutCollison());
    }

    //private void OnEnable()
    //{
    //    if(!Application.isPlaying)
    //    {
    //        DestorySelf();
    //    }
    //}
    private  IEnumerator DestoryWithoutCollison()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        StopCoroutine(DestoryWithoutCollison());
        if (animator != null)
        {
            //������ʧ����
            _player.AddCurrentHealth(healValue);//��Ҽ�Ѫ
        }else
        {
            DestorySelf();
            _player.AddCurrentHealth(healValue);//��Ҽ�Ѫ
        }

        

    }

    private void DestorySelf()
    {
        Destroy(gameObject);
    }
}
