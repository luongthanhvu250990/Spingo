using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum DialogStatus
{
	Invalid,
	Ok,
	Showing,
	Showed,
	Hiding
}

public enum DialogType
{
	Single,
	Multiple
}

public class GUIDialogBase : MonoBehaviour
{
	public const string PREFAB_PATH = "Prefabs/Dialogs/";
	public const string SHOW_ANIM = "show";
	public const string HIDE_ANIM = "hide";

	[SerializeField]  DialogName dialogName = DialogName.None;

	public DialogName DialogName {
		get {
			return dialogName;
		}
	}

	[SerializeField]  string prefabName = "";

	[Space (10)]
	[SerializeField] DialogType dialogType = DialogType.Single;

	[Space (10)]
	[SerializeField] bool useBlackBorder = true;
	public bool UseBlackBorder {
		get {
			return useBlackBorder;
		}
	}

	[SerializeField] float showDelay = 0f;

	[Space (10)]
	[SerializeField] bool isSetupLocation = true;
	[SerializeField] Vector3 position = Vector3.zero;

	public List<RectTransform> rectCheckers = new List<RectTransform>();

	private List<GUIBaseDialogHandler> showedDialogList = new List<GUIBaseDialogHandler> ();

	public GUIBaseDialogHandler Init ()
	{
		try {   
			GameObject obj = GameObjectUtils.LoadGameObject (gameObject.transform, PREFAB_PATH + prefabName);
			obj.name = prefabName;      

			GUIBaseDialogHandler baseDialogHandler = obj.GetComponentInChildren<GUIBaseDialogHandler> ();

			Canvas canvas = obj.GetComponent<Canvas> ();
			canvas.overrideSorting = true;
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = GUIManager.Instance.uiCamera;		

			baseDialogHandler.InstanceId = GameUtils.GenerateID ();
			baseDialogHandler.ShowStatus = DialogStatus.Ok;
			baseDialogHandler.DialogName = dialogName;

			baseDialogHandler.OnInit ();

			return baseDialogHandler;
		} catch (Exception ex) {
			Debug.LogError ("Error:" + ex.Message);
		}
		return null;
	}

	public void TryShow (object parameter, bool closeOnClickBlackBorder = false)
	{		
		if (dialogType == DialogType.Single) {
			if (showedDialogList.Count != 0) {
				if (showedDialogList [0].ShowStatus == DialogStatus.Showed || showedDialogList [0].ShowStatus == DialogStatus.Showing) {
					showedDialogList [0].OnUpdateData (parameter);
					return;
				} else if (showedDialogList [0].ShowStatus == DialogStatus.Hiding) {			
					showedDialogList [0].OnEndHide ();

					Destroy (showedDialogList [0].gameObject);

					GUIManager.Instance.RemoveShowedDialog (showedDialogList [0]);
					showedDialogList.RemoveAt (0);
				}
			}
		}

		GUIBaseDialogHandler baseDialogHandler = Init ();
		if (baseDialogHandler == null) {
			return;
		}
		baseDialogHandler.CloseOnClickBlackBorder = closeOnClickBlackBorder;

		showedDialogList.Add (baseDialogHandler);
		GUIManager.Instance.AddShowedDialog (baseDialogHandler);

		StartCoroutine (DoShow (baseDialogHandler, parameter));
	}

	private IEnumerator DoShow (GUIBaseDialogHandler baseDialogHandler, object parameter)
	{
		GUIManager.InvokeEvent (dialogName, DialogEvent.BeginShow);
		yield return new WaitForSeconds (showDelay);

		if (isSetupLocation && baseDialogHandler.guiControlLocation != null) {
			baseDialogHandler.guiControlLocation.transform.localPosition = position;				
		}
	
		baseDialogHandler.OnBeginShow (parameter);
		baseDialogHandler.ShowStatus = DialogStatus.Showing;

			
		int newLayer = GUIManager.Instance.NewTopLayer ();
		baseDialogHandler.SetLayer (newLayer);
		if (useBlackBorder) {
			GUIManager.Instance.SetBlackBorder (baseDialogHandler.GetLayer ());	
		}

		Vector3 temp = baseDialogHandler.guiControlLocation.transform.localPosition;
		baseDialogHandler.guiControlLocation.transform.localPosition = new Vector3 (temp.x, temp.y, temp.z + newLayer * GUIManager.Instance.DeltaZ);

		if(rectCheckers!=null && rectCheckers.Count > 0)
			for (int i = 0; i < rectCheckers.Count; i++) {
				baseDialogHandler.AddRectChecker(rectCheckers[i]);
			}

		baseDialogHandler.PlayShow ();
	}

