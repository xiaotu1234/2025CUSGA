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
            Time.timeScale = 0f; // ��ͣ��Ϸʱ��
            Cursor.lockState = CursorLockMode.None; // �������
            Cursor.visible = true; // ��ʾ���
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
