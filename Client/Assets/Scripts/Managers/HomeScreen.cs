using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreen : MonoBehaviour
{
    public void OnClickStories() {
        SceneController.Instance.OpenScene(GameScene.Gameplay);
    }
}
