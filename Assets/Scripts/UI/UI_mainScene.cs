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
        // ��ʱ����������Ϊ0����Ϸ��ͣ
        Time.timeScale = 0f;

    }

    // ������Ϸ�ķ���
    void ResumeGame()
    {
        // ��ʱ����������Ϊ1����Ϸ����
        Time.timeScale = 1f;

    }
}
