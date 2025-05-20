using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUIInputDown : MonoBehaviour
{
    public UI_mainScene uI;
    public GameObject TechUI;
   
    

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            uI.ResumeGame();
            uI.TeachOver();
            EnemyManager.Instance.ShowAllEnemies();
            TechUI.SetActive(false);
        }
    }
}
