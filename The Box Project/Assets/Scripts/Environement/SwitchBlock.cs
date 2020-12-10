using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class SwitchBlock : ActionBlock
{
	public override void OnBlockTrigger(Collider2D collision, Projectile projectile)
	{
		if (!m_isPressed)
		{
			m_isPressed = true;
			foreach (GameObject go in m_gameObjectsToDeactivate)
			{
				go.SetActive(false);
			}
			m_spriteRenderer.sprite = m_offSprite;
		}
		ResourceManager.Instance.ReleaseInstance(projectile);
	}

	#region Private

	private void Start()
	{
#if UNITY_EDITOR
		if (m_gameObjectsToDeactivate.Count == 0)
		{
			Debug.LogError("Switch has no gameobjects to deactivate! Game might not work.", this);
		}
#endif
		CheckpointManager.Instance.ReloadCheckpointRelay.RegisterListener(OnCheckpointReload, true);
	}

	public void OnCheckpointReload()
	{
		foreach (GameObject go in m_gameObjectsToDeactivate)
		{
			go.SetActive(true);
		}
		m_isPressed = false;
		m_spriteRenderer.sprite = m_onSprite;
	}

	[SerializeField]
	private List<GameObject> m_gameObjectsToDeactivate = new List<GameObject>();
	[SerializeField]
	private SpriteRenderer m_spriteRenderer = null;
	[SerializeField]
	private Sprite m_onSprite = null;
	[SerializeField]
	private Sprite m_offSprite = null;

	[NonSerialized]
	private bool m_isPressed = false;

	#endregion Private
}