using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonMono<SceneManager>
{
    private bool isPaused = false;
    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f; // 暂停游戏时间
            Cursor.lockState = CursorLockMode.None; // 解锁鼠标
            Cursor.visible = true; // 显示鼠标
            isPaused = true;
        }
    }
    public void StartGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}
