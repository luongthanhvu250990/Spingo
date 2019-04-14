using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class MessageBoxHandler : GUIBaseDialogHandler {
	MessageBox.MessageItem msgItem;
	[SerializeField] Text captionText;
	[SerializeField] Text msgText;
	[SerializeField] Button button1;
	[SerializeField] Button button2;
	[SerializeField] Button button3;
	public override void OnBeginShow (object parameter)
	{
		msgItem = (MessageBox.MessageItem)parameter;
		captionText.text = msgItem.caption;
		msgText.text = msgItem.message;
		CheckActiveButton ();
		base.OnBeginShow (parameter);
		Debug.LogWarning ("begin show message");
	}

	public override void OnBeginHide (object parameter)
	{
		base.OnBeginHide (parameter);
		Debug.LogWarning ("Begin hide message");
	}

	public override void OnEndShow ()
	{
		base.OnEndShow ();
		Debug.LogWarning ("End Show message");
	}

	public override void OnEndHide ()
	{
		base.OnEndHide ();
		Debug.LogWarning ("end hide message");
	}

	public override void SettingShowEffect ()
	{
		showSequence.Append (guiControlLocation.transform.DOLocalMoveX (-40, 0.5f));
		showSequence.Append (guiControlLocation.transform.DOLocalMoveX (0, 0.2f));
//		showSequence.Insert (0.6f, guiControlLocation.transform.DOScale (1.2f, 0.2f));
//		showSequence.Insert (0.8f, guiControlLocation.transform.DOScale (1f, 0.2f));
//		base.SettingShowEffect ();
	}

	void CheckActiveButton(){
		button1.gameObject.SetActive (false);
		button2.gameObject.SetActive (false);
		button3.gameObject.SetActive (false);
		switch (msgItem.buttons) {
		case MessageBox.ButtonStyle.OK:		
			button1.gameObject.SetActive (true);
			button1.GetComponentInChildren<Text> ().text = "OK";
			break;
		case MessageBox.ButtonStyle.YesNo:
			button1.gameObject.SetActive (true);
			button2.gameObject.SetActive (true);

			button1.GetComponentInChildren<Text> ().text = "Yes";
			button2.GetComponentInChildren<Text> ().text = "No";
			break;
		default:
			break;
		}	
	}

	public void OnButton1Click(){
		if (msgItem.callback != null) {
			switch (msgItem.buttons) {
			case MessageBox.ButtonStyle.OK:		
				msgItem.callback (MessageBox.MessageResult.Ok);			
				break;
			case MessageBox.ButtonStyle.YesNo:
				msgItem.callback (MessageBox.MessageResult.Yes);
							
				break;
			default:
				break;
			}	
		}
		HideSelf ();			
	}

	public void OnButton2Click(){
		if (msgItem.callback != null) {
			switch (msgItem.buttons) {
			case MessageBox.ButtonStyle.YesNo:
				msgItem.callback (MessageBox.MessageResult.No);
				break;
			default:
				break;
			}	
		}
		HideSelf ();	
	}

	public void OnButton3Click(){
		
	}
}
