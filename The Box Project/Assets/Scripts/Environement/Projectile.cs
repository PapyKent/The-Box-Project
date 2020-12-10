﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class Projectile : RaycastCollisionDetector
{
	public void OverrideSpeed(float newSpeed)
	{
		m_currentSpeed = newSpeed;
	}

	#region Private

	protected override void Awake()
	{
		base.Awake();
		m_currentSpeed = m_speed;
	}

	private void OnDisable()
	{
		m_currentSpeed = m_speed;
	}

	private void FixedUpdate()
	{
		transform.position += m_currentSpeed * Time.fixedDeltaTime * transform.right;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Debug.Log("Projo collide player");
			CheckpointManager.Instance.ReloadLevel();
			ResourceManager.Instance.ReleaseInstance(this);
		}
		else
		{
			ActionBlock actionBlock = collision.GetComponent<ActionBlock>();
			if (actionBlock != null)
			{
				actionBlock.OnBlockTrigger(collision, this);
			}
			else if (collision.tag == "Ground")
			{
				ResourceManager.Instance.ReleaseInstance(this);
			}
		}
	}

	[Header("Projectile")]
	[SerializeField]
	private float m_speed = 1.0f;

	[NonSerialized]
	private float m_currentSpeed = 0.0f;

	private CollisionInfo m_collisions;

	#endregion Private
}