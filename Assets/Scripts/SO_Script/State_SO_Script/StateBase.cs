using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase : ScriptableObject
{
    //���ö������
    protected Animator animtor;
    protected string animBoolName;

    public StateBase(Animator _animtor, string _animBoolName)
    {
        this.animtor = _animtor;
        this.animBoolName = _animBoolName;
    }

    public virtual void OnEnter()
    {
        animtor.SetBool(animBoolName, true);
    }
    public abstract void OnUpdate();
    public virtual void OnExit()
    {
        animtor.SetBool(animBoolName, false);
    }
}
