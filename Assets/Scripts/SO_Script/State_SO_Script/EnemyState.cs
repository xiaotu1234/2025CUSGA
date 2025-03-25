using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class EnemyState : StateBase
{
    protected GameObject enemy;
    protected abstract void Awake();
}
