using System.Collections;
using UnityEngine;

public class CharacterController : RaycastCollisionDetector
{
	public static CharacterController Instance
	{
		get
		{
			return s_instance;
		}
	}

	public void SetExternalForce(Vector2 externalForce)
	{
		m_externalForces += externalForce;
	}

	public void BouncePlayer(BoucingPlatform.BoucingConfig config)
	{
		m_lastBouncingConfig = config;
		m_isBouncing = true;
	}

	public bool FaceRight { get { return m_collisions.faceRight; } }

	#region Private

	protected override void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
			InputManager.Instance.RegisterOnJumpInput(OnJumpPressed, true);
			base.Awake();
		}
		else
			Destroy(this);
	}

	protected void OnDestroy()
	{
		InputManager.Instance?.RegisterOnJumpInput(OnJumpPressed, false);
	}

	private void Update()
	{
		m_animator.SetFloat("Speed", Mathf.Abs(m_inputs.x));
		m_animator.SetBool("IsJumping", m_isJumping);
		m_animator.SetFloat("YVelocity", m_velocity.y);
		m_animator.SetBool("IsGrounded", m_isGrounded);
	}

	private void FixedUpdate()
	{
		m_inputs = InputManager.Instance.DirectionalInput * new Vector2(m_groundSpeed, 0.0f);
		HandleSpriteDirection();
		m_collisions.Reset();
		CheckSideCollisions();
		if (m_externalForces.x > 0.0f && m_inputs.x < 0.0f && (m_collisions.left && m_collisions.leftHit.distance < Mathf.Epsilon)
			|| m_externalForces.x < 0.0f && m_inputs.x > 0.0f && (m_collisions.right && m_collisions.rightHit.distance < Mathf.Epsilon))
		{
			m_inputs.x = 0.0f;
		}

		if (m_velocity.y > 0.0f || m_externalForces.y > 0.0f)
			CheckAboveCollisions(ref m_collisions);
		CheckBelowCollisions(ref m_collisions);
		CheckIfGrounded();
		HandleJump();
		ApplyGravity();
		m_inputs.y = m_velocity.y * Time.fixedDeltaTime;
		Vector3 newPos = transform.position + (Vector3)m_inputs + (Vector3)m_externalForces;
		if (m_velocity.y < 0.0f || m_externalForces.y < 0.0f)
		{
			if (m_collisions.below && (newPos.y - (ColliderBounds.size.y / 2)) < m_collisions.belowHit.point.y)
			{
				newPos.y = m_collisions.belowHit.point.y + (ColliderBounds.size.y / 2);
				m_velocity.y = 0.0f;
				m_isJumping = false;
				m_isBouncing = false;
				m_isGrounded = true;
			}
		}
		else if (m_velocity.y > 0.0f || m_externalForces.y > 0.0f)
		{
			if (m_collisions.above && (newPos.y + (ColliderBounds.size.y / 2)) > m_collisions.aboveHit.point.y)
			{
				m_willHitAbove = true;
				newPos.y = m_collisions.aboveHit.point.y - (ColliderBounds.size.y / 2);
			}
			else
			{
				m_willHitAbove = false;
			}
		}

		if (m_inputs.x > 0.0f || m_externalForces.x > 0.0f)
		{
			if (m_collisions.right && (newPos.x + (ColliderBounds.size.x / 2) > m_collisions.rightHit.point.x))
			{
				if (m_externalForces.y == 0.0f ||
					m_externalForces.y > 0.0f && newPos.y - (ColliderBounds.size.y / 2) <= m_collisions.rightHit.point.y)
				{
					newPos.x = transform.position.x; //m_collisions.rightHit.point.x - (ColliderBounds.size.x / 2);
				}
			}
		}
		else if (m_inputs.x < 0.0f || m_externalForces.x < 0.0f)
		{
			if (m_collisions.left && (newPos.x - (ColliderBounds.size.x / 2) < m_collisions.leftHit.point.x))
			{
				if (m_externalForces.y == 0.0f
					|| m_externalForces.y < 0.0f && newPos.y - (ColliderBounds.size.y / 2) <= m_collisions.leftHit.point.y)
				{
					newPos.x = m_collisions.leftHit.point.x + (ColliderBounds.size.x / 2);
				}
			}
		}

		Collider2D collider2D = Physics2D.OverlapBox(ColliderBounds.center, ColliderBounds.size, 360.0f, m_platformMask);
		if (collider2D != null)
		{
			if (newPos.y > collider2D.bounds.center.y
				&& (newPos.x >= collider2D.bounds.min.x && newPos.x <= collider2D.bounds.max.x)
				&& (newPos.y - ColliderBounds.extents.y) < collider2D.bounds.max.y
				&& !m_willHitAbove)
			{
				newPos.y = collider2D.bounds.max.y + ColliderBounds.extents.y + Mathf.Epsilon;
				Debug.Log("Reajust above");
			}
			else if (newPos.y < collider2D.bounds.center.y
				&& (newPos.x >= collider2D.bounds.min.x && newPos.x <= collider2D.bounds.max.x)
				&& (newPos.y + ColliderBounds.extents.y) > collider2D.bounds.min.y)
			{
				newPos.y = collider2D.bounds.min.y - ColliderBounds.extents.y - Mathf.Epsilon;
				Debug.Log("Reajust below");
			}
			//This portion of code is used to put the player out of a moving plateform, from left/right. This is a bit extreme...
			//else if (newPos.x >= collider2D.bounds.min.x
			//	&& (newPos.y >= collider2D.bounds.min.y && newPos.y <= collider2D.bounds.max.y))
			//{
			//	newPos.x = collider2D.bounds.max.x + ColliderBounds.extents.x + Mathf.Epsilon;
			//	Debug.Log("Reajust right");
			//}
			//else if (newPos.x <= collider2D.bounds.max.x
			//	&& (newPos.y >= collider2D.bounds.min.y && newPos.y <= collider2D.bounds.max.y))
			//{
			//	newPos.x = collider2D.bounds.min.x - ColliderBounds.extents.x - Mathf.Epsilon;
			//	Debug.Log("Reajust left");
			//}
		}

		transform.position = newPos;
		m_externalForces = Vector2.zero;

		m_collisions.Reset();
		CheckSideCollisions();
		if (m_velocity.y > 0.0f || m_externalForces.y > 0.0f)
			CheckAboveCollisions(ref m_collisions);
		CheckBelowCollisions(ref m_collisions);
		CheckIfGrounded();
		//transform.Translate(m_inputs);
	}

	private void CheckSideCollisions()
	{
		if (m_collisions.faceRight)
			CheckRightCollisions(ref m_collisions);
		else
			CheckLeftCollisions(ref m_collisions);
	}

	private void HandleSpriteDirection()
	{
		if (m_inputs.x != 0.0f)
		{
			bool flipX = m_inputs.x < 0.0f;
			m_sprite.flipX = flipX;
			m_collisions.faceRight = !flipX;
		}
	}

	private void OnJumpPressed(bool jumpReleased)
	{
		bool nothing = true;
		if (!m_isJumping && !jumpReleased && m_isGrounded)
		{
			nothing = false;
			StartJump(true);
		}
		else if (!m_hasReleasedJump && m_isJumping && jumpReleased)
		{
			nothing = false;
			ReleaseJump();
		}
		if (nothing && !jumpReleased)
		{
			Debug.Log($"Ground Distance: {m_collisions.belowHit.distance}");
			//Debug.Break();
		}
	}

	private void StartJump(bool jump)
	{
		//play jump effect
		AudioManager.Instance.PlaySFX(AudioManager.SFXType.JUMP);

		//Here we start jumping
		m_isJumping = true;
		m_hasReleasedJump = false;
		m_velocity.y = jump ? m_jumpSpeed : m_lastBouncingConfig.BoucingSpeed;
		m_yJumpStart = transform.position.y;
		m_currentMaxJumpHeight = jump ? m_maxJumpHeight : m_lastBouncingConfig.BoucingHeight;
	}

	private void ReleaseJump()
	{
		m_hasReleasedJump = true;
		m_currentTimeVerticalSpeedCut = m_willHitAbove ? m_timeVerticalSpeedCut / 2 : m_timeVerticalSpeedCut;
		m_elapsedTimeVerticalSpeedCut = 0.0f;
	}

	private void HandleJump()
	{
		if (!m_isJumping && m_isBouncing && m_isGrounded)
		{
			StartJump(false);
		}
		else if ((m_isJumping && !m_hasReleasedJump) && ((transform.position.y - m_yJumpStart >= m_currentMaxJumpHeight) || (m_collisions.above && m_willHitAbove)))
		{
			ReleaseJump();
		}
		else if (m_isJumping && m_hasReleasedJump && m_elapsedTimeVerticalSpeedCut <= m_currentTimeVerticalSpeedCut)
		{
			m_isBouncing = false;
			float newVelocity = m_jumpSpeed * ((m_currentTimeVerticalSpeedCut - (m_elapsedTimeVerticalSpeedCut / m_currentTimeVerticalSpeedCut)) / m_currentTimeVerticalSpeedCut);
			if (newVelocity < m_maxFallSpeed)
				newVelocity = m_maxFallSpeed;
			m_velocity.y = newVelocity;
			m_elapsedTimeVerticalSpeedCut += Time.fixedDeltaTime;
		}
	}

	private void CheckIfGrounded()
	{
		if (m_collisions.below)
		{
			if (m_collisions.belowHit.distance <= m_groundedTolerance)
			{
				m_isGrounded = true;
				if (m_hasReleasedJump)
				{
					m_isJumping = false;
				}
			}
			else
			{
				m_isGrounded = false;
			}
		}
		else
		{
			m_isGrounded = false;
		}
	}

	private void ApplyGravity()
	{
		if ((!m_isGrounded && !m_isJumping) || (m_isJumping && m_hasReleasedJump && m_elapsedTimeVerticalSpeedCut >= m_timeVerticalSpeedCut))
		{
			if (m_velocity.y > m_maxFallSpeed)
			{
				float newVelocity = m_velocity.y + (m_gravity * Time.fixedDeltaTime);
				if (newVelocity < m_maxFallSpeed)
				{
					newVelocity = m_maxFallSpeed;
				}
				m_velocity.y = newVelocity;
			}
		}
	}

	[SerializeField]
	private float m_groundSpeed = 5.0f;
	[SerializeField]
	private float m_jumpSpeed = 20.0f;
	[SerializeField]
	private float m_maxJumpHeight = 5.0f;
	[SerializeField]
	private float m_gravity = -9.81f;
	[SerializeField]
	private float m_maxFallSpeed = -25.0f;
	[SerializeField]
	private float m_timeVerticalSpeedCut = 0.3f;
	[SerializeField]
	private LayerMask m_platformMask;
	[SerializeField]
	private float m_groundedTolerance = 0.35f;

	[Header("Graphics Settings")]
	[SerializeField]
	private SpriteRenderer m_sprite = null;

	[Header("Animation Settings")]
	[SerializeField]
	private Animator m_animator = null;

	private Vector2 m_inputs = Vector2.zero;
	private Vector2 m_externalForces = Vector2.zero;
	private Vector2 m_velocity = Vector2.zero;
	private CollisionInfo m_collisions;
	private bool m_isJumping = false;
	private float m_yJumpStart = 0.0f;
	private float m_elapsedTimeVerticalSpeedCut = 0.0f;
	private float m_currentMaxJumpHeight = 0.0f;

	private bool m_hasReleasedJump = false;
	private bool m_isGrounded = false;
	private bool m_willHitAbove = false;
	private float m_currentTimeVerticalSpeedCut = 0.0f;
	private bool m_isBouncing = false;
	private BoucingPlatform.BoucingConfig m_lastBouncingConfig;

	private static CharacterController s_instance = null;

	#endregion Private
}