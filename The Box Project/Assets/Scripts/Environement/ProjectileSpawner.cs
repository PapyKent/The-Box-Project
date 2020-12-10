using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class ProjectileSpawner : MonoBehaviour
{
	public void SetActive(bool active)
	{
		if (active != m_isActive)
		{
			m_isActive = active;
			if (m_isActive)
			{
				StartCoroutine(SpawnProjectiles());
			}
			else
			{
				StopAllCoroutines();
			}
		}
	}

	#region Private

	private void Start()
	{
		SetActive(m_startActive);
	}

	private IEnumerator SpawnProjectiles()
	{
		while (true)
		{
			Projectile projectile = ResourceManager.Instance.AcquireInstance<Projectile>(m_projectileToSpawn, transform);
			projectile.transform.rotation = transform.rotation;
			if (m_overrideProjectileSpeed)
			{
				projectile.OverrideSpeed(m_projectileSpeed);
			}
			yield return new WaitForSeconds(m_spawnCadence);
		}
	}

	[SerializeField]
	private bool m_startActive = true;
	[SerializeField]
	private Projectile m_projectileToSpawn = null;
	[SerializeField]
	private float m_spawnCadence = 1.0f;
	[SerializeField]
	private bool m_overrideProjectileSpeed = true;
	[SerializeField]
	private float m_projectileSpeed = 10.0f;

	[NonSerialized]
	private bool m_isActive = false;

	#endregion Private
}