
using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealthBar : MonoBehaviour
{
    [Header("血条参数")]
    [SerializeField] private Slider healthSlider;        // 绑定的Slider组件
    [SerializeField] private float healthInPhase3;       // 三阶段血量


    [Header("分隔线设置")]
    [SerializeField] private GameObject dividerPrefab; // 分隔线预制体
    [SerializeField] private Transform dividersContainer; // 分隔线父对象

    //[Header("视觉效果")]
    //[SerializeField] private Gradient healthGradient;    // 血条颜色渐变
    
    private int maxHealthSegments = 10; // 总血量段数
    private Image[] _segmentDividers;    // 分段分隔线图片数组
    private int _currentSegments;
    private float _currentHealth;
    private Image _fillImage;
    private Boss_1_Controller _boss1;
    private BossManager _bossManager;
    private int _currentPhase = 2;
    private float _maxHealth;

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
        _bossManager.OnTakeDamageByZuma -= TakeDamageByZuma;
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

        _bossManager = BossManager.Instance;
        _boss1 = BossManager.Instance.boss_1;
        maxHealthSegments = _boss1.tentacles.Count;
        Debug.Log("Boss的血量段数: " + maxHealthSegments);
        _currentSegments = maxHealthSegments;
        Debug.LogWarning("_currentSegments: " + _currentSegments);
        // 配置Slider组件
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealthSegments;
        healthSlider.wholeNumbers = true; // 启用整数值
        _maxHealth = _bossManager.healthInPhase3;
        _currentHealth = _maxHealth;
        // 获取填充图片引用
        _fillImage = healthSlider.fillRect.GetComponent<Image>();

        // 初始化分隔线
        //UpdateSegmentDividers();

        // 重置血量
        UpdateHealthVisual();
    }

    // 外部调用伤害的方法
    public void TakeDamage(int damage)
    {
        //Debug.LogWarning($"Boss扣血前血量段数: {_currentSegments}, _boss1.tentacles.Count: {_boss1.tentacles.Count}" +
        //    $"伤害量{damage}, 最大生命值{maxHealthSegments}");
        _currentSegments = _boss1.tentacles.Count;
        UpdateHealthVisual();
        Debug.LogWarning($"Boss当前血量段数: {_currentSegments}, _boss1.tentacles.Count: {_boss1.tentacles.Count}" +
            $"_boss1.tentaclesnum: {_boss1.tentaclesnum}");
        if (_currentSegments <= 0 && _currentPhase == 2)
        {
            try
            {
                HandleDeathPhase2();
            }
            catch (System.Exception)
            {
                Debug.LogError("Boss二阶段死亡出错");
            }
        }
    }

    void UpdateHealthVisual()
    {
        // 更新Slider数值
        if(_currentPhase == 2)
            healthSlider.value = _currentSegments;
        if (_currentPhase == 3)
        {
            Debug.Log("UI进入三阶段");
            healthSlider.value = _currentHealth;
        }

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

    void HandleDeathPhase2()
    {
        _currentPhase = 3;
        healthSlider.wholeNumbers = false;
        healthSlider.maxValue = _maxHealth;
        _currentHealth = healthSlider.maxValue;
        healthSlider.value = _currentHealth;
        _bossManager.OnTakeDamageByZuma += TakeDamageByZuma;
        // 触发死亡事件
        Debug.Log("Boss二阶段已被击败");
        Debug.LogWarning($"Boss三阶段Info:  " +
            $"_maxHealth: {healthSlider.maxValue}\n" +
            $"_currentHealth: {healthSlider.value}");
    }

    void HandleDeathPhase3()
    {
        
        Debug.Log("Boss三阶段已被击败");
    }


    public void TakeDamageByZuma(float damage)
    {
        Debug.Log($"造成伤害前的血量: " + _currentHealth);
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        UpdateHealthVisual();
        Debug.LogWarning($"TakeDamageByZuma: {damage}");
        Debug.LogWarning($"Boss当前血量: {healthSlider.value}, _maxHealth: {healthSlider.maxValue}");
        if (_currentHealth <= 0 && _currentPhase == 3)
        {
            try
            {
                HandleDeathPhase3();
            }
            catch (System.Exception)
            {
                Debug.LogError("Boss三阶段死亡出错");
            }
        }
        
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