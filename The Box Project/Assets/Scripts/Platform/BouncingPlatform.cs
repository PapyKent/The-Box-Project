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
		if (m_collisions.aboveCollision.isColliding && m_collisions.aboveCollision.hit.distance < Mathf.Epsilon)
		{
			m_character = m_collisions.aboveCollision.hit.collider.GetComponentInParent<CharacterController>();
		}
		return m_character != null;
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