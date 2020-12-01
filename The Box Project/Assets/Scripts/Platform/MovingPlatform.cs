using UnityEngine;

public class MovingPlatform : Platform
{
	public void ResetMovingPlatform()
	{
		m_isMoving = false;
		percentBetweenWaypoints = 0.0f;
		transform.position = m_startingPos;
		globalWaypointsCreation();
		m_characterStandingOnPlatform = false;
		if (m_alwaysMove)
			m_isMoving = true;
		Start();
	}

	public void SetCurrentPlayer(bool isCharStanding)
	{
		m_characterStandingOnPlatform = isCharStanding;
		if (isCharStanding)
			m_isMoving = true;
	}

	#region Private

	protected override void Awake()
	{
		base.Awake();
		m_startingPos = transform.position;
	}

	protected void Start()
	{
		globalWaypointsCreation();
		if (m_alwaysMove)
		{
			m_isMoving = true;
		}
		if (m_currentColor == Color.NONE)
			m_active = true;
	}

	private void globalWaypointsCreation()
	{
		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < globalWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i] + m_startingPos;
		}
	}

	private void FixedUpdate()
	{
		m_collisions.Reset();
		if (m_active)
		{
			CheckAboveCollisions(ref m_collisions);
			CheckIfPlayerIsOnPlatform();
		}
		if (m_isMoving)
		{
			Vector3 velocity = CalculatePlatformMovement();
			if (m_active)
			{
				CheckLeftCollisions(ref m_collisions);
				CheckRightCollisions(ref m_collisions);
				if (velocity.y < 0.0f)
					CheckBelowCollisions(ref m_collisions);
				CalculatePassengerMovement(velocity);
			}
			transform.Translate(velocity);
		}
	}

	protected override void OnColorButtonPressed(Color newColor)
	{
		base.OnColorButtonPressed(newColor);
		if (!m_active)
		{
			SetCurrentPlayer(false);
		}
	}

	private void CheckIfPlayerIsOnPlatform()
	{
		if (m_collisions.aboveCollision.isColliding
			&& m_collisions.aboveCollision.hit.distance <= GameManager.Instance.GameConstants.MovingPlateformCollisionCheck)
		{
			SetCurrentPlayer(true);
			m_player = m_collisions.aboveCollision.hit.collider.GetComponentInParent<CharacterController>();
		}
		else
		{
			SetCurrentPlayer(false);
			m_player = null;
		}
	}

	private Vector3 CalculatePlatformMovement()
	{
		if (Time.time < nextMoveTime)
		{
			return Vector3.zero;
		}

		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * m_speed / distanceBetweenWaypoints;

		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
		float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1)
		{
			percentBetweenWaypoints = 0f;
			fromWaypointIndex++;

			if (!m_cyclic)
			{
				if (fromWaypointIndex >= globalWaypoints.Length - 1)
				{
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
				}
			}

			nextMoveTime = Time.time + m_waitTIme;
		}

		return newPos - transform.position;
	}

	private void CalculatePassengerMovement(Vector3 velocity)
	{
		if (m_characterStandingOnPlatform)
		{
			m_player.SetExternalForce(velocity);
		}
		else if (velocity.x > 0.0f || velocity.x < 0.0f)
		{
			//if (m_collisions.right && Mathf.Abs(m_collisions.rightHit.distance) < Mathf.Epsilon
			//	|| m_collisions.left && Mathf.Abs(m_collisions.leftHit.distance) < Mathf.Epsilon)
			//{
			//	m_player.SetExternalForce(new Vector2(velocity.x, 0.0f));
			//}
		}
		else if (velocity.y < 0.0f)
		{
			if (m_collisions.belowCollision.isColliding
				&& Mathf.Abs(m_collisions.belowCollision.hit.distance) <= GameManager.Instance.GameConstants.MovingPlateformCollisionCheck)
			{
				m_player.SetExternalForce(new Vector2(0.0f, velocity.y));
			}
		}
	}

	private float Ease(float x)
	{
		float a = easeAmount + 1f;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}

	private void OnDrawGizmos()
	{
		if (localWaypoints != null)
		{
			Gizmos.color = UnityEngine.Color.red;
			float size = .3f;

			for (int i = 0; i < localWaypoints.Length; i++)
			{
				Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}

	[Header("Moving properties")]
	[SerializeField]
	private bool m_alwaysMove = false;
	[SerializeField]
	private Vector3[] localWaypoints = null;
	[SerializeField]
	private float m_speed = 0.0f;
	[SerializeField]
	private bool m_cyclic = false;
	[SerializeField]
	private float m_waitTIme = 0.0f;
	[Range(0, 2)]
	[SerializeField]
	public float easeAmount = 0.0f;

	private CollisionInfo m_collisions = new CollisionInfo();
	private int fromWaypointIndex = 0;
	private float percentBetweenWaypoints = 0.0f;
	private float nextMoveTime = 0.0f;
	private bool m_isMoving = false;
	private Vector3[] globalWaypoints = null;
	private CharacterController m_player = null;
	private bool m_characterStandingOnPlatform = false;
	private Vector3 m_startingPos = Vector3.zero;

	#endregion Private
}