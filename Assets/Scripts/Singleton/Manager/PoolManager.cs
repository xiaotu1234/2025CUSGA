using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : BaseManager<PoolManager> 
{
    private Dictionary<string, Stack<GameObject>> poolDic = new Dictionary<string, Stack<GameObject>>();
    //�����޶��󣩴�Resources�ļ����ж�ȡ������Դ�����뻺���
    //�����ж��󣩴ӻ�����л�ȡ����
    public GameObject GetObject(string name)
    {
        GameObject obj;
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            obj = poolDic[name].Pop();
            obj.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        }
        return obj;
    }

    //������ѹ���Ӧ���ֵĻ������
    public void PushObject(string name, GameObject obj)
    {
        obj.SetActive(false);

        if (!poolDic.ContainsKey(name)) 
            poolDic.Add(name, new Stack<GameObject>());
        poolDic[name].Push(obj);
    }
    //��ջ����  ��Ҫ���ڹ�����ʱʹ�ã�������GC����
    public void Clear()
    {
        poolDic.Clear();
    }

}
