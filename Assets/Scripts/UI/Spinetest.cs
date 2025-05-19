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
        //参数 1.层级, 2.要播放的动画名 , 3.是否循环        
        ska?.AnimationState.SetAnimation(0, "idle", true);
    }
}