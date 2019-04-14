using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
   
    public void OnClickPause() {
        GUIManager.Instance.ShowDialog(DialogName.Pause);
    }

}