	public void Hide (object parameter, bool forceImediate = false)
	{
		for (int i = 0; i < showedDialogList.Count; i++) {
			Hide (parameter, showedDialogList [i], forceImediate);
		}	
	}

	public void Hide(object parameter, string instanseID, bool forceImediate = false){
		GUIBaseDialogHandler baseDialogHandler = null;
		for (int i = 0; i < showedDialogList.Count; i++) {
			if (string.Compare (instanseID, showedDialogList [i].InstanceId) == 0) {
				baseDialogHandler = showedDialogList [i];
			}
		}
		if (baseDialogHandler == null) {
			Debug.Log("Can not find dialog " + dialogName + " with id: " + instanseID);
			return ;
		}

		Hide (parameter, baseDialogHandler, forceImediate);
	}

	void Hide(object parameter, GUIBaseDialogHandler baseDialogHandler, bool forceImediate = false){
		if (baseDialogHandler.ShowStatus == DialogStatus.Showing) {
			baseDialogHandler.OnEndShow ();
		}
		if (forceImediate) {
			GUIManager.InvokeEvent (dialogName, DialogEvent.BeginHide);
			baseDialogHandler.OnBeginHide(parameter);
			baseDialogHandler.ShowStatus = DialogStatus.Hiding;
			baseDialogHandler.OnEndHide();
			GUIManager.InvokeEvent (dialogName, DialogEvent.Hidden);
			DestroyDialog (baseDialogHandler);
		} else {
			GUIManager.InvokeEvent (dialogName, DialogEvent.BeginHide);
			baseDialogHandler.OnBeginHide (parameter);
			baseDialogHandler.ShowStatus = DialogStatus.Hiding;
			baseDialogHandler.PlayHide (this);
		}
	}

	public void DestroyDialog(GUIBaseDialogHandler baseDialogHandler){
		if (baseDialogHandler == null)
			return;
		
		showedDialogList.Remove(baseDialogHandler);
		GUIManager.Instance.RemoveShowedDialog (baseDialogHandler);

		Destroy(baseDialogHandler.gameObject);
		GUIManager.Instance.CheckBlackBorder ();
	}

	public void UpdateData(object param){
		for (int i = 0; i < showedDialogList.Count; i++) {
			showedDialogList [i].OnUpdateData (param);
		}	
	}


	public void UpdateData(string instanceId, object param){
		GUIBaseDialogHandler baseDialogHandler = null;
		for (int i = 0; i < showedDialogList.Count; i++) {
			if (string.Compare (instanceId, showedDialogList [i].InstanceId) == 0) {
				baseDialogHandler = showedDialogList [i];
			}
		}
		if (baseDialogHandler == null) {
			Debug.Log("Can not find dialog " + dialogName + " with id: " + instanceId);
			return ;
		}

		baseDialogHandler.OnUpdateData (param);
	}


	public int GetTopLayer ()
	{
		int rs = 0;
		for (int i = 0; i < showedDialogList.Count; i++) {
			rs = Mathf.Max (rs, showedDialogList [i].GetLayer ());
		}
		return rs;
	}

	public GUIBaseDialogHandler GetTopDialog(){
		if (!useBlackBorder)
			return null;
		
		GUIBaseDialogHandler rs = null;
		for (int i = 0; i < showedDialogList.Count; i++) {
			if (rs == null) {
				rs = showedDialogList [i];
			} else {
				if (showedDialogList [i].canvas.sortingOrder > rs.canvas.sortingOrder)
					rs = showedDialogList [i];
			}
		}
		return rs;
	}
}
