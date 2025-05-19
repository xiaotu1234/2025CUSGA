using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_mainScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject finishUI;
    public GameObject PauseUI;
    public Camera maincamera;
    public GameObject player;
    public Button start;
    public Button start2;
    private Animator camera;
    private Animator Aniplayer;
    void Start()
    {
      camera= maincamera.GetComponent<Animator>();
        Aniplayer = player.GetComponent<Animator>();
        PauseGame();
    }

    private void OnEnable()
    {
        BossManager.Instance.OnBossDie += ShowFinishUI;
        SceneManager.OnPauseGame += ShowPauseUI;
        SceneManager.OnResumeGame += ClosePauseUI;
    }

    

    private void OnDisable()
    {
        BossManager.Instance.OnBossDie -= ShowFinishUI;
        SceneManager.OnPauseGame -= ShowPauseUI;
        SceneManager.OnResumeGame -= ClosePauseUI;
    }

    // Update is called once per frame
    void Update()
    {
        start.onClick.AddListener(StartActions);
        start2.onClick.AddListener(Start2Actions);
    }
    void StartActions()
    {
        camera.SetTrigger("start");
        Aniplayer.SetBool("isStart", true);
        Destroy(start);
        PauseGamestart();


    }
    void Start2Actions()
    {
      ResumeGame();
        AudioManager.Instance.PlayBGM(1);
    }
    void PauseGame()
    {
        Time.timeScale = 0f;
       // StartCoroutine(PauseAfterDelay(1f));
    }
    void PauseGamestart()
    {
        // 将时间缩放设置为0，游戏暂停
        Time.timeScale = 1f;

        // 启动协程，2秒后暂停游戏
        StartCoroutine(PauseAfterDelay(0.8f));
    }

    // 继续游戏的方法
    public void ResumeGame()
    {
        // 恢复游戏
        Time.timeScale = 1f;

      
     
    }

    private void ShowPauseUI()
    {
        if (PauseUI == null)
        {
            Debug.LogError("UImgr未配置PauseUI");
            return;
        }

        PauseUI.SetActive(true);
    }

    private void ClosePauseUI()
    {
        if (PauseUI == null)
        {
            Debug.LogError("UImgr未配置PauseUI");
            return;
        }

        PauseUI.SetActive(false);
    }


    private void ShowFinishUI()
    {
        if (finishUI == null)
        {
            Debug.LogError("UImgr未配置FinishUI");
            return;
        }

        finishUI.SetActive(true);
    }

    IEnumerator PauseAfterDelay(float delay)
    {
        // 等待 delay 秒
        yield return new WaitForSecondsRealtime(delay); // 使用 Realtime 不受 Time.timeScale 影响
        Time.timeScale = 0f;
    }
}
