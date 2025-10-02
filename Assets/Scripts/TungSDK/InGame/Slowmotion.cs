using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SlowMotion
{
    public static void DoSlowMotion(float timescale = 0.1f, float duration = 2)
    {
        Time.timeScale = timescale;
        Time.fixedDeltaTime = Time.timeScale * .01f;
        Thread.Sleep((int)(duration * 1000));
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.015f;
    }
}
