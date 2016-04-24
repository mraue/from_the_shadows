using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioService : MonoBehaviour
{
	public AudioClip[] clips;
	AudioSource _audioSource;

	void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void Play(int n, bool loop)
	{
//		_audioSource.Stop();
//		_audioSource.clip = clips[n];
//		_audioSource.loop = loop;
//		_audioSource.Play();
		StartCoroutine(Play(clips[n]));
	}

	const float FADE_OUT_DURATION = 0.5f;

	IEnumerator Play(AudioClip clip)
	{
		_audioSource.Stop();
		_audioSource.clip = clip;
		_audioSource.loop = false;
		_audioSource.volume = 1f;
		_audioSource.Play();
		yield return new WaitForSeconds(Mathf.Max(0, clip.length - FADE_OUT_DURATION));
		yield return FadeOut(FADE_OUT_DURATION);
	}

	IEnumerator FadeOut(float delay)
	{
		float _StartVolume = _audioSource.volume;

		float _StartTime = Time.time;

		while (Time.time < _StartTime + delay)
		{
			_audioSource.volume = _StartVolume + ((0f - _StartVolume) * ((Time.time - _StartTime) / delay));

			yield return null;
		}
	}
}
