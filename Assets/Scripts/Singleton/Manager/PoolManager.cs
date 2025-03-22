using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : BaseManager<PoolManager> 
{
    private Dictionary<string, Stack<GameObject>> poolDic = new Dictionary<string, Stack<GameObject>>();
    //（若无对象）从Resources文件夹中读取对象资源并放入缓存池
    //（若有对象）从缓存池中获取对象
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

    //将对象压入对应名字的缓存池内
    public void PushObject(string name, GameObject obj)
    {
        obj.SetActive(false);

        if (!poolDic.ContainsKey(name)) 
            poolDic.Add(name, new Stack<GameObject>());
        poolDic[name].Push(obj);
    }
    //清空缓存池  主要用于过场景时使用，并搭配GC回收
    public void Clear()
    {
        poolDic.Clear();
    }

}
