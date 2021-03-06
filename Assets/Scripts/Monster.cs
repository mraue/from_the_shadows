﻿using System;
using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
	public GameObject player;
	public float speed = 1f;
	public Action onGameOver;

	public bool isActive;

	public RandomAudioClipOnEnable audioPlayer;

	public void ShutDown(float delay)
	{
		if (!isActive)
		{
			return;
		}

		isActive = false;
		audioPlayer.Shutdown(delay);
		StartCoroutine(ShutDownCoroutine(delay));
	}

	IEnumerator ShutDownCoroutine(float delay)
	{
		yield return new WaitForSeconds(delay);
		yield return new WaitForEndOfFrame();
		gameObject.SetActive(false);
	}

	void Update()
	{
		var pos = transform.position;
		var vect = player.transform.position - pos;
		if (vect.magnitude > 0f)
		{			
			var fract = speed * Time.deltaTime / vect.magnitude;
			transform.position = pos + vect * fract;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!isActive)
		{
			return;
		}

		foreach (ContactPoint contact in collision.contacts)
		{
			if (contact.otherCollider != null && contact.otherCollider.gameObject != null)
			{				
				var player = contact.otherCollider.gameObject.GetComponent<Player>();
				
				if (player != null)
				{
					onGameOver();
				}
			}
		}
	}
}
