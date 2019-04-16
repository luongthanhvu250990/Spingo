    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseDialog : GUIBaseDialogHandler {
    [SerializeField] TwoStateButton soundButton;

    public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
        soundButton.IsOn = !SoundManager.Instance.IsMuted();
        GameManager.Instance.IsPause = true;
    }

    public override void OnEndHide()
    {
        base.OnEndHide();
        GameManager.Instance.IsPause = false;
    }

    public void OnClickHome()
    {
        //HideSelf(null, true);
        SceneController.Instance.OpenScene(GameScene.MainMenu);
    }


    public void OnClickSoundButton()
    {
        SoundManager.Instance.MuteAll(!soundButton.IsOn);
    }
}
