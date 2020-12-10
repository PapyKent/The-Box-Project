using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileActivationTrigger : PlayerTrigger
{
	public override void OnPlayerTrigger(CharacterController character)
	{
		foreach (ProjectileSpawner spawner in m_spawners)
		{
			spawner.SetActive(true);
		}
	}

	#region private

	private void Start()
	{
		CheckpointManager.Instance.ReloadCheckpointRelay.RegisterListener(OnCheckpointReload, true);
	}

	private void OnDestroy()
	{
		CheckpointManager.Instance.ReloadCheckpointRelay.RegisterListener(OnCheckpointReload, false);
	}

	public void OnCheckpointReload()
	{
		foreach (ProjectileSpawner spawner in m_spawners)
		{
			spawner.SetActive(false);
		}
	}

	private void OnDrawGizmos()
	{
		UnityEditor.Handles.Label(transform.position, "Activator");
	}

	[SerializeField]
	private List<ProjectileSpawner> m_spawners = new List<ProjectileSpawner>();

	#endregion private
}