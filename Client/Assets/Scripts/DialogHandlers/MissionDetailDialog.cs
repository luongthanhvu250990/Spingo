    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionDetailDialog : GUIBaseDialogHandler {	
	public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
	}

    public void OnClickPlay()
    {
        GameManager.Instance.StartNewGame(GameMode.Endless);        
    }
}
