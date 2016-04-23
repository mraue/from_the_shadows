using UnityEngine;

public class Column : MonoBehaviour
{
	public float height;
	public float growthDuration;

	float _currentDuration;

	public bool isActive;

	void Update()
	{
		if (!isActive)
		{
			return;
		}

		if (_currentDuration < growthDuration)
		{
			_currentDuration += Time.deltaTime;
			var fract = _currentDuration / growthDuration;
			var scale = transform.localScale;
			transform.localScale = new Vector3(scale.x, scale.y, 2 * height * fract);
		}
	}

	public void Reset()
	{
		height = 0f;
		growthDuration = 1f;
		_currentDuration = 0f;
	}
}
