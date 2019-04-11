using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger: MonoBehaviour
{
    protected abstract void OnDotTrigger();
    protected abstract void OnPaddleTrigger();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Dot")
            OnDotTrigger();

        if (other.gameObject.tag == "Paddle")
            OnPaddleTrigger();

        Debug.LogError(other.name);
    }
}
