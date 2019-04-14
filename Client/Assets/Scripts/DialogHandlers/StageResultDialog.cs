    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageResultDialog : GUIBaseDialogHandler {	
	public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
	}

	public void Close(){
		GUIManager.Instance.HideDialog (DialogName.StageResult);
	}
}
