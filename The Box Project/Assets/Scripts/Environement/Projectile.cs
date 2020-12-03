using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class Projectile : RaycastCollisionDetector
{
	#region Private

	private void FixedUpdate()
	{
		transform.position += m_speed * Time.fixedDeltaTime * transform.right;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Ground")
		{
			ResourceManager.Instance.ReleaseInstance(this);
		}
		else if (collision.tag == "Player")
		{
			Debug.Log("Projo collide player");
			ResourceManager.Instance.ReleaseInstance(this);
		}
	}

	[Header("Projectile")]
	[SerializeField]
	private float m_speed = 1.0f;

	private CollisionInfo m_collisions;

	#endregion Private
}