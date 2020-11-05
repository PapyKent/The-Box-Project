using System.Collections;
using UnityEngine;
using Yube.Relays;

public class CharacterController : RaycastCollisionDetector
{
	public void SetExternalForce(Vector2 externalForce)
	{
		m_externalForces += externalForce;
	}

	public void BouncePlayer(BouncingPlatform.BouncingConfig config)
	{
		m_lastBouncingConfig = config;
		m_isBouncing = true;
	}

	public bool FaceRight { get { return m_collisions.faceRight; } }
	public IRelayLink<bool, bool> JumpRelay { get { return m_jumpRelay ?? (m_jumpRelay = new Relay<bool, bool>()); } }
	public IRelayLink<bool> GroundedRelay { get { return m_groundedRelay ?? (m_groundedRelay = new Relay<bool>()); } }
	public float XSpeed { get { return Mathf.Abs(m_inputs.x); } }
	public float YVelocity { get { return m_velocity.y; } }

	#region Private

	public bool IsJumping
	{
		get { return m_isJumping; }
		set
		{
			if (value != m_isJumping)
			{
				m_isJumping = value;
				m_jumpRelay?.Dispatch(m_isJumping, m_isBouncing);
			}
		}
	}

	protected bool IsGrounded
	{
		get { return m_isGrounded; }
		set
		{
			if (value != m_isGrounded)
			{
				m_isGrounded = value;
				m_groundedRelay?.Dispatch(m_isGrounded);
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		InputManager.Instance.RegisterOnJumpInput(OnJumpPressed, true);
	}

	protected void OnDestroy()
	{
		InputManager.Instance?.RegisterOnJumpInput(OnJumpPressed, false);
	}

	private void FixedUpdate()
	{
		m_inputs = InputManager.Instance.DirectionalInput * new Vector2(m_groundSpeed, 0.0f);
		m_collisions.Reset();
		if (m_inputs.x != 0)
		{
			m_collisions.faceRight = m_inputs.x > 0.0f;
		}
		CheckSideCollisions();
		if (m_externalForces.x > 0.0f && m_inputs.x < 0.0f && (m_collisions.left && m_collisions.leftHit.distance < Mathf.Epsilon)
			|| m_externalForces.x < 0.0f && m_inputs.x > 0.0f && (m_collisions.right && m_collisions.rightHit.distance < Mathf.Epsilon))
		{
			m_inputs.x = 0.0f;
			Debug.Log("Input to 0");
			//Debug.Break();
		}

		if (m_velocity.y > 0.0f || m_externalForces.y > 0.0f)
			CheckAboveCollisions(ref m_collisions);
		CheckBelowCollisions(ref m_collisions);
		CheckIfGrounded();
		HandleJump();
		ApplyGravity();
		m_inputs.y = m_velocity.y * Time.fixedDeltaTime;
		Vector3 newPos = transform.position + (Vector3)m_inputs + (Vector3)m_externalForces;
		m_willHitAbove = false;
		if (m_velocity.y < 0.0f || m_externalForces.y < 0.0f)
		{
			if (m_collisions.below && (newPos.y - (ColliderBounds.size.y / 2)) < m_collisions.belowHit.point.y)
			{
				newPos.y = m_collisions.belowHit.point.y + (ColliderBounds.size.y / 2) + GameManager.Instance.GameConstants.ConstantDistanceToGround;
				m_velocity.y = 0.0f;
				IsJumping = false;
				m_isBouncing = false;
				IsGrounded = true;
			}
		}
		else if (m_velocity.y > 0.0f || m_externalForces.y > 0.0f)
		{
			if (m_collisions.above && (newPos.y + (ColliderBounds.size.y / 2)) > m_collisions.aboveHit.point.y)
			{
				m_willHitAbove = true;
				newPos.y = m_collisions.aboveHit.point.y - (ColliderBounds.size.y / 2);
				if (!m_hasReleasedJump)
				{
					ReleaseJump();
				}
				else
				{
					m_velocity.y = 0.0f;//to not have the impression to be bounced down
					m_elapsedTimeVerticalSpeedCut = m_currentTimeVerticalSpeedCut + 1;
				}
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
				&& Utils.IsInferior((newPos.y - ColliderBounds.extents.y), collider2D.bounds.max.y, true)
				&& !m_willHitAbove)
			{
				newPos.y = collider2D.bounds.max.y + ColliderBounds.extents.y + GameManager.Instance.GameConstants.EjectDistance;
				Debug.Log("Reajust above");
			}
			else if (newPos.y < collider2D.bounds.center.y
				&& (newPos.x >= collider2D.bounds.min.x && newPos.x <= collider2D.bounds.max.x)
				&& Utils.IsSuperior((newPos.y + ColliderBounds.extents.y), collider2D.bounds.min.y, true))
			{
				newPos.y = collider2D.bounds.min.y - ColliderBounds.extents.y - GameManager.Instance.GameConstants.EjectDistance;
				Debug.Log("Reajust below");
			}
			else if (newPos.x >= collider2D.bounds.center.x //Eject right
				&& (collider2D.bounds.min.y - ColliderBounds.extents.y) < (newPos.y + Mathf.Epsilon) && (newPos.y - Mathf.Epsilon) < (collider2D.bounds.max.y + ColliderBounds.extents.y)
				&& Utils.IsInferior((newPos.x - ColliderBounds.extents.x), collider2D.bounds.max.x, true))
			{
				newPos.x = collider2D.bounds.max.x + ColliderBounds.extents.x + GameManager.Instance.GameConstants.EjectDistance;
				Debug.Log("Reajust right " + IsJumping);
			}
			else if (newPos.x <= collider2D.bounds.center.x //Eject left
				&& (collider2D.bounds.min.y - ColliderBounds.extents.y) < (newPos.y + Mathf.Epsilon) && (newPos.y - Mathf.Epsilon) < (collider2D.bounds.max.y + ColliderBounds.extents.y)
				&& Utils.IsSuperior((newPos.x + ColliderBounds.extents.x), collider2D.bounds.min.x, true))
			{
				newPos.x = collider2D.bounds.min.x - ColliderBounds.extents.x - GameManager.Instance.GameConstants.EjectDistance;
				Debug.Log("Reajust left " + IsJumping);
			}
		}

		transform.position = newPos;
		m_externalForces = Vector2.zero;

		m_collisions.Reset();
		CheckSideCollisions();
		if (m_velocity.y > 0.0f || m_externalForces.y > 0.0f)
			CheckAboveCollisions(ref m_collisions);
		CheckBelowCollisions(ref m_collisions);
		CheckIfGrounded();
	}

	private void CheckSideCollisions()
	{
		if (m_collisions.faceRight)
			CheckRightCollisions(ref m_collisions);
		else
			CheckLeftCollisions(ref m_collisions);
	}

	private void OnJumpPressed(bool jumpReleased)
	{
		bool nothing = true;
		if (!IsJumping && !jumpReleased && IsGrounded)
		{
			nothing = false;
			StartJump(true);
		}
		else if (!m_hasReleasedJump && IsJumping && jumpReleased)
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
		IsJumping = true;
		m_hasReleasedJump = false;
		m_yJumpStart = transform.position.y;
		m_velocity.y = jump ? m_jumpSpeed : m_lastBouncingConfig.BoucingSpeed;
		m_currentMaxJumpHeight = jump ? m_maxJumpHeight : m_lastBouncingConfig.BoucingHeight;

		Debug.Log("Start jump");
	}

	private void ReleaseJump()
	{
		m_hasReleasedJump = true;
		if (m_willHitAbove)
		{
			m_velocity.y = 1.0f;//to not have the impression to be bounced down
			m_elapsedTimeVerticalSpeedCut = m_currentTimeVerticalSpeedCut + 1;
		}
		else
		{
			m_yVelocityBeforeVerticalSpeedCut = m_velocity.y;
			m_elapsedTimeVerticalSpeedCut = 0.0f;
			m_currentTimeVerticalSpeedCut = m_timeVerticalSpeedCut;
		}
		Debug.Log("Release Jump");
	}

	private void HandleJump()
	{
		if (!IsJumping && m_isBouncing && IsGrounded)
		{
			StartJump(false);
		}
		else if ((IsJumping && !m_hasReleasedJump) && ((transform.position.y - m_yJumpStart >= m_currentMaxJumpHeight) || (m_collisions.above && m_willHitAbove)))
		{
			ReleaseJump();
		}
		else if (IsJumping && m_hasReleasedJump && m_elapsedTimeVerticalSpeedCut <= m_currentTimeVerticalSpeedCut)
		{
			m_isBouncing = false;
			float newVelocity = Mathf.Lerp(m_yVelocityBeforeVerticalSpeedCut, 0.0f, m_elapsedTimeVerticalSpeedCut / m_currentTimeVerticalSpeedCut);
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
				IsGrounded = true;
				if (m_hasReleasedJump)
				{
					IsJumping = false;
				}
			}
			else
			{
				IsGrounded = false;
			}
		}
		else
		{
			IsGrounded = false;
		}
	}

	private void ApplyGravity()
	{
		if ((!IsGrounded && !IsJumping) || (IsJumping && m_hasReleasedJump && m_elapsedTimeVerticalSpeedCut >= m_currentTimeVerticalSpeedCut))
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
	private float m_timeVerticalSpeedCut = 2.0f;
	[SerializeField]
	private LayerMask m_platformMask = 0;
	[SerializeField]
	private float m_groundedTolerance = 0.35f;

	private Vector2 m_inputs = Vector2.zero;
	private Vector2 m_externalForces = Vector2.zero;
	private Vector2 m_velocity = Vector2.zero;
	private CollisionInfo m_collisions;
	private float m_yJumpStart = 0.0f;
	private float m_currentTimeVerticalSpeedCut = 0.0f;
	private float m_elapsedTimeVerticalSpeedCut = 0.0f;
	private float m_yVelocityBeforeVerticalSpeedCut = 0.0f;
	private float m_currentMaxJumpHeight = 0.0f;

	private bool m_hasReleasedJump = false;
	private bool m_willHitAbove = false;
	private bool m_isBouncing = false;
	private bool m_isJumping = false;
	private bool m_isGrounded = false;
	private BouncingPlatform.BouncingConfig m_lastBouncingConfig;

	public Relay<bool, bool> m_jumpRelay = null;
	public Relay<bool> m_groundedRelay = null;

	#endregion Private
}