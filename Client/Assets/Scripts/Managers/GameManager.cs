using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameMode {
    Story,
    Endless,
}

public class GameManager : SingletonMono<GameManager>
{
    //For Demo only---------------------
    int star = 0;
    int skull = 0;
    int score = 0;
    public int Star
    {
        get
        {
            return star;
        }
    }
    //---------------------------------    
    public const string PREFAB_PATH = "Paddles/";

    GameMode gameMode;
    bool isPause = false;
    bool isGameOver = false;
    PaddleController paddleController;

    public bool IsPause { get => isPause; set => isPause = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public GameMode GameMode
    {
        get
        {
            return gameMode;
        }

        private set
        {
            gameMode = value;
        }
    }   

    private void Start()
    {       
        SoundManager.Instance.Play("Background");
    }

    public void StartNewGame(GameMode gameMode) {
        this.GameMode = gameMode;
        star = 0;
        skull = 0;
        score = 0;

        SceneController.Instance.OpenScene(GameScene.Gameplay, true, () =>
        {
            if (paddleController != null)
                Destroy(paddleController.gameObject);
            GameObject go = GameObjectUtils.LoadGameObject(PREFAB_PATH + "Paddle_theme" + Random.Range(1, 5));;
            paddleController = go.GetComponent<PaddleController>();
            paddleController.IsPause = false;
            IsGameOver = false;
            IsPause = false;     
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

    public void EatStar() {
        star++;
        HUD.Instance.SetStar(star);
    }

    public void EatSkull()
    {
        skull++;
        HUD.Instance.SetSkull(skull);
    }

    public void IncreaseScore(int score)
    {
        this.score += score;
        HUD.Instance.SetScore(this.score);
    }
    public void GameOver()
    {
        IsPause = true;
        if (!IsGameOver)
        {
            IsGameOver = true;

            if (GameMode == GameMode.Story)
            {
                GUIManager.Instance.ShowDialog(DialogName.StageResult);
            }
            else
            {
                GUIManager.Instance.ShowDialog(DialogName.MissionResult);
            }
        }
    }

    public void Replay() {
        StartNewGame(this.gameMode);
    }
}
