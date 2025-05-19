using UnityEngine;
using Spine.Unity;

public class TestSpine : MonoBehaviour
{
    SkeletonAnimation ska;
    void Start()
    {
        ska = gameObject.GetComponent<SkeletonAnimation>();
        ska.timeScale = 1f;
        ska.loop = true;
        ska.AnimationName = "idle";
        //���� 1.�㼶, 2.Ҫ���ŵĶ����� , 3.�Ƿ�ѭ��        
        ska?.AnimationState.SetAnimation(0, "idle", true);
    }
}