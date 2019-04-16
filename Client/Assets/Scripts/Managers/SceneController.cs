using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum GameScene {
    MainMenu,
    Gameplay
}

public class SceneController : SingletonMono<SceneController>
{
    [SerializeField] Material TransitionMaterial;
    bool lastLoadUsedEff = false;
    System.Action onLoadSceneFinish;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OpenScene(GameScene scene, bool useEff = true, System.Action onLoadSceneFinish = null) {        
        lastLoadUsedEff = useEff;
        this.onLoadSceneFinish = onLoadSceneFinish;
        GUIManager.Instance.HideAllDialogImediately();
        if (!useEff)
            SceneManager.LoadScene(scene.ToString());
        if (useEff) {          
            PlayHideEffect(()=> {
                SceneManager.LoadScene(scene.ToString());
            });
        }
      
    }

    public void PlayAppearEffect() {
        Blit blitEff = Camera.main.GetComponent<Blit>();
        if (blitEff == null)
            blitEff = Camera.main.gameObject.AddComponent<Blit>();
        blitEff.TransitionMaterial = TransitionMaterial;

        if (lastLoadUsedEff)
        {
            DOTween.To(x =>
            {
                TransitionMaterial.SetFloat("_Cutoff", x);
            }, 1, 0, 1f).OnComplete(()=> {
                if (onLoadSceneFinish != null)
                    onLoadSceneFinish();
            });
        }
        else {
            TransitionMaterial.SetFloat("_Cutoff", 0);
            TransitionMaterial.SetFloat("_Fade", 0);
            if (onLoadSceneFinish != null)
                onLoadSceneFinish();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {         
        PlayAppearEffect();
    }

    public void PlayHideEffect(System.Action onFinish = null) {
        Blit blitEff = Camera.main.GetComponent<Blit>();
        if (blitEff == null)
            blitEff = Camera.main.gameObject.AddComponent<Blit>();
        blitEff.TransitionMaterial = TransitionMaterial;

        TransitionMaterial.SetFloat("_Cutoff", 1);
        TransitionMaterial.SetFloat("_Fade", 0);
        // hide current scene
        DOTween.To(x => {
            TransitionMaterial.SetFloat("_Fade", x);
        }, 0, 1, 1f).OnComplete(() => {
            if (onFinish != null)
                onFinish();
        });
    }
}
