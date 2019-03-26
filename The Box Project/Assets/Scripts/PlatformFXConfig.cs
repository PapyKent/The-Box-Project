using UnityEngine;

[CreateAssetMenu(menuName = "PlatformFXConfig")]
public class PlatformFXConfig : ScriptableObject
{
	public float PlatformDissolveSpeed { get { return m_platformDissolveSpeed; } }
	public float AppearDissolveAmount { get { return m_appearDissolveAmount; } }
	public float AppearUVScale { get { return m_appearUVScale; } }
	public float BeginDisappearDissolveAmount { get { return m_beginDisappearDissolveAmount; } }
	public float EndDisappearDissolveAmount { get { return m_endDisappearDissolveAmount; } }
	public float DisappearUVScale { get { return m_disappearUVScale; } }

	#region Private

	[SerializeField]
	private float m_platformDissolveSpeed = 3.0f;

	[Header("Appear FX")]
	[SerializeField]
	private float m_appearDissolveAmount = 0.0f;
	[SerializeField]
	private float m_appearUVScale = 0.0f;

	[Header("Disappear FX")]
	[SerializeField]
	private float m_beginDisappearDissolveAmount = 0.0f;
	[SerializeField]
	private float m_endDisappearDissolveAmount = 0.0f;
	[SerializeField]
	private float m_disappearUVScale = 0.0f;

	#endregion Private
}