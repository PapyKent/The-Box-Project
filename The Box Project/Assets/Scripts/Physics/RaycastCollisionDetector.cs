using UnityEngine;

public class RaycastCollisionDetector : MonoBehaviour
{
	public struct CollisionInfo
	{
		public DirectionalCollisionInfo aboveCollision;
		public DirectionalCollisionInfo belowCollision;
		public DirectionalCollisionInfo leftCollision;
		public DirectionalCollisionInfo rightCollision;

		public bool faceRight;

		public void Reset()
		{
			aboveCollision.Reset();
			belowCollision.Reset();
			leftCollision.Reset();
			rightCollision.Reset();
		}
	}

	public struct DirectionalCollisionInfo
	{
		public bool isColliding;
		public RaycastHit2D hit;

		public void Reset()
		{
			isColliding = false;
		}
	}

	public Bounds ColliderBounds { get { return m_collider.bounds; } }

	public void CheckBelowCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.belowCollision, m_collider.bounds.min, Vector3.down, true);
	}

	public void CheckAboveCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.aboveCollision, new Vector2(m_collider.bounds.min.x, m_collider.bounds.max.y), Vector3.up, true);
	}

	public void CheckRightCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.rightCollision, new Vector2(m_collider.bounds.max.x, m_collider.bounds.min.y), Vector3.right, false);
	}

	public void CheckLeftCollisions(ref CollisionInfo collisions)
	{
		CheckCollisions(ref collisions.leftCollision, m_collider.bounds.min, Vector3.left, false);
	}

	#region Private

	protected virtual void Awake()
	{
		if (m_collider != null)
		{
			m_horizontalRaycastCount = Mathf.RoundToInt(m_collider.bounds.size.y / m_distanceBetweenRay)
				+ (int)(m_collider.bounds.size.y % m_distanceBetweenRay);
			m_verticalRaycastCount = Mathf.RoundToInt(m_collider.bounds.size.x / m_distanceBetweenRay)
				+ (int)(m_collider.bounds.size.x % m_distanceBetweenRay);
		}
	}

	private void CheckCollisions(ref DirectionalCollisionInfo collisionInfo, Vector2 origin, Vector3 rayDirection, bool vertical)
	{
		int limit = vertical ? m_verticalRaycastCount + 1 : m_horizontalRaycastCount;
		if (vertical)
		{
			origin = new Vector2(origin.x + ((m_collider.bounds.size.x / m_verticalRaycastCount) / 2), origin.y);
		}
		else
		{
			origin = new Vector2(origin.x, origin.y + ((m_collider.bounds.size.y / m_horizontalRaycastCount) / 2));
		}
		collisionInfo.hit = new RaycastHit2D();
		bool alreadyHit = false;
		for (int i = 0; i < limit; i++)
		{
			if ((vertical && origin.x > m_collider.bounds.max.x)
				|| (!vertical && origin.y > m_collider.bounds.max.y))
			{
				continue;
			}
			Debug.DrawRay(origin, rayDirection * m_shootRayDistance, Color.red);
			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, rayDirection, m_shootRayDistance, m_collisionMask);
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider != null && !hit.collider.isTrigger)
				{
					collisionInfo.isColliding = true;
					if (!alreadyHit || IsNewHitCloser(collisionInfo.hit, hit, rayDirection, vertical))
					{
						alreadyHit = true;
						collisionInfo.hit = hit;
					}
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
	[SerializeField]
	private float m_distanceBetweenRay = 0.2f;
	[SerializeField]
	private float m_shootRayDistance = 1.0f;

	private int m_verticalRaycastCount = 0;
	private int m_horizontalRaycastCount = 0;

	#endregion Private
}