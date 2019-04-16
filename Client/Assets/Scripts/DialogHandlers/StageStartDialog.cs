    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageStartDialog : GUIBaseDialogHandler {	
	public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
	}

	public void OnClickPlay(){
        GameManager.Instance.StartNewGame(GameMode.Story);
    }
}
