    using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using TMPro;

public class MissionResultDialog : GUIBaseDialogHandler {
    [SerializeField]
    TextMeshProUGUI continueText;

    [SerializeField]
    Image continueFrame;

    public override void OnBeginShow (object parameter)
	{		
		base.OnBeginShow (parameter);
	}

    public override void OnEndShow()
    {
        base.OnEndShow();
        DOTween.To(x =>
        {
            continueText.text = ((int)x + 1).ToString();
            continueFrame.fillAmount = x / 10f;
        }, 9.99999999f, 0, 10f).SetEase(Ease.Linear).OnComplete(() => {
            GUIManager.Instance.ShowDialog(DialogName.StarResult);
        });
    }

    public void OnClickHome(){        
        SceneController.Instance.OpenScene(GameScene.MainMenu);
	}

    public void OnClickContinue() {
        GameManager.Instance.Replay();
    }
}
