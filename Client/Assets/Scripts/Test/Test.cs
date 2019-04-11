using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	public void ShowMessageBox(){
		GUIMessageDialog.Show (res => {
			if(res == MessageBox.DialogResult.Ok){
				GUIManager.Instance.ShowDialog (DialogName.Test);
			}
			return true;
		}, "aaaaaaaaaaaaaaaaaaaa", "dafadf", MessageBox.Buttons.OKCancel);
//		GUIManager.Instance.ShowDialog (DialogName.Test);
	}
}
