﻿    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopDialog : GUIBaseDialogHandler {	
	public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
	}

	public void Close(){
		GUIManager.Instance.HideDialog (DialogName.Shop);
	}
}
