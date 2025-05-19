using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonMono<SceneManager>
{
    private bool isPaused = false;
    public GameObject retryUI;
    [SerializeField] private GameObject pauseMenuUI; // ��ͣ����

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
