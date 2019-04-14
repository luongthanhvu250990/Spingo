using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundItem : MonoBehaviour {	
	[SerializeField] AudioClip audClip;

	[SerializeField][Range(0, 1)] float volume = 1.0f;
	public float Volume {
		get {
			return volume;
		}
		set {
			volume = value;
			for (int i = 0; i < playingList.Count; i++) {
				playingList [i].volume = volume * (SndType == SoundType.Background ? SoundManager.Instance.BackgroundVolume : SoundManager.Instance.EffectVolume);
			}
		}
	}

	List<AudioSource> playingList = new List<AudioSource> ();
	public AudioClip AudClip {
		get {
			return audClip;
		}
		set {
			audClip = value;
		}
	}

	[SerializeField] SoundType sndType;
	public SoundType SndType {
		get {
			return sndType;
		}
		set {
			sndType = value;
		}
	}

	public void Play(){
		switch (SndType) {
		case SoundType.Background: 
		case SoundType.Effect_Single:
			if (playingList.Count > 0) {
				playingList [0].volume = Volume * (SndType == SoundType.Background ? SoundManager.Instance.BackgroundVolume : SoundManager.Instance.EffectVolume);
				playingList [0].Play ();
			} else {
				GameObject go = new GameObject (this.name);
				go.transform.parent = this.transform;
				AudioSource aus = go.AddComponent<AudioSource> ();
				aus.clip = AudClip;
				playingList.Add (aus);					
				aus.loop = SndType == SoundType.Background;
				aus.playOnAwake = false;
				aus.volume = Volume * (SndType == SoundType.Background ? SoundManager.Instance.BackgroundVolume : SoundManager.Instance.EffectVolume);
				aus.Play ();
			}
			break;

		case SoundType.Effect_Multiple: 
			AudioSource _aus = null;
			for (int i = 0; i < playingList.Count; i++) {
				if (!playingList [i].isPlaying) {
					_aus = playingList [i];
					break;
				}				
			}		
			if (_aus == null) {
				GameObject go = new GameObject (this.name);
				go.transform.parent = this.transform;
				_aus = go.AddComponent<AudioSource> ();
				_aus.clip = AudClip;
				playingList.Add (_aus);					
				_aus.loop = false;
				_aus.playOnAwake = false;
			}
			_aus.volume = Volume * SoundManager.Instance.EffectVolume;
			_aus.Play ();
			break;
		default:
			break;
		}
	}

	public void Stop(){
		for (int i = 0; i < playingList.Count; i++) {
			playingList [i].Stop ();
		}	
	}

	public void PauseBackground(){
		for (int i = 0; i < playingList.Count; i++) {
			playingList [i].Pause ();
		}	
	}

	public void UnpauseBackground(){
		for (int i = 0; i < playingList.Count; i++) {
			playingList [i].UnPause ();
		}	
	}

	public void RefreshVolume(){
		for (int i = 0; i < playingList.Count; i++) {
			playingList [i].volume = Volume * (SndType == SoundType.Background ? SoundManager.Instance.BackgroundVolume : SoundManager.Instance.EffectVolume);
		}	
	}
}

