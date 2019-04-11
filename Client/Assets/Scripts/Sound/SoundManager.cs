using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioListener))]
public class SoundManager : SingletonMono<SoundManager> {
	AudioSource[] lsAudio;
	AudioListener auListener;
	bool isMute;

	public bool IsMute {
		get {
			return AudioListener.pause;
		}
	}

	// Use this for initialization
	void Start () {
		auListener = GetComponent<AudioListener> ();
		lsAudio = GetComponentsInChildren<AudioSource> ();
	}

	public void MuteAll(bool isMute){
		AudioListener.pause = isMute;
	}

	public void StopAll(){
		for (int i = 0; i < lsAudio.Length; i++) {
			lsAudio[i].Stop();
		}
	}

	public AudioSource GetSound(string sName){	
		Transform t = transform.Find (sName);
		AudioSource s = null;
		if (t != null) {
			s = t.GetComponent<AudioSource>();
		}

		return s;
	}

	public void Play(string sName, bool isLoop = false){
		AudioSource s = GetSound (sName);
		if (s != null) {
			s.loop = isLoop;
			s.Play ();
		} else {
			Debug.LogWarning(sName + " not found");
		}
	}

	public void Stop(string sName){
		AudioSource s = GetSound (sName);
		if (s != null) {
			s.Stop();
		} else {
			Debug.LogWarning(sName + "not found");
		}
	}

	public void Pause(string sName){
		AudioSource s = GetSound (sName);
		if (s != null) {
			s.Pause();
		} else {
			Debug.LogWarning(sName + "not found");
		}
	}

	public void Resume(string sName){
		AudioSource s = GetSound (sName);
		if (s != null) {
			s.UnPause();
		} else {
			Debug.LogWarning(sName + "not found");
		}
	}
}
