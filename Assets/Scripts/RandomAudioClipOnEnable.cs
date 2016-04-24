using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioClipOnEnable : MonoBehaviour
{
	public AudioClip[] audioClips;
	AudioSource _audioSource;

	void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	void OnEnable()
	{
		if (audioClips.Length > 0)
		{
			_audioSource.Stop();
			_audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
			_audioSource.volume = 1f;
			_audioSource.Play();
		}
	}

	public void Shutdown(float delay)
	{		
		StartCoroutine(ShutdownCoroutine(delay));
	}

	IEnumerator ShutdownCoroutine(float delay)
	{
		float _StartVolume = _audioSource.volume;

		float _StartTime = Time.time;

		while (Time.time < _StartTime + delay)
		{
			_audioSource.volume = _StartVolume + ((0f - _StartVolume) * ((Time.time - _StartTime) / delay));

			yield return null;
		}
	}

	void OnDisable()
	{
		if (_audioSource.isPlaying)
		{
			_audioSource.volume = 0f;
			_audioSource.Stop();
		}
	}
}
