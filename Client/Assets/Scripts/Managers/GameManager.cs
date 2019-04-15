using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : SingletonMono<GameManager>
{    
    public const string PREFAB_PATH = "Paddles/";
    PaddleController paddleController;
    private void Start()
    {
        SoundManager.Instance.Play("Background");
    }

    public void StartNewGame() {
        SceneController.Instance.OpenScene(GameScene.Gameplay, true, () =>
        {
            if (paddleController != null)
                Destroy(paddleController.gameObject);
            GameObject go = GameObjectUtils.LoadGameObject(PREFAB_PATH + "Paddle_theme" + Random.Range(1, 5));;
            paddleController = go.GetComponent<PaddleController>();
            paddleController.IsPause = false;            
        });        
    }

    public void AddSize(float val) {
        if (paddleController == null) return;
        if (paddleController.IsPause) return;
        paddleController.AddSize(val);     
    }

    public void AddSpeed(float val)
    {
        if (paddleController == null) return;
        if (paddleController.IsPause) return;
        paddleController.AddSpeed(val);
    }

    public void PowerUp() {
        if (paddleController == null) return;
        if (paddleController.IsPause) return;
        paddleController.PowerUp();
    }

    public void SetPause(bool isPause) {
        if (paddleController == null) return;
        paddleController.IsPause = isPause;
    }
}
