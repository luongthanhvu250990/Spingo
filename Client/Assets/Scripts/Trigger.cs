using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger: MonoBehaviour
{
    protected abstract void OnDotTrigger();
    protected abstract void OnPaddleTrigger();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Head")
            OnDotTrigger();

        if (other.gameObject.tag == "Bar")
            OnPaddleTrigger();

        Debug.LogError(other.name);
    }
}
