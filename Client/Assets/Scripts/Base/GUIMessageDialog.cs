using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MessageBox;


public class GUIMessageDialog : GUIDialogBase
{
    #region Dialog Button Control
    public class DialogBtnControl
    {
        public Transform tranformBtn;
        public Text text;
        public DialogResult result;
        public DialogBtnControl(Transform _tran, Text _labelText)
        {
            tranformBtn = _tran;
            text = _labelText;
        }
        public void Reset()
        {
            result = DialogResult.None;
            text.text = "";
            tranformBtn.gameObject.SetActive(false);
        }
        public void SetInfomation(DialogResult _result, string _text)
        {
            tranformBtn.gameObject.SetActive(true);
            result = _result;
			text.text = _text;            
        }
    }
    #endregion

    #region delegate
    public ButtonClick bt1_click;
    public ButtonClick bt2_click;
    public ButtonClick bt3_click;
    #endregion

    Text contentMessage;
    Text captionText;    
    Image imageTitle;

  
    // current Result for three button
    private static DialogBtnControl[] btnDialog = new DialogBtnControl[3];

    // control static
    private static List<MessageItem> items = new List<MessageItem>();
    public void Awake()
    {
        //messageHandler = this;
    }


    //********************* Overide *********************//
    public override GameObject OnInit()
    {

        GameObject obj = GameObjectUtils.LoadGameObject(gameObject.transform, GUI_PATH_PREFAB + dialogPrefab);
        obj.name = "UIMessageDialog";        

		guiControlLocation = obj.transform.Find (locationName).gameObject;
		contentMessage = guiControlLocation.transform.Find ("Content").GetComponent<Text>();
		captionText = guiControlLocation.transform.Find ("Caption").GetComponent<Text>();
		imageTitle = guiControlLocation.transform.Find ("Image").GetComponent<Image>();

        for (int i = 0; i < 3; i++)
        {
            Transform _tran = guiControlLocation.transform.Find("Buttons/Btn0" + (i + 1).ToString());
            Text _label = _tran.Find("Text").GetComponent<Text>();
            btnDialog[i] = new DialogBtnControl(_tran, _label);
            btnDialog[i].Reset();
        }
        layer = 20;
        Canvas canvas = obj.GetComponent<Canvas>();
        if (canvas != null)
            canvas.sortingOrder = layer;

        return obj;
    }

    protected override float OnBeginShow(object parameter)
    { 
        guiControlDlg.SetActive(true);
     
        if (parameter != null)
        {
            MessageItem item = (MessageItem)parameter;
            SetupDisplayButtons(item);
            contentMessage.text = item.message;
            captionText.text = item.caption;            
        }
        else
        {
            Debug.LogError("Method to open Message Dialog is not exactly");
        }
        return base.OnBeginShow(parameter);
    }


    protected override float OnBeginHide(object parameter)
    {
        return base.OnBeginHide(parameter);
    }

    protected override void OnEndHide(bool isDestroy)
    {
		base.OnEndHide (isDestroy);   
    }

    protected override void OnEndShow()
    {
		base.OnEndShow ();
    }

    //********************  End override ****************//
   public void OnClick()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Btn01":
                {
                    //Debug.LogError("111");
                    OnBtnClick(0);
                    if (bt1_click != null) bt1_click();
                }
                break;

            case "Btn02":
                {
                    //Debug.LogError("222");
                    OnBtnClick(1);
                    if (bt2_click != null) bt2_click();
                }
                break;

            case "Btn03":
                {
                    //Debug.LogError("333");
                    OnBtnClick(2);
                    if (bt3_click != null) bt3_click();
                }
                break;
        }
    }

    public void OnBtnClick(int i)
    {

        bool close = true;

        if (items[items.Count - 1].callback != null)
            close = items[items.Count - 1].callback(btnDialog[i].result);

        if (!close)
            return;

        items.RemoveAt(items.Count - 1);

        if (close && !CheckShowMessageDialog())
            GUIManager.Instance.HideDialog(DialogName.MessageBox);
    }

    public bool CheckShowMessageDialog()
    {
        if (items.Count > 0)
        {
            MessageItem _item = items[items.Count - 1];
            GUIManager.Instance.ShowDialog(DialogName.MessageBox, _item);
            return true;
        }

        return false;
    }


    #region simulator message same with .Net
    private void ResetMessageState()
    {
        for (int i = 0; i < 3; i++)
        {
            btnDialog[i].Reset();
        }
    }

    private void SetupDisplayButtons(MessageItem item)
    {
        ResetMessageState();
        switch (item.buttons) {
		case Buttons.OK:
			btnDialog [0].SetInfomation (DialogResult.Ok, "OK");
			break;

		case Buttons.OKCancel:
			btnDialog [0].SetInfomation (DialogResult.Ok, "OK");
			btnDialog [1].SetInfomation (DialogResult.Cancel, "Cancel");
			break;

		case Buttons.AbortRetryIgnore:
			btnDialog [0].SetInfomation (DialogResult.Abort, "Abort");
			btnDialog [1].SetInfomation (DialogResult.Retry, "Retry");
			btnDialog [2].SetInfomation (DialogResult.Ignore, "Ignore");
			break;

		case Buttons.YesNoCancel:
			btnDialog [0].SetInfomation (DialogResult.Yes, "Yes");
			btnDialog [1].SetInfomation (DialogResult.No, "No");
			btnDialog [2].SetInfomation (DialogResult.Cancel, "Cancel");
			break;

		case Buttons.YesNo:
			btnDialog [0].SetInfomation (DialogResult.Yes, "Yes");
			btnDialog [1].SetInfomation (DialogResult.No, "No");
			break;

		case Buttons.RetryCancel:
			btnDialog [0].SetInfomation (DialogResult.Retry, "Retry");
			btnDialog [1].SetInfomation (DialogResult.Cancel, "Cancel");
			break;

		default:
			btnDialog [0].SetInfomation (DialogResult.None, "OK");
			break;
		}
    }

    public static void Show(string message)
    {
        Show(null, message, string.Empty, Buttons.OK, Icon.None, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message)
    {
        Show(callback, message, string.Empty, Buttons.OK, Icon.None, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption)
    {
        Show(callback, message, caption, Buttons.OK, Icon.None, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption, Buttons buttons)
    {
        Show(callback, message, caption, buttons, Icon.None, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption, Buttons buttons, Icon icon)
    {
        Show(callback, message, caption, buttons, icon, MessageBox.DefaultButton.Button1);
    }

    public static void Show(MessageCallback callback, string message, string caption, Buttons buttons, Icon icon, MessageBox.DefaultButton defaultButton)
    {
        MessageItem item = new MessageItem
        {
            caption = caption,
            buttons = buttons,
            defaultButton = defaultButton,
            callback = callback
        };
        item.message = message;
        switch (icon)
        {
            case Icon.Hand:
            case Icon.Stop:
            case Icon.Error:               
                break;

            case Icon.Exclamation:
            case Icon.Warning:                
                break;

            case Icon.Asterisk:
            case Icon.Information:                
                break;
        }
        if (items.Count > 2)
        {
            items.RemoveAt(0);
        }
        items.Add(item);

        GUIManager.Instance.ShowDialog(DialogName.MessageBox, item);
    }
    #endregion


}
