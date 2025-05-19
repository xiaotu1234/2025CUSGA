using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugZumaDie : MonoBehaviour
{
    public BossManager BossManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            BossManager.GoPhase3();
        }
    }
}
