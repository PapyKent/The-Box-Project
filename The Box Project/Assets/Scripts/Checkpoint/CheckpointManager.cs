using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;
using Yube.Relays;

public class CheckpointManager : Singleton<CheckpointManager>
{
	public Checkpoint CurrentCheckpoint { get { return m_currentCheckpoint; } set { m_currentCheckpoint = value; } }

	public IRelayLink ReloadCheckpointRelay { get { return m_reloadCheckpointRelay ?? (m_reloadCheckpointRelay = new Relay()); } }

	public void ReloadLevel()
	{
		Player.transform.position = m_currentCheckpoint.transform.position;
		m_reloadCheckpointRelay?.Dispatch();
	}

#if UNITY_EDITOR

	public void LoadCheckpoint(Checkpoint checkpoint)
	{
		m_currentCheckpoint = checkpoint;
		ReloadLevel();
	}

#endif

	#region Private

	private CharacterController Player { get { return (m_player ?? (m_player = FindObjectOfType<CharacterController>())); } }

	protected override void Awake()
	{
		base.Awake();
		if (m_checkPoints.Count == 0)
		{
			Debug.LogError("No checkpoints filled in CheckpointManager.");
			return;
		}
		m_currentCheckpoint = m_checkPoints[0];
	}

	[SerializeField]
	private List<Checkpoint> m_checkPoints = new List<Checkpoint>();

	private Checkpoint m_currentCheckpoint = null;
	private CharacterController m_player = null;
	private Relay m_reloadCheckpointRelay = null;

	#endregion Private
}