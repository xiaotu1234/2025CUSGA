using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDie", menuName = "ScriptableObject/Player/PlayerDie", order = 0)]
public class PlayerDie : PlayerState
{
    private PlayerController player;
    public override void OnEnter()
    {
        player = PlayerManager.Instance.player;
        player.gameObject.GetComponent<PlayerShooting>().enabled = false;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
