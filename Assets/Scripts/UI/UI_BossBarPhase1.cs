
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedBossHealth : MonoBehaviour
{
    [Header("血条参数")]
    [SerializeField] private int maxHealthSegments = 10; // 总血量段数
    [SerializeField] private Slider healthSlider;        // 绑定的Slider组件


    [Header("分隔线设置")]
    [SerializeField] private GameObject dividerPrefab; // 分隔线预制体
    [SerializeField] private Transform dividersContainer; // 分隔线父对象

    //[Header("视觉效果")]
    //[SerializeField] private Gradient healthGradient;    // 血条颜色渐变
    
    private Image[] _segmentDividers;    // 分段分隔线图片数组
    private int _currentSegments;
    private Image _fillImage;
    private Boss_1_Controller _boss1;

    void OnEnable()
    {
        InitializeHealthSystem();
        _boss1.OnTentacleDie += TakeDamage;
        ////Test
        //StartCoroutine(TestBar());

    }

    private void OnDisable()
    {
        _boss1.OnTentacleDie -= TakeDamage;
    }

    private IEnumerator TestBar()
    {
        yield return new WaitForSeconds(1f);
        TakeDamage(1);
        yield return new WaitForSeconds(1f);
        TakeDamage(1);
        yield return new WaitForSeconds(1f);
        TakeDamage(1);
    }



    void InitializeHealthSystem()
    {

        //ClearExistingDividers();
        //GenerateDividers();


        _boss1 = BossManager.Instance.boss_1;
        maxHealthSegments = _boss1.tentacles.Count;
        Debug.Log("Boss的血量段数: " + maxHealthSegments);
        // 配置Slider组件
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealthSegments;
        healthSlider.wholeNumbers = true; // 启用整数值

        // 获取填充图片引用
        _fillImage = healthSlider.fillRect.GetComponent<Image>();

        // 初始化分隔线
        //UpdateSegmentDividers();

        // 重置血量
        _currentSegments = maxHealthSegments;
        UpdateHealthVisual();
    }

    // 外部调用伤害的方法
    public void TakeDamage(int damage)
    {
        _currentSegments = Mathf.Clamp(_currentSegments - damage, 0, maxHealthSegments);
        UpdateHealthVisual();

        if (_currentSegments <= 0)
        {
            HandleDeath();
        }
    }

    void UpdateHealthVisual()
    {
        // 更新Slider数值
        healthSlider.value = _currentSegments;

        //// 更新颜色渐变
        //float healthPercent = (float)_currentSegments / maxHealthSegments;
        //_fillImage.color = healthGradient.Evaluate(healthPercent);
    }

    // 更新分隔线可见性
    //void UpdateSegmentDividers()
    //{
    //    float spacing = 1f / maxHealthSegments;

    //    for (int i = 0; i < _segmentDividers.Length; i++)
    //    {
    //        if (i < maxHealthSegments - 1)
    //        {
    //            // 计算分隔线位置
    //            RectTransform dividerRT = _segmentDividers[i].GetComponent<RectTransform>();
    //            dividerRT.anchorMin = new Vector2((i + 1) * spacing, 0);
    //            dividerRT.anchorMax = new Vector2((i + 1) * spacing + 0.005f, 1);
    //            _segmentDividers[i].gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            //_segmentDividers[i].gameObject.SetActive(false);
    //        }
    //    }
    //}

    void HandleDeath()
    {
        // 触发死亡事件
        Debug.Log("Boss已被击败");
    }

    // 编辑器改变数值时自动更新
    void OnValidate()
    {
        if (Application.isPlaying && healthSlider != null)
        {
            //UpdateSegmentDividers();
            UpdateHealthVisual();
        }
    }
    //void GenerateDividers()
    //{
    //    // 创建数组存储分隔线引用
    //    _segmentDividers = new Image[maxHealthSegments - 1];

    //    // 生成每个分隔线
    //    for (int i = 0; i < maxHealthSegments - 1; i++)
    //    {
    //        GameObject divider = Instantiate(dividerPrefab, dividersContainer);
    //        divider.name = $"Divider_{i + 1}";

    //        // 配置分隔线样式
    //        Image dividerImage = divider.GetComponent<Image>();
    //        dividerImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f); // 深灰色半透明

    //        // 存储引用
    //        _segmentDividers[i] = dividerImage;
    //    }

    //    //UpdateSegmentDividers();
    //}

    //void ClearExistingDividers()
    //{
    //    foreach (Transform child in dividersContainer)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //}

}