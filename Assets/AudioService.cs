using UnityEngine;

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
		_audioSource.Stop();
		_audioSource.clip = clips[n];
		_audioSource.loop = loop;
		_audioSource.Play();
	}
}
