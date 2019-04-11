using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DialogName
{
    None = 0,
	MessageBox,
	Test,
}
public delegate void ButtonClick();

public class GUIManager : SingletonMono<GUIManager>
{
    bool lockShowDialog = false;

    public void LockShowDialog()
    {
        lockShowDialog = true;
    }

    public void UnLockShowDialog()
    {
        lockShowDialog = false;
    }

    public bool CanShow(DialogName dialogName)
    {
        if (lockShowDialog)
            return false;
        if (activeDialog == null || activeDialog.Count == 0)
            return true;
        for (int i = 0; i < activeDialog.Count; i++)
        {
            if (activeDialog[i].dialogName == dialogName)
                return false;
        }
        return true;
    }

    private List<GUIDialogBase> listDialogs = null;  
    //public GUIMessageDialog guiMessageDialogHandler;

    public List<GUIDialogBase> activeDialog = new List<GUIDialogBase>();

    public int ActiveDialogCount
    {
        get
        {
            int c = 0;
            foreach (GUIDialogBase dlg in activeDialog)
            {
                if (dlg.gameObject.activeSelf)
                {
                    c++;
                }
            }
            return c;
        }
    }


    public GameObject blackBorder;
    public Camera uiCamera;
  
    public Camera GetUICamera()
    {
        return uiCamera.GetComponent<Camera>();
    }
  
    void Start()
    {           
        listDialogs = new List<GUIDialogBase>(GetComponentsInChildren<GUIDialogBase>());      
    }
 
    public void UpdateCamera()
    {
        if (activeDialog.Count > 0)
        { // co dialog truoc do
            GUIBaseDialogHandler guiDialog;
            for (int i = 0; i < activeDialog.Count - 1; i++)
            {
                guiDialog = activeDialog[i].guiControlDlg.GetComponent<GUIBaseDialogHandler>();
                if (guiDialog != null)
                    guiDialog.GetComponent<GUIBaseDialogHandler>().HideAllChildCamera();
            }
            guiDialog = activeDialog[activeDialog.Count - 1].guiControlDlg.GetComponent<GUIBaseDialogHandler>();
            if (guiDialog != null)
                guiDialog.GetComponent<GUIBaseDialogHandler>().ShowAllChildCamera();
        }
    }

    public void ShowDialog(DialogName dlgName, object param = null)
    {
        if (!CanShow(dlgName))
            return;
        GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.dialogName == dlgName);
        if (foundDlg == null)
        {
            Debug.LogError("Ko tim thay dialog:" + dlgName);
            return;
        }
        Debug.Log("Start show dialog:" + dlgName);


        if (!foundDlg.TryShow(param))
        {
            Debug.LogError("Ko the show dialog:" + dlgName);
        }
        else
        {
            int index = activeDialog.FindIndex(obj => obj == foundDlg);
            if (index > -1)
                activeDialog.RemoveAt(index);
            activeDialog.Add(foundDlg);// add vao active
            UpdateCamera();
        }
        Input.multiTouchEnabled = false;
    }

    public void HideDialog(DialogName dlgName, object param = null)
    {

        GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.dialogName == dlgName);
        if (foundDlg == null)
        {
            Debug.LogError("Ko tim thay dialog:" + dlgName);
            return;
        }
        //Debug.LogError(dlgName);
        foundDlg.Hide(param);

        int index = activeDialog.FindIndex(obj => obj == foundDlg);
        if (index > -1)
            activeDialog.RemoveAt(index);
        UpdateCamera();
        Input.multiTouchEnabled = true;
    }

    public void HideDialogImediate(DialogName dlgName, object param = null)
    {
        GUIDialogBase foundDlg = listDialogs.Find(dlg => dlg.dialogName == dlgName);
        if (foundDlg == null)
        {
            Debug.LogError("Ko tim thay dialog:" + dlgName);
            return;
        }
        //Debug.LogError(dlgName);
        foundDlg.Hide(param);

        int index = activeDialog.FindIndex(obj => obj == foundDlg);
        if (index > -1)
            activeDialog.RemoveAt(index);
        UpdateCamera();

    }

    public GUIDialogBase GetDialog(DialogName dlgName)
    {
        return listDialogs.Find(dlg => dlg.dialogName == dlgName);
    }

    public void HideDialogAfterTime(DialogName dlgName, float delayTime)
    {
        StartCoroutine(CoroutineHideDialog(dlgName, delayTime));
    }

    private IEnumerator CoroutineHideDialog(DialogName dlgName, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        HideDialog(dlgName);
    }

    public void HideAllDialog(bool forceImediate = false)
    {
        if (activeDialog != null)
            activeDialog.Clear();
        if (listDialogs == null)
            return;

        foreach (var dlg in listDialogs)
        {
            if (dlg != null)
                dlg.Hide(null, forceImediate);
        }      
    }  

	public GUIDialogBase GetTopShow(){
		GUIDialogBase topShow = null;     
		
		for (int i = 0; i < listDialogs.Count; i++)
		{
			if (listDialogs[i].useBlackBackground)
			{
				if (listDialogs[i].status == GUIDialogBase.GUIDialogStatus.Showed
				    || listDialogs[i].status == GUIDialogBase.GUIDialogStatus.Showing)
				{
					if (topShow == null)
					{
						topShow = listDialogs[i];
					}
					else
					{
						if (topShow.guiControlLocation != null && listDialogs[i].guiControlLocation != null)
						{
							if (topShow.layer < listDialogs[i].layer)
							{
								topShow = listDialogs[i];
							}
						}
					}
				}
			}
		}
		return topShow;
	}

    public void CheckShowBorder()
    {
		GUIDialogBase topShow = GetTopShow ();
        if (topShow != null && topShow.guiControlLocation != null)
        {
            blackBorder.SetActive(true);
            blackBorder.GetComponent<Canvas>().sortingOrder = topShow.layer - 1;
        }
        else
        {
            blackBorder.SetActive(false);
        }
    }

	public int GetShowLayer(){
		var topShow = GetTopShow ();
		if (topShow != null&& topShow.guiControlLocation != null) {
			return topShow.layer + 20;
		}
		return -1000000;
	}
 
    public void ReleaseAllDialog()
    {
        if (listDialogs == null)
            return;
        for (int i = 0; i < listDialogs.Count; i++)
        {
            listDialogs[i].ForceDestroy();
        }
    }

   
}
