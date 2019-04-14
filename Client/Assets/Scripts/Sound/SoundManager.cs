using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Background,
    Effect_Single,
    Effect_Multiple
}

public class SoundManager : SingletonMono<SoundManager>
{
    [SerializeField] AudioListener auListener;
    [SerializeField] bool backgroundOn = true;
    public bool BackgroundOn
    {
        get
        {
            return backgroundOn;
        }
        set
        {
            backgroundOn = value;
            if (backgroundOn)
            {
                foreach (var item in soundDict)
                {
                    if (item.Value.SndType == SoundType.Background)
                        item.Value.UnpauseBackground();
                }
            }
            else
            {
                foreach (var item in soundDict)
                {
                    if (item.Value.SndType == SoundType.Background)
                        item.Value.PauseBackground();
                }
            }
        }
    }

    [SerializeField] bool effectOn = true;
    public bool EffectOn
    {
        get
        {
            return effectOn;
        }
        set
        {
            effectOn = value;
            if (!effectOn)
            {
                foreach (var item in soundDict)
                {
                    if (item.Value.SndType == SoundType.Effect_Single || item.Value.SndType == SoundType.Effect_Multiple)
                        item.Value.Stop();
                }
            }
        }
    }

    [SerializeField] [Range(0, 1)] float backgroundVolume = 1;
    public float BackgroundVolume
    {
        get
        {
            return backgroundVolume;
        }
        set
        {
            backgroundVolume = value;
            foreach (var item in soundDict)
            {
                if (item.Value.SndType == SoundType.Background)
                    item.Value.RefreshVolume();
            }
        }
    }

    [SerializeField] [Range(0, 1)] float effectVolume = 1;
    public float EffectVolume
    {
        get
        {
            return effectVolume;
        }
        set
        {
            effectVolume = value;
            foreach (var item in soundDict)
            {
                if (item.Value.SndType != SoundType.Background)
                    item.Value.RefreshVolume();
            }
        }
    }

    List<SoundItem> currentBackground = new List<SoundItem>();
    public bool IsMuted()
    {
        return AudioListener.pause;
    }

    Dictionary<string, SoundItem> soundDict = new Dictionary<string, SoundItem>();
    // Use this for initialization
    void Awake()
    {
        var temp = GetComponentsInChildren<SoundItem>();
        for (int i = 0; i < temp.Length; i++)
        {
            soundDict.Add(temp[i].gameObject.name, temp[i]);
        }
    }

    public void MuteAll(bool isMuted)
    {
        AudioListener.pause = isMuted;
    }

    public void StopAll()
    {
        foreach (var item in soundDict)
        {
            item.Value.Stop();
        }
    }

    public SoundItem GetSound(string sName)
    {
        if (soundDict.ContainsKey(sName))
            return soundDict[sName];
        return null;
    }

    public void Play(string sName)
    {
        SoundItem s = GetSound(sName);

        if (s != null)
        {
            if (!BackgroundOn && s.SndType == SoundType.Background)
                return;

            if (!EffectOn && (s.SndType == SoundType.Effect_Single || s.SndType == SoundType.Effect_Multiple))
                return;

            if (s.SndType == SoundType.Background && !currentBackground.Contains(s))
                currentBackground.Add(s);
            s.Play();
        }
        else
        {
            Debug.LogWarning(sName + " not found");
        }
    }

    public void Stop(string sName)
    {
        SoundItem s = GetSound(sName);
        if (s != null)
        {
            if (s.SndType == SoundType.Background && currentBackground.Contains(s))
                currentBackground.Remove(s);
            s.Stop();
        }
        else
        {
            Debug.LogWarning(sName + "not found");
        }
    }

    //	public void Pause(string sName){
    //		SoundItem s = GetSound (sName);
    //		if (s != null) {
    //			s.Pause();
    //		} else {
    //			Debug.LogWarning(sName + "not found");
    //		}
    //	}
    //
    //	public void Resume(string sName){
    //		SoundItem s = GetSound (sName);
    //		if (s != null) {
    //			s.UnPause();
    //		} else {
    //			Debug.LogWarning(sName + "not found");
    //		}
    //	}

    public void AddSound(string sName, AudioClip sound, SoundType soundType, float volume = 1)
    {
        if (GetSound(sName) != null)
            return;
        GameObject go = Instantiate(new GameObject(), transform);
        go.name = sName;
        SoundItem si = go.AddComponent<SoundItem>();
        si.AudClip = sound;
        si.SndType = soundType;
        si.Volume = Mathf.Clamp(volume, 0f, 1f);
    }

#if ENABLE_DEBUG_GUI

	void OnGUI(){
		if (GUI.Button(new Rect(200, 0 , 100, 50), "background")) {
			Play("background");
		}

		if (GUI.Button(new Rect(300, 0 , 100, 50), "buttonClick")) {
			Play("buttonClick");
		}

		if (GUI.Button(new Rect(400, 0 , 100, 50), "swordHit")) {
			Play("swordHit");
		}

		if (GUI.Button(new Rect(500, 0 , 100, 50), "On/Off BG")) {
			BackgroundOn = !BackgroundOn;
		}

		if (GUI.Button(new Rect(600, 0 , 100, 50), "On/Off Eff")) {
			EffectOn = !EffectOn;
		}

		BackgroundVolume = GUI.HorizontalSlider (new Rect (500, 75, 100, 50), backgroundVolume, 0, 1);
		EffectVolume = GUI.HorizontalSlider (new Rect (600, 75, 100, 50), effectVolume, 0, 1);
	}

#endif
}
