using UnityEngine;

public class BouncingPlatform : Platform
{
	[System.Serializable]
	public struct BouncingConfig
	{
		public float BoucingSpeed { get { return m_boucingSpeed; } }
		public float BoucingHeight { get { return m_boucingHeight; } }

		[SerializeField]
		private float m_boucingSpeed;
		[SerializeField]
		private float m_boucingHeight;
	}

	#region Private

	protected override void Awake()
	{
		base.Awake();
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
	private BouncingConfig m_boucingConfig = new BouncingConfig();

	private CollisionInfo m_collisions = new CollisionInfo();
	private CharacterController m_character = null;

	#endregion Private
}