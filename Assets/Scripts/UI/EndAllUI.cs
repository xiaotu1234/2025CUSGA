using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAllUI : MonoBehaviour
{
    public List<GameObject> needCloseUIList;

    private void OnEnable()
    {
        BossManager.Instance.OnCloseUI += EndActivateUI;
    }

    private void OnDisable()
    {
        BossManager.Instance.OnCloseUI -= EndActivateUI;
    }


    private void EndActivateUI()
    {
        foreach (var UI in needCloseUIList)
        {
            UI.SetActive(false);
        }
    }
}
