using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    private void Start()
    {
        GUIManager.Instance.ShowDialog(DialogName.MissionResult);
    }
    public void ShowMessageBox(){
//		GUIMessageDialog.Instantiate Show (res => {
//			if(res == MessageBox.DialogResult.Ok){
//				GUIManager.Instance.ShowDialog (DialogName.MissionDetail);
//			}
//			return true;
//		}, "aaaaaaaaaaaaaaaaaaaa", "dafadf", MessageBox.Buttons.OKCancel);
////		GUIManager.Instance.ShowDialog (DialogName.Test);
	}
}
