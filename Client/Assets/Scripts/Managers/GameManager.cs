using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    private void Start()
    {
        SoundManager.Instance.Play("Background");
    }
}
