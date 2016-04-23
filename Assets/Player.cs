using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float xSpeedMax = 1f;
	public float xIncrement = 0.2f;

	public float xMax = 10f;
	public float yMax = 10f;
	public float xMin = 0f;
	public float yMin = 0f;

	public ParticleSystem destroyedParticles;

	float _xSpeed;
	float _ySpeed;

	float _jumpDelay;

	const float JUMP_DELAY = 0.8f;

	Vector3 _target;

	public bool canMove = true;

	public void Destroyed()
	{
		canMove = false;
		StartCoroutine(DestroyedCoroutine());
	}

	IEnumerator DestroyedCoroutine()
	{
		destroyedParticles.Play();
		var scale = 0.5f;
		while (scale > 0)
		{
			scale -= 0.1f;
			transform.localScale = new Vector3(scale, scale, scale);
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(1f);

		destroyedParticles.Stop();

		gameObject.SetActive(false);
	}

	void Update()
	{
		if (!canMove)
		{
			return;
		}

		if (_jumpDelay > 0)
		{
			var scale = 1f;

			if (_jumpDelay > JUMP_DELAY / 2f)
			{
				scale = 0.5f * (_jumpDelay - JUMP_DELAY / 2f) / (JUMP_DELAY / 2f);
			}
			else
			{
				scale = 0.5f * (_jumpDelay - JUMP_DELAY / 2) / JUMP_DELAY * 2f;
				transform.position = _target;
			}

			transform.localScale = new Vector3(scale, scale, scale);

			_jumpDelay -= Time.deltaTime;

			return;
		}

		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{  
			_xSpeed = Mathf.Min(xSpeedMax, _xSpeed + xIncrement);
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			_xSpeed = Mathf.Max(-xSpeedMax, _xSpeed - xIncrement);
		}
		else
		{
			_xSpeed = _xSpeed > 0f ? Mathf.Max(0f, _xSpeed - xIncrement) : Mathf.Min(0f, _xSpeed + xIncrement);
		}

		if (_xSpeed != 0f)
		{			
			var pos = transform.position;
			transform.position = new Vector3(pos.x + _xSpeed * Time.deltaTime, pos.y, pos.z);
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{  
			_ySpeed = Mathf.Min(xSpeedMax, _ySpeed + xIncrement);
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			_ySpeed = Mathf.Max(-xSpeedMax, _ySpeed - xIncrement);
		}
		else
		{
			_ySpeed = _ySpeed > 0f ? Mathf.Max(0f, _ySpeed - xIncrement) : Mathf.Min(0f, _ySpeed + xIncrement);
		}

		if (_ySpeed != 0f)
		{			
			var pos = transform.position;
			transform.position = new Vector3(pos.x, pos.y + _ySpeed * Time.deltaTime, pos.z);
		}

		var newPos = transform.position;

		if (newPos.x > xMax)
		{
			newPos.x = xMin;
			_jumpDelay = JUMP_DELAY;
		}
		else if (newPos.x < xMin)
		{
			newPos.x = xMax;
			_jumpDelay = JUMP_DELAY;
		}

		if (newPos.y > yMax)
		{
			newPos.y = yMin;
			_jumpDelay = JUMP_DELAY;
		}
		else if (newPos.y < yMin)
		{
			newPos.y = yMax;
			_jumpDelay = JUMP_DELAY;
		}
			
		_target = newPos;
	}
}
