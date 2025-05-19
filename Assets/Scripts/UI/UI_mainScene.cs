using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_mainScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject finishUI;
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
        BossManager.Instance.OnBossDie += showFinishUI;
    }

    private void OnDisable()
    {
        BossManager.Instance.OnBossDie -= showFinishUI;
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
        // ��ʱ����������Ϊ0����Ϸ��ͣ
        Time.timeScale = 1f;

        // ����Э�̣�2�����ͣ��Ϸ
        StartCoroutine(PauseAfterDelay(0.8f));
    }

    // ������Ϸ�ķ���
    public void ResumeGame()
    {
        // �ָ���Ϸ
        Time.timeScale = 1f;

      
     
    }

    private void showFinishUI()
    {
        if (finishUI == null)
        {
            Debug.LogError("UImgrδ����FinishUI");
            return;
        }

        finishUI.SetActive(true);
    }

    IEnumerator PauseAfterDelay(float delay)
    {
        // �ȴ� delay ��
        yield return new WaitForSecondsRealtime(delay); // ʹ�� Realtime ���� Time.timeScale Ӱ��
        Time.timeScale = 0f;
    }
}
