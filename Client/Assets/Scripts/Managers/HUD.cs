using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
   
    public void OnClickPause() {
        GUIManager.Instance.ShowDialog(DialogName.Pause);
    }

    public void OnClickShorten() {
        GameManager.Instance.AddSize(-1);
    }

    public void OnClickExtend() {
        GameManager.Instance.AddSize(1);
    }

    public void OnClickPowerUp()
    {
        GameManager.Instance.PowerUp();
    }

    public void OnClickSlow()
    {
        GameManager.Instance.AddSpeed(-100);
    }
}
