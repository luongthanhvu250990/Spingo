using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;


public class GUIBaseDialogHandler : MonoBehaviour
{
	DialogName dialogName = DialogName.None;
	public DialogName DialogName {
		get {
			return dialogName;
		}
		set {
			dialogName = value;
		}
	}

	string instanceId;
	public string InstanceId {
		get {
			return instanceId;
		}
		set {
			instanceId = value;
		}
	}

	DialogStatus showStatus = DialogStatus.Invalid;
	public DialogStatus ShowStatus {
		get {
			return showStatus;
		}
		set{ 
			showStatus = value;
		}
	}


	bool closeOnClickBlackBorder;
	public bool CloseOnClickBlackBorder {
		get {
			return closeOnClickBlackBorder;
		}
		set {
			closeOnClickBlackBorder = value;
		}
	}

    [HideInInspector]
    public Canvas canvas;

	public GameObject guiControlLocation;
	public List<RectTransform> insideRectChecker;
	List<RectTransform> runtimeRectChecker;

    public void Awake()
    {
        Initial();
        OnInit();
    }

	public void Initial()
	{
		if (canvas == null)
		{
			canvas = gameObject.GetComponent<Canvas>();
		}
	}

    public virtual void OnInit()
    {

    }

    public virtual void OnBeginShow(object parameter)
    {

    }

    public virtual void OnEndShow()
    {

    }

    public virtual void OnBeginHide(object parameter)
    {
        
    }

    public virtual void OnEndHide()
    {

    }

	public virtual void OnUpdateData(object parameter){
		
	}

	public virtual void OnClickOutSide(Vector2 inputPosition){
		
	}

	void Update(){		
		CheckOutsideClick ();
	}

	void CheckOutsideClick(){
		if (Input.GetMouseButtonDown (0)) {
			if (ShowStatus == DialogStatus.Showed && insideRectChecker.Count > 0) {
				if (!IsInsideClick ()) {
					OnClickOutSide (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
				}
			}
		}
	}

	bool IsInsideClick(){		
		for (int i = 0; i < insideRectChecker.Count; i++) {
			if (RectTransformUtility.RectangleContainsScreenPoint (insideRectChecker [i], new Vector2 (Input.mousePosition.x, Input.mousePosition.y), GUIManager.Instance.uiCamera)) {
				return true;
			}
		}

		for (int i = 0; i < runtimeRectChecker.Count; i++) {
			if (RectTransformUtility.RectangleContainsScreenPoint (runtimeRectChecker [i], new Vector2 (Input.mousePosition.x, Input.mousePosition.y), Camera.main)) {
				return true;
			}
		}

		return false;
	}

	public void AddRectChecker(RectTransform _rect)
	{
		if (runtimeRectChecker == null)
			runtimeRectChecker = new List<RectTransform> ();
		if (!runtimeRectChecker.Contains (_rect))
			runtimeRectChecker.Add (_rect);
	}

	public void HideSelf(object parameter, bool forceImediate = false){
		GUIManager.Instance.HideDialog (dialogName, instanceId, parameter, forceImediate);
	}

	public void HideSelf(){
		GUIManager.Instance.HideDialog (dialogName, instanceId, null, false);
	}

	public int GetLayer(){
		if (canvas == null)
		{
			return 0;
		}
		return canvas.sortingOrder;
	}

	public void SetLayer(int layer){
		if (canvas == null)
		{
			return;
		}
		canvas.sortingOrder = layer;
	}

	#region Effect
	protected Sequence showSequence;
	protected Sequence hideSequence;

	public virtual void SettingShowEffect(){
		//showSequence.Append (guiControlLocation.transform.DOLocalMoveX (-40, 0.5f));
		//showSequence.Append (guiControlLocation.transform.DOLocalMoveX (0, 0.2f));        
        showSequence.Append(guiControlLocation.transform.DOScale(1.2f, 0.2f));
        showSequence.Append(guiControlLocation.transform.DOScale(1, 0.2f));
    }

	public virtual void SettingHideEffect(){
		hideSequence.Append (guiControlLocation.transform.DOLocalMoveX (1500, 0.5f));
	}

	public void PlayShow(){
		if (hideSequence != null) {
			hideSequence.Kill ();
		}

		if (guiControlLocation != null) {
			showSequence = DOTween.Sequence ();
			SettingShowEffect ();
			showSequence.AppendCallback (() => {				
				OnEndShow ();
				GUIManager.InvokeEvent (dialogName, DialogEvent.Showed);
				showStatus = DialogStatus.Showed;
			});
			showSequence.Play ();
		} else {
			OnEndShow ();
			GUIManager.InvokeEvent (dialogName, DialogEvent.Showed);
			showStatus = DialogStatus.Showed;
		}
	}


	public void PlayHide(GUIDialogBase dialogBase){
		if (showSequence != null) {
			showSequence.Kill ();
		}

		if (guiControlLocation != null) {
			hideSequence = DOTween.Sequence ();
			SettingHideEffect ();
			hideSequence.AppendCallback (() => {				
				OnEndHide ();
				GUIManager.InvokeEvent (dialogName, DialogEvent.Hidden);
				dialogBase.DestroyDialog (this);
			});
			hideSequence.Play ();
			showStatus = DialogStatus.Hiding;
		} else {
			showStatus = DialogStatus.Hiding;
			OnEndHide ();
			GUIManager.InvokeEvent (dialogName, DialogEvent.Hidden);
			dialogBase.DestroyDialog (this);
		}
	}
	#endregion
}
