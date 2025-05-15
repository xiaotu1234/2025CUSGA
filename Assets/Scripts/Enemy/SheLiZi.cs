using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheLiZi : MonoBehaviour

{
    public int healValue = 1;
    [SerializeField] private Animator animator;
    private PlayerController _player;
    private void Start()
    {
        _player = PlayerManager.Instance.player;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;


        if (animator != null)
        {
            //������ʧ����
        }

        DestorySelf();

    }

    private void DestorySelf()
    {
        _player.AddCurrentHealth(healValue);//��Ҽ�Ѫ
        Destroy(gameObject);
    }
}
