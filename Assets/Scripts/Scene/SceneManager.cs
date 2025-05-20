using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonMono<SceneManager>
{
    public static event Action OnPauseGame;
    public static event Action OnResumeGame;
    private bool isPaused = false;
    public GameObject retryUI;
    [SerializeField] private GameObject pauseMenuUI; // ��ͣ����
    public GameObject dieimg;
    public GameObject dieui;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ResumeGame()
    {

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
        OnResumeGame?.Invoke();

    }
    public void PauseGame()
    {
        if (!isPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f; // ��ͣ��Ϸʱ��
            Cursor.lockState = CursorLockMode.None; // �������
            Cursor.visible = true; // ��ʾ���
            isPaused = true;
            OnPauseGame?.Invoke(); // ������ͣ�¼�
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
