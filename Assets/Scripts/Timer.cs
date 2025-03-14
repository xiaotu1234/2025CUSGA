using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timer = 0;

    public Timer(float time)
    {
        timer = time;
    }

    public bool TimingBuff()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
            return true; //µ½Ê±¼äÁË
        else
            return false;
    }

    public void SetTimer(float time)
    {
        timer = time;
    }

}