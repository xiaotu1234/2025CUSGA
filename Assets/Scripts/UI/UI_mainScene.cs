using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_mainScene : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera maincamera;
    public GameObject player;
    public Button start;
    private Animator camera;
    private Animator Aniplayer;
    void Start()
    {
      camera= maincamera.GetComponent<Animator>();
        Aniplayer = player.GetComponent<Animator>();
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        start.onClick.AddListener(StartActions);
    }
    void StartActions()
    {
        Debug.Log("start");
        camera.SetTrigger("start");
        Aniplayer.SetBool("isStart", true);
        Destroy(start);
        ResumeGame();
    }
    void PauseGame()
    {
        // 将时间缩放设置为0，游戏暂停
        Time.timeScale = 0f;

    }

    // 继续游戏的方法
    void ResumeGame()
    {
        // 将时间缩放设置为1，游戏继续
        Time.timeScale = 1f;

    }
}
