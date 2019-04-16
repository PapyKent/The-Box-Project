using UnityEngine;

public class RaycastCollisionDetector : MonoBehaviour
{
	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;

		public RaycastHit2D aboveHit, belowHit;
		public RaycastHit2D leftHit, rightHit;

		public bool faceRight;

		public void Reset()
		{
			above = below = false;
			left = right = false;
		}
	}

	public Bounds ColliderBounds { get { return m_collider.bounds; } }

	public void CheckBelowCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.below, ref collisions.belowHit, m_collider.bounds.min, Vector3.down, true);
	}

	public void CheckAboveCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.above, ref collisions.aboveHit, new Vector2(m_collider.bounds.min.x, m_collider.bounds.max.y), Vector3.up, true);
	}

	public void CheckRightCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.right, ref collisions.rightHit, new Vector2(m_collider.bounds.max.x, m_collider.bounds.min.y), Vector3.right, false);
	}

	public void CheckLeftCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.left, ref collisions.leftHit, m_collider.bounds.min, Vector3.left, false);
	}

	#region Private

	protected virtual void Start()
	{
		if (m_collider != null)
		{
			m_horizontalRaycastCount = Mathf.RoundToInt(m_collider.bounds.size.y / m_distanceBetweenRay);
			m_verticalRaycastCount = Mathf.RoundToInt(m_collider.bounds.size.x / m_distanceBetweenRay);
		}
		m_shootRayDistance = 1.0f;
	}

	private void CheckCollisions(ref bool collisionDirection, ref RaycastHit2D refHit, Vector2 origin, Vector3 rayDirection, bool vertical)
	{
		int limit = vertical ? m_verticalRaycastCount + 1 : m_horizontalRaycastCount;
		if (vertical)
			origin.x += 0.05f;
		else
			origin.y += 0.05f;
		refHit = new RaycastHit2D();
		bool alreadyHit = false;
		for (int i = 0; i < limit; i++)
		{
			if (i == limit - 1)
			{
				if (vertical)
					origin.x -= 0.1f;
				else
					origin.y -= 0.1f;
			}
			Debug.DrawRay(origin, rayDirection * m_shootRayDistance, Color.red);
			RaycastHit2D hit = Physics2D.Raycast(origin, rayDirection, m_shootRayDistance, m_collisionMask);

			if (hit.collider != null && !hit.collider.isTrigger)
			{
				collisionDirection = true;
				if (!alreadyHit || IsNewHitCloser(refHit, hit, rayDirection, vertical))
				{
					alreadyHit = true;
					refHit = hit;
				}
			}
			if (vertical)
			{
				origin = new Vector2(origin.x + (m_collider.bounds.size.x / m_verticalRaycastCount), origin.y);
			}
			else
			{
				origin = new Vector2(origin.x, origin.y + (m_collider.bounds.size.y / m_horizontalRaycastCount));
			}
		}
	}

	private bool IsNewHitCloser(RaycastHit2D refHit, RaycastHit2D newHit, Vector3 rayDir, bool vertical)
	{
		if (vertical)
		{
			if (rayDir == Vector3.up)
			{
				return newHit.point.y < refHit.point.y;
			}
			else if (rayDir == Vector3.down)
			{
				return newHit.point.y > refHit.point.y;
			}
		}
		else
		{
			if (rayDir == Vector3.left)
			{
				return newHit.point.x > refHit.point.x;
			}
			else if (rayDir == Vector3.right)
			{
				return newHit.point.x < refHit.point.x;
			}
		}
		return false;
	}

	[Header("Physics Settings")]
	[SerializeField]
	protected BoxCollider2D m_collider = null;
	[SerializeField]
	protected LayerMask m_collisionMask;

	private int m_verticalRaycastCount = 0;
	private int m_horizontalRaycastCount = 0;
	private float m_distanceBetweenRay = 0.1f;
	private float m_shootRayDistance = 0.1f;

	#endregion Private
}