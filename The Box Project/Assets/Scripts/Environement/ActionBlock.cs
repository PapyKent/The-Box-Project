using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBlock : MonoBehaviour
{
	public abstract void OnBlockTrigger(Collider2D collision, Projectile projectile);
}