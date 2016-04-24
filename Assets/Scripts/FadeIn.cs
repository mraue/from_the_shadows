using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeIn : MonoBehaviour
{
	CanvasGroup _canvasGroup;

	public float fadeInDuration = 0.5f;
	float _current;

	void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	void Update()
	{
		_current += Time.deltaTime;

		if (_current < fadeInDuration)
		{			
			_canvasGroup.alpha = _current / fadeInDuration * 0.8f;
		}
	}

	void OnEnable()
	{
		_canvasGroup.alpha = 0;
		_current = 0;
	}
}
