using UnityEngine;

public class InvisibleWithShadow : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Start()
    {
        // ��ȡMeshRenderer���
        meshRenderer = GetComponent<MeshRenderer>();
        // ������Ⱦ
        meshRenderer.enabled = false;
        // ȷ������Ͷ����Ӱ
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        meshRenderer.receiveShadows = false;
    }
}