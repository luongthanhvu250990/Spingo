    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSelectDialog : GUIBaseDialogHandler {	
	public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
	}

    public void OnClickMissionItem() {
        GUIManager.Instance.ShowDialog(DialogName.MissionDetail);
    }
}
