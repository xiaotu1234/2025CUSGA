using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFaceColor : MonoBehaviour
{
    public SpriteRenderer _renderer;

    


    private void OnEnable()
    {
        PlayerManager.Instance.player.OnColorChanged += SetColor;
    }
    private void OnDisable()
    {
        PlayerManager.Instance.player.OnColorChanged -= SetColor;

    }

    private void SetColor(Color color)
    {
        _renderer.color = color;
        Debug.Log($"设置颜色成功color: {color}");
    }
}
