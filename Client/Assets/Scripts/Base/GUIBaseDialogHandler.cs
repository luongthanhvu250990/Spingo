using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class GUIBaseDialogHandler : MonoBehaviour
{

    [HideInInspector]
    public Canvas canvas;
    public GUIBaseDialogHandler guiDialogPrev;
    public List<Camera> childCameras;

    public void Initial()
    {
        if (canvas == null)
        {
            canvas = gameObject.GetComponent<Canvas>();
        }
    }

    public void Awake()
    {
        Initial();
        OnInit();
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
        HideAllChildCamera();
    }

    public virtual void OnEndHide(bool isDestroy)
    {

    }

    public virtual void HideAllChildCamera()
    {
        foreach (Camera c in childCameras)
        {
            if (c != null)
            {
                c.enabled = false;
                c.gameObject.SetActive(false);
            }

        }
    }

    public virtual void ShowAllChildCamera()
    {
        foreach (Camera c in childCameras)
        {
            if (c != null)
            {
                c.gameObject.SetActive(true);
                c.enabled = true;
            }
        }
    }
}
