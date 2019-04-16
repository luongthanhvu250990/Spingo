    using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class StarResultDialog : GUIBaseDialogHandler {
    [SerializeField]
    TextMeshProUGUI starText;

	public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
        starText.text = GameManager.Instance.Star.ToString();
	}

	public void OnClickMenu(){
        SceneController.Instance.OpenScene(GameScene.MainMenu);
	}

    public void OnClickReplay() {
        GameManager.Instance.Replay();
    }
}
