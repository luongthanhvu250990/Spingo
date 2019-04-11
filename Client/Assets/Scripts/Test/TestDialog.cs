using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestDialog : GUIBaseDialogHandler {
	public Text txt;
	public override void OnBeginShow (object parameter)
	{
		txt.text = "dshfgajdhgkajdgfj";
		base.OnBeginShow (parameter);
	}

	public void Close(){
//		GUIMessageDialog.Show ("fffffffffffffffffffffffff");
		GUIManager.Instance.HideDialog (DialogName.Test);
	}
}
