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

	[SerializeField]
	private List<ProjectileSpawner> m_spawners = new List<ProjectileSpawner>();

	#endregion private
}