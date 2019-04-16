using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : SingletonMono<HUD>
{
    [SerializeField]
    TextMeshProUGUI scoreText;
   [SerializeField]
    TextMeshProUGUI starText;
   [SerializeField]
    TextMeshProUGUI skullText;
    [SerializeField]
    GameObject treasureObj;

    private void Start()
    {
        SetScore(0);
        SetStar(0);
        SetSkull(0);
        
        treasureObj.SetActive(GameManager.Instance.GameMode == GameMode.Story);
    }

    public void SetScore(int val)
    {
        scoreText.text = val.ToString();        
    }

    public void SetStar(int val)
    {
        starText.text = val.ToString();
    }

    public void SetSkull(int val)
    {
        skullText.text = val.ToString();
    }

    public void OnClickPause() {
        GUIManager.Instance.ShowDialog(DialogName.Pause);
    }

    public void OnClickShorten() {
        GameManager.Instance.AddSize(-1);
    }

    public void OnClickExtend() {
        GameManager.Instance.AddSize(1);
    }

    public void OnClickPowerUp()
    {
        GameManager.Instance.PowerUp();
    }

    public void OnClickSlow()
    {
        GameManager.Instance.AddSpeed(-100);
    }
}
