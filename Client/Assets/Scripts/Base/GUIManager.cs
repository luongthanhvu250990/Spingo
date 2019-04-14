using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum DialogName
{	
	None = 0,
	MessageBox,
	MissionDetail,
	MissionResult,
	MissionSelect,
	Pause,
	Shop,
    StageStart,
	StageResult,
	StarResult,	
}

public enum DialogEvent
{
	BeginShow,
	Showed,
	BeginHide,
	Hidden
}

public delegate void ButtonClick();
public delegate void OnGuiEvent(DialogName _dialog, DialogEvent _event);

public class GUIManager : SingletonMono<GUIManager>
{	    
	private bool lockShowDialog = false;	
	private GUIDialogBase lastSelectedDialog;
	private List<GUIDialogBase> listDialogs = new List<GUIDialogBase>();  
	private List<GUIBaseDialogHandler> showedDialogList = new List<GUIBaseDialogHandler>();
	private event OnGuiEvent onGuiEvent;

	public void AddShowedDialog (GUIBaseDialogHandler dl){
		showedDialogList.Add (dl);
	}

	public void RemoveShowedDialog(GUIBaseDialogHandler dl){
		showedDialogList.Remove (dl);
	}

    [SerializeField] Transform dialogsTrans;
 	[SerializeField] int baseLayer = 0;
	[SerializeField] int deltaLayer = 5;
	[SerializeField] int deltaBlackborder = 1;
	[SerializeField] int deltaZ = 100;
	public int DeltaZ {
		get {
			return deltaZ;
		}
	}
	[SerializeField] private Canvas blackBorderCanvas;

	public Camera uiCamera;

	void Start()
	{
        Init();
	}

	protected void Init ()
	{
		listDialogs = new List<GUIDialogBase>(dialogsTrans.GetComponentsInChildren<GUIDialogBase>());      
		if (uiCamera == null) {
			uiCamera = Camera.main;
		}
        blackBorderCanvas.worldCamera = uiCamera;
	}

	public bool CanShow(DialogName dialogName)
	{
		if (lockShowDialog) {
			Debug.Log ("Lock show");
			return false;
		}
		for (int i = 0; i < listDialogs.Count; i++) {
			if (listDialogs [i].DialogName == dialogName)
				return true;
		}
		Debug.Log ("Can not find dialog: " + dialogName.ToString());
		return false;
	}

	public bool IsVisible(DialogName dialogName)
	{
		for (int i = 0; i < showedDialogList.Count; i++) {
			if (showedDialogList [i].DialogName == dialogName) {
				Debug.LogError (showedDialogList [i].ShowStatus);
				return (showedDialogList [i].ShowStatus == DialogStatus.Showing || showedDialogList [i].ShowStatus == DialogStatus.Showed);
			}
		}
		return false;
	}

	public int GetTopLayer(){
		int rs = baseLayer;
		for (int i = 0; i < listDialogs.Count; i++) {
			rs = Mathf.Max (listDialogs [i].GetTopLayer(), rs);
		}
		return rs;
	}

	public int NewTopLayer(){
		return GetTopLayer () + deltaLayer;
	}

	public void SetBlackBorder(int layer){
		blackBorderCanvas.gameObject.SetActive (true);
		blackBorderCanvas.sortingOrder = layer - deltaBlackborder;
	}

	public void CheckBlackBorder(){
		int rs = baseLayer;
		for (int i = 0; i < listDialogs.Count; i++) {
			if (listDialogs [i].UseBlackBorder)
				rs = Mathf.Max (listDialogs [i].GetTopLayer (), rs);
		}

		if (rs == baseLayer)
			blackBorderCanvas.gameObject.SetActive (false);
		else
			SetBlackBorder (rs);
	}

	public void OnClickBlackBorder(){
		if (showedDialogList.Count <= 0)
			return;
		GUIBaseDialogHandler dl = showedDialogList [showedDialogList.Count - 1];
		if (dl.CloseOnClickBlackBorder)
			dl.HideSelf ();
	}

