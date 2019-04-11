using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Trigger
{

    protected override void OnDotTrigger()
    {
        Debug.LogError("=========Game Over=========");
    }

    protected override void OnPaddleTrigger()
    {
        Debug.LogError("Score " + Time.realtimeSinceStartup);
    }
}
