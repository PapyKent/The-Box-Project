using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class CheckpointManager : Singleton<CheckpointManager>
{
	public Checkpoint CurrentCheckpoint { get { return m_currentCheckpoint; } set { m_currentCheckpoint = value; } }

	public void ReloadLevel()
	{
		Player.transform.position = m_currentCheckpoint.transform.position;
	}

	#region Private

	private CharacterController Player { get { return (m_player ?? (m_player = FindObjectOfType<CharacterController>())); } }

	protected override void Awake()
	{
		base.Awake();
		m_currentCheckpoint = m_beginCheckpoint;
	}

	[SerializeField]
	private Checkpoint m_beginCheckpoint = null;

	private Checkpoint m_currentCheckpoint = null;
	private CharacterController m_player = null;

	#endregion Private
}