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
            //播放消失动画
        }

        _player.AddCurrentHealth(healValue);//玩家加血


    }
}
