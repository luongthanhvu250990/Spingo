    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapDialog : GUIBaseDialogHandler {	
	public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
	}

    public void OnClickStageButton() {
        GUIManager.Instance.ShowDialog(DialogName.StageStart);
    }
}
