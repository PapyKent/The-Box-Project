using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectionBlock : ActionBlock
{
	public override void OnBlockTrigger(Collider2D collision, Projectile projectile)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(projectile.transform.position, projectile.transform.right);
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider.gameObject == gameObject)
			{
				projectile.transform.position = hit.point;
				break;
			}
		}
		projectile.transform.right = m_direction;
	}

	#region Private

	[SerializeField]
	private Vector2 m_direction = Vector2.right;

	#endregion Private
}