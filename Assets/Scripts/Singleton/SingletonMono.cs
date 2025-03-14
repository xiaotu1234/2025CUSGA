using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get 
        {
            //自动创建一个继承Monobehaviour单例模式泛型类，并保证过场景时不被移除
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
            return instance; 
        }
    }
}
