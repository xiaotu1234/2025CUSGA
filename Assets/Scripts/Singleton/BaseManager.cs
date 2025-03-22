using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T> where T : class,new()
{
    private static T instance;
    public static T Instance
    {
        //不继承Monobehaviour的单例基类
        get
        {
            if (instance == null) 
                instance = new T();
            return instance;
        }
    }
}
