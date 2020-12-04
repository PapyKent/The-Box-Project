using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTrigger : MonoBehaviour
{
	public abstract void OnPlayerTrigger(CharacterController character);

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			OnPlayerTrigger(collision.GetComponentInParent<CharacterController>());
		}
	}
}