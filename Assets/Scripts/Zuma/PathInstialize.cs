using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathInstialize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerController player = PlayerManager.Instance.player;
        float y = player.firePoint.transform.position.y;
        transform.position = new Vector3(transform.position.x,y,
            transform.position.z);
    }

    
}
