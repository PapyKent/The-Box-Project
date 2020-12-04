using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class BreakableBlock : ActionBlock
{
	public override void OnBlockTrigger(Collider2D collision, Projectile projectile)
	{
		ResourceManager.Instance.ReleaseInstance(projectile);
		ResourceManager.Instance.ReleaseInstance(this);
	}
}