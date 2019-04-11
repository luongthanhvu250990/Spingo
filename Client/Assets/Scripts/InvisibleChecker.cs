using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleChecker : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float offSet = 10;
    
  
    void Update()
    {
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        if (screenPosition.x < offSet || screenPosition.x > (Screen.width - offSet) || screenPosition.y < offSet || screenPosition.y > (Screen.height - offSet))
            Debug.LogError("=========== " + screenPosition);
    }

}
