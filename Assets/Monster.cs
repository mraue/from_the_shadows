using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
	public GameObject player;
	public float speed = 1f;
	public Action onGameOver;

	public bool isActive = false;

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
