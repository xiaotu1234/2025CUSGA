using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonMono<SceneManager>
{
    private bool isPaused = false;
    public GameObject retryUI;
    [SerializeField] private GameObject pauseMenuUI; // 暂停界面

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
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止运行
#else
        Application.Quit(); // 发布版本中退出游戏
#endif
    }
}
