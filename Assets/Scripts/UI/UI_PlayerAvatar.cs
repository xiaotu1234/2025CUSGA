using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerAvatar : MonoBehaviour
{
    private PlayerController player;
    private SpriteRenderer sr;
    void Start()
    {
        player = PlayerManager.Instance.player;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpriteColor();
    }

    private void UpdateSpriteColor()
    {
        if(player.skill == null) return;
        if (player.skill is HyperShot hyperShot)
        {
            if (hyperShot._color != sr.color)
            {
                sr.color = hyperShot._color;
            }
            Debug.Log("sr" + sr.color + "h" + hyperShot._color);
        }

    }
}
