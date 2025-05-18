using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonMono<SceneManager>
{
    private bool isPaused = false;
    public GameObject retryUI;
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
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �ڱ༭����ֹͣ����
#else
        Application.Quit(); // �����汾���˳���Ϸ
#endif
    }
}
