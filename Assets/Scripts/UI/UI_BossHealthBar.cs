
using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealthBar : MonoBehaviour
{
    [Header("Ѫ������")]
    [SerializeField] private Slider healthSlider;        // �󶨵�Slider���
    [SerializeField] private float healthInPhase3;       // ���׶�Ѫ��


    [Header("�ָ�������")]
    [SerializeField] private GameObject dividerPrefab; // �ָ���Ԥ����
    [SerializeField] private Transform dividersContainer; // �ָ��߸�����

    //[Header("�Ӿ�Ч��")]
    //[SerializeField] private Gradient healthGradient;    // Ѫ����ɫ����
    
    private int maxHealthSegments = 10; // ��Ѫ������
    private Image[] _segmentDividers;    // �ֶηָ���ͼƬ����
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
        Debug.Log("Boss��Ѫ������: " + maxHealthSegments);
        _currentSegments = maxHealthSegments;
        Debug.LogWarning("_currentSegments: " + _currentSegments);
        // ����Slider���
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealthSegments;
        healthSlider.wholeNumbers = true; // ��������ֵ
        _maxHealth = _bossManager.healthInPhase3;
        _currentHealth = _maxHealth;
        // ��ȡ���ͼƬ����
        _fillImage = healthSlider.fillRect.GetComponent<Image>();

        // ��ʼ���ָ���
        //UpdateSegmentDividers();

        // ����Ѫ��
        UpdateHealthVisual();
    }

    // �ⲿ�����˺��ķ���
    public void TakeDamage(int damage)
    {
        //Debug.LogWarning($"Boss��ѪǰѪ������: {_currentSegments}, _boss1.tentacles.Count: {_boss1.tentacles.Count}" +
        //    $"�˺���{damage}, �������ֵ{maxHealthSegments}");
        _currentSegments = _boss1.tentacles.Count;
        UpdateHealthVisual();
        Debug.LogWarning($"Boss��ǰѪ������: {_currentSegments}, _boss1.tentacles.Count: {_boss1.tentacles.Count}" +
            $"_boss1.tentaclesnum: {_boss1.tentaclesnum}");
        if (_currentSegments <= 0 && _currentPhase == 2)
        {
            try
            {
                HandleDeathPhase2();
            }
            catch (System.Exception)
            {
                Debug.LogError("Boss���׶���������");
            }
        }
    }

    void UpdateHealthVisual()
    {
        // ����Slider��ֵ
        if(_currentPhase == 2)
            healthSlider.value = _currentSegments;
        if (_currentPhase == 3)
        {
            Debug.Log("UI�������׶�");
            healthSlider.value = _currentHealth;
        }

        //// ������ɫ����
        //float healthPercent = (float)_currentSegments / maxHealthSegments;
        //_fillImage.color = healthGradient.Evaluate(healthPercent);
    }

    // ���·ָ��߿ɼ���
    //void UpdateSegmentDividers()
    //{
    //    float spacing = 1f / maxHealthSegments;

    //    for (int i = 0; i < _segmentDividers.Length; i++)
    //    {
    //        if (i < maxHealthSegments - 1)
    //        {
    //            // ����ָ���λ��
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
        // ���������¼�
        Debug.Log("Boss���׶��ѱ�����");
        Debug.LogWarning($"Boss���׶�Info:  " +
            $"_maxHealth: {healthSlider.maxValue}\n" +
            $"_currentHealth: {healthSlider.value}");
    }

    void HandleDeathPhase3()
    {
        
        Debug.Log("Boss���׶��ѱ�����");
    }


    public void TakeDamageByZuma(float damage)
    {
        Debug.Log($"����˺�ǰ��Ѫ��: " + _currentHealth);
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        UpdateHealthVisual();
        Debug.LogWarning($"TakeDamageByZuma: {damage}");
        Debug.LogWarning($"Boss��ǰѪ��: {healthSlider.value}, _maxHealth: {healthSlider.maxValue}");
        if (_currentHealth <= 0 && _currentPhase == 3)
        {
            try
            {
                HandleDeathPhase3();
            }
            catch (System.Exception)
            {
                Debug.LogError("Boss���׶���������");
            }
        }
        
    }

    // �༭���ı���ֵʱ�Զ�����
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
    //    // ��������洢�ָ�������
    //    _segmentDividers = new Image[maxHealthSegments - 1];

    //    // ����ÿ���ָ���
    //    for (int i = 0; i < maxHealthSegments - 1; i++)
    //    {
    //        GameObject divider = Instantiate(dividerPrefab, dividersContainer);
    //        divider.name = $"Divider_{i + 1}";

    //        // ���÷ָ�����ʽ
    //        Image dividerImage = divider.GetComponent<Image>();
    //        dividerImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f); // ���ɫ��͸��

    //        // �洢����
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