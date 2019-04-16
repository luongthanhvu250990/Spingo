    using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DG.Tweening;

public class StageResultDialog : GUIBaseDialogHandler {
    [SerializeField]
    TextMeshProUGUI continueText;

    [SerializeField]
    Image continueFrame;

    [SerializeField]
    Button continueButton;
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
            continueFrame.fillAmount = x/10f;
        }, 9.99999999f, 0, 10f).SetEase(Ease.Linear).OnComplete(() => {
            SceneController.Instance.OpenScene(GameScene.MainMenu);
        });
    }

    public void OnClickContinue(){
        HideSelf(null, true);
        GameManager.Instance.Replay();
	}

    
}
