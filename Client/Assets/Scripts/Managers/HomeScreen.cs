using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreen : MonoBehaviour
{
    public void OnClickStories() {
        GUIManager.Instance.ShowDialog(DialogName.Map);       
    }

    public void OnClickEndlessMode() {
        GUIManager.Instance.ShowDialog(DialogName.MissionSelect);
    }
}
