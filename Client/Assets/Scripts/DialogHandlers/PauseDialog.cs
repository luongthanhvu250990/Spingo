    using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseDialog : GUIBaseDialogHandler {
    [SerializeField] TwoStateButton soundButton;

    public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
        soundButton.IsOn = !SoundManager.Instance.IsMuted();
    }

    public void OnClickHome()
    {
        HideSelf(null, true);
        SceneController.Instance.OpenScene(GameScene.MainMenu);
    }


    public void OnClickSoundButton()
    {
        SoundManager.Instance.MuteAll(!soundButton.IsOn);
    }
}
