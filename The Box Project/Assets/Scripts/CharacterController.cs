using UnityEngine;

public class CharacterController : RaycastCollisionDetector
{
	public static CharacterController Instance { get { return s_instance; } }

	public void SetExternalForce(Vector2 externalForce)
	{
		m_externalForces += externalForce;
	}

	#region Private

	private void Awake()
	{
		if (s_instance == null)
			s_instance = this;
		else
			Destroy(this);
	}

	private void Update()
	{
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

		if (m_velocity.y > 0.0f)
			CheckAboveCollisions(ref m_collisions);
		else
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
				m_isGrounded = true;
			}
		}
		else if (m_velocity.y > 0.0f || m_externalForces.y > 0.0f)
		{
			if (m_collisions.above && (newPos.y + (ColliderBounds.size.y / 2)) > m_collisions.aboveHit.point.y)
			{
				//if (m_velocity.y > 0.0f)
				//    m_velocity.y = 0.0f;
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
				newPos.x = transform.position.x;// m_collisions.rightHit.point.x - (m_groundCollider.ColliderBounds.size.x / 2);
			}
		}
		else if (m_inputs.x < 0.0f || m_externalForces.x < 0.0f)
		{
			if (m_collisions.left && (newPos.x - (ColliderBounds.size.x / 2) < m_collisions.leftHit.point.x))
			{
				newPos.x = m_collisions.leftHit.point.x + (ColliderBounds.size.x / 2);
			}
		}
		transform.position = newPos;
		m_externalForces = Vector2.zero;
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
			bool flipX = m_inputs.x > 0.0f;
			m_sprite.flipX = flipX;
			m_collisions.faceRight = flipX;
		}
	}

	private void HandleJump()
	{
		m_pressedJump = InputManager.Instance.PressedJump;
		m_releasedJump = InputManager.Instance.ReleasedJump;

		if (!m_isJumping && m_pressedJump && m_isGrounded)
		{
			//play jump effect
			AudioManager.Instance.PlaySFX(AudioManager.SFXType.JUMP);

			//Here we start jumping
			m_isJumping = true;
			m_hasReleasedJump = false;
			m_velocity.y = m_jumpSpeed;
			m_yJumpStart = transform.position.y;
		}
		else if (!m_hasReleasedJump && (m_isJumping && m_releasedJump || (transform.position.y - m_yJumpStart >= m_maxJumpHeight) || (m_collisions.above && m_willHitAbove)))
		{
			m_hasReleasedJump = true;
			m_currentTimeVerticalSpeedCut = m_willHitAbove ? m_timeVerticalSpeedCut / 2 : m_timeVerticalSpeedCut;
			m_elapsedTimeVerticalSpeedCut = 0.0f;
		}

		if (m_isJumping && m_hasReleasedJump && m_elapsedTimeVerticalSpeedCut <= m_currentTimeVerticalSpeedCut)
		{
			float newVelocity = m_jumpSpeed * ((m_currentTimeVerticalSpeedCut - (m_elapsedTimeVerticalSpeedCut / m_currentTimeVerticalSpeedCut)) / m_currentTimeVerticalSpeedCut);
			if (newVelocity < m_maxFallSpeed)
				newVelocity = m_maxFallSpeed;
			m_velocity.y = newVelocity;
			m_elapsedTimeVerticalSpeedCut += Time.deltaTime;
		}
	}

	private void CheckIfGrounded()
	{
		if (m_collisions.below)
		{
			if (Mathf.Abs(m_collisions.belowHit.point.y - ColliderBounds.min.y) < 0.05f)
			{
				m_isGrounded = true;
				m_isJumping = false;
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

	[Header("Graphics Settings")]
	[SerializeField]
	private SpriteRenderer m_sprite = null;

	private Vector2 m_inputs = Vector2.zero;
	private Vector2 m_externalForces = Vector2.zero;
	private Vector2 m_velocity = Vector2.zero;
	private CollisionInfo m_collisions;
	private bool m_isJumping = false;
	private bool m_pressedJump = false;
	private bool m_releasedJump = false;
	private float m_yJumpStart = 0.0f;
	private float m_elapsedTimeVerticalSpeedCut = 0.0f;

	private bool m_hasReleasedJump = false;
	private bool m_isGrounded = false;
	private bool m_willHitAbove = false;
	private float m_currentTimeVerticalSpeedCut = 0.0f;

	private static CharacterController s_instance = null;

	#endregion Private
}