	public void ShowDialog(DialogName dlgName, bool closeOnClickBlackBorder = false, object param = null)
	{
		if (!CanShow(dlgName))
			return;
		
		GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.DialogName == dlgName);
		if (foundDlg == null)
		{			
			return;
		}

		foundDlg.TryShow (param, closeOnClickBlackBorder);
	}

	public void HideDialog(DialogName dlgName, object param = null, bool forceImediate = false)
	{		
		GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.DialogName == dlgName);
		if (foundDlg == null)
		{
			Debug.LogError("Can not find dialog:" + dlgName.ToString());
			return;
		}

		foundDlg.Hide(param, forceImediate);	
	}


	public void HideDialog(DialogName dlgName, string instanceId, object param = null, bool forceImediate = false)
	{		
		GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.DialogName == dlgName);
		if (foundDlg == null)
		{
			Debug.LogError("Can not find dialog:" + dlgName.ToString());
			return;
		}

		foundDlg.Hide(param, instanceId, forceImediate);	
	}

	public void UpdateDialogData(DialogName dlgName, object param = null){
		GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.DialogName == dlgName);
		if (foundDlg == null)
		{
			Debug.LogError("Can not find dialog:" + dlgName.ToString());
			return;
		}

		foundDlg.UpdateData(param);	
	}

	public void UpdateDialogData(DialogName dlgName, string instanceId, object param = null){
		GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.DialogName == dlgName);
		if (foundDlg == null)
		{
			Debug.LogError("Can not find dialog:" + dlgName.ToString());
			return;
		}

		foundDlg.UpdateData(instanceId, param);	
	}

	public void AddRectChecker(DialogName dialogName, RectTransform rect)
	{
		GUIDialogBase foundDlg = listDialogs.Find (dlg => dlg.DialogName == dialogName);
		if(foundDlg!=null)
			foundDlg.rectCheckers.Add (rect);
	}

	#if ENABLE_DEBUG_GUI
	void OnGUI(){
		if (GUI.Button (new Rect (0, 0, 50, 50), "Show")) {
//			ShowMessageBox (res=>{
//				Debug.LogWarning(res.ToString());	
//			}, "aaa", "bbbb", true);
			GUIManager.instance.ShowDialog(DialogName.Shop, true);
		}

		if (GUI.Button (new Rect (100, 0, 50, 50), "HIde")) {
			GUIManager.Instance.HideDialog (DialogName.Shop);
		}
	}
	#endif

	#region Message Box
	public static void ShowMessageBox(MessageBox.MessageCallback callback, string caption, string message, MessageBox.ButtonStyle buttonStyle, bool closeOnClickBlackBorder){
		MessageBox.MessageItem item = new MessageBox.MessageItem ();
		item.caption = caption;
		item.message = message;
		item.callback = callback;
		item.buttons = buttonStyle;

		Instance.ShowDialog (DialogName.MessageBox, closeOnClickBlackBorder, item);
	}

	public static void ShowMessageBox(string caption, string message, bool closeOnClickBlackBorder){
		GUIManager.ShowMessageBox (null, caption, message, MessageBox.ButtonStyle.OK, closeOnClickBlackBorder);
	}

	public static void ShowMessageBox(MessageBox.MessageCallback callback, string caption, string message, bool closeOnClickBlackBorder){
		GUIManager.ShowMessageBox (callback, caption, message, MessageBox.ButtonStyle.OK, closeOnClickBlackBorder);
	}

	#endregion

	#region Dialog Event trigger
	public static void Register(OnGuiEvent _action)
	{
		if (_action != null)
			Instance.onGuiEvent += _action;
	}

	public static void Unregister(OnGuiEvent _action)
	{
		if (_action != null && Instance!=null)
            Instance.onGuiEvent -= _action;
	}

	public static void InvokeEvent(DialogName _dialog, DialogEvent _event)
	{
		if (Instance.onGuiEvent != null) {
			Instance.onGuiEvent.Invoke (_dialog, _event);
		}
	}
	#endregion
}
