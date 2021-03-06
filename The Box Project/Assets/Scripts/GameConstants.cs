﻿using UnityEngine;

[CreateAssetMenu(menuName = "Game/Constants")]
public class GameConstants : ScriptableObject
{
	public float ConstantDistanceToGround { get { return m_constantDistanceToGround; } }
	public float MovingPlateformCollisionCheck { get { return m_movingPlateformCollisionCheck; } }
	public float EjectDistance { get { return m_ejectDistance; } }

	#region Private

	[Header("Physics Settings")]
	[SerializeField]
	private float m_constantDistanceToGround = 0.0001f;
	[SerializeField, Tooltip("Should be less than Constant Distance To Ground")]
	private float m_movingPlateformCollisionCheck = 0.001f;
	[SerializeField]
	private float m_ejectDistance = 0.001f;

	#endregion Private
}