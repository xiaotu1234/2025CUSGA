using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoss : MonoBehaviour
{
    public BossManager BossManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            BossManager.EndPhase3();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            BossManager.EndPhase2();
        }
    }
}
