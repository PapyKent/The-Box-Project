using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : RaycastCollisionDetector
{
	#region Private

	private void FixedUpdate()
	{
		m_collisions.Reset();
	}

	[Header("Projectile")]
	[SerializeField]
	private float m_speed = 1.0f;

	private CollisionInfo m_collisions;

	#endregion Private
}