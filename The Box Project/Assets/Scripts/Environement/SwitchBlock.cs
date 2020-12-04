using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlock : ActionBlock
{
	public override void OnBlockTrigger(Collider2D collision, Projectile projectile)
	{
		if (!m_isActivated)
		{
			m_isActivated = true;
			foreach (GameObject go in m_gameObjectsToDeactivate)
			{
				go.SetActive(false);
			}
			m_spriteRenderer.sprite = m_offSprite;
		}
	}

	#region Private

	[SerializeField]
	private List<GameObject> m_gameObjectsToDeactivate = new List<GameObject>();
	[SerializeField]
	private SpriteRenderer m_spriteRenderer = null;
	[SerializeField]
	private Sprite m_onSprite = null;
	[SerializeField]
	private Sprite m_offSprite = null;

	[NonSerialized]
	private bool m_isActivated = false;

	#endregion Private
}