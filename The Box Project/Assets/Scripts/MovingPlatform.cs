using UnityEngine;

public class MovingPlatform : Platform
{
	public void ResetMovingPlatform()
	{
		m_isMoving = false;
		percentBetweenWaypoints = 0.0f;
		transform.position = globalWaypoints[0];
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

	protected override void Start()
	{
		base.Start();
		m_player = CharacterController.Instance;
		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < globalWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
		if (m_alwaysMove)
		{
			m_isMoving = true;
		}
	}

	private void FixedUpdate()
	{
		m_collisions.Reset();
		CheckAboveCollisions(ref m_collisions);
		CheckIfPlayerIsOnPlatform();
		if (m_isMoving)
		{
			Vector3 velocity = CalculatePlatformMovement();
			if (m_active)
			{
				CheckLeftCollisions(ref m_collisions);
				CheckRightCollisions(ref m_collisions);
				CalculatePassengerMovement(velocity);
			}
			transform.Translate(velocity);
		}
	}

	private void CheckIfPlayerIsOnPlatform()
	{
		if (m_collisions.above && m_collisions.aboveHit.distance < Mathf.Epsilon)
		{
			SetCurrentPlayer(true);
		}
		else
		{
			SetCurrentPlayer(false);
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
		if (velocity.x > 0.0f || velocity.x < 0.0f)
		{
			if (m_collisions.right && Mathf.Abs(m_collisions.rightHit.distance) < Mathf.Epsilon
				|| m_collisions.left && Mathf.Abs(m_collisions.leftHit.distance) < Mathf.Epsilon)
			{
				m_player.SetExternalForce(velocity);
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
			Gizmos.color = Color.red;
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
	private Vector3[] localWaypoints;
	[SerializeField]
	private float m_speed;
	[SerializeField]
	private bool m_cyclic;
	[SerializeField]
	private float m_waitTIme;
	[Range(0, 2)]
	[SerializeField]
	public float easeAmount;

	private CollisionInfo m_collisions;
	private int fromWaypointIndex;
	private float percentBetweenWaypoints;
	private float nextMoveTime;
	private bool m_isMoving = false;
	private Vector3[] globalWaypoints;
	private CharacterController m_player = null;
	private bool m_characterStandingOnPlatform = false;

	#endregion Private
}