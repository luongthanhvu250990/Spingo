using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    public string sfxName = "Click";
    // Use this for initialization
    void Start()
    {
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (string.IsNullOrEmpty(sfxName))
        {
            sfxName = "Click";
        }
        SoundManager.Instance.Play(sfxName);
    }
}
