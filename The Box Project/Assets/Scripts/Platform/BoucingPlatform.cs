using UnityEngine;

public class BoucingPlatform : Platform
{
	[System.Serializable]
	public struct BoucingConfig
	{
		public float BoucingSpeed { get { return m_boucingSpeed; } }
		public float BoucingHeight { get { return m_boucingHeight; } }

		[SerializeField]
		private float m_boucingSpeed;
		[SerializeField]
		private float m_boucingHeight;
	}

	#region Private

	protected override void Start()
	{
		base.Start();
		m_character = CharacterController.Instance;
	}

	private void FixedUpdate()
	{
		if (m_active)
		{
			m_collisions.Reset();
			CheckAboveCollisions(ref m_collisions);
			BounceCharacterOnPlatform();
		}
	}

	private bool IsSomeoneStandingOnPlatform()
	{
		return m_collisions.above && m_collisions.aboveHit.distance < Mathf.Epsilon;
	}

	private void BounceCharacterOnPlatform()
	{
		if (IsSomeoneStandingOnPlatform())
		{
			m_character.BouncePlayer(m_boucingConfig);
		}
	}

	[SerializeField]
	private BoucingConfig m_boucingConfig;

	private CollisionInfo m_collisions;
	private CharacterController m_character = null;

	#endregion Private
}