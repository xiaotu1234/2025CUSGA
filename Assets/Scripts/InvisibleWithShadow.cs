using UnityEngine;

public class InvisibleWithShadow : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Start()
    {
        // 获取MeshRenderer组件
        meshRenderer = GetComponent<MeshRenderer>();
        // 禁用渲染
        meshRenderer.enabled = false;
        // 确保物体投射阴影
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        meshRenderer.receiveShadows = false;
    }
}