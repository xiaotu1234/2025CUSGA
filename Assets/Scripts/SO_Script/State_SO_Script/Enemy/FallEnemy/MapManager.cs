using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonMono<MapManager>
{
    PlayerController player;
    public GameObject mapCamera;
    public float mapHeight;
    
    void Start()
    {
        player = PlayerManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        MapFollowPlayer();
    }

    private void MapFollowPlayer()
    {
        mapCamera.transform.position = player.transform.position + new Vector3(0, mapHeight, 0);
    }
}
