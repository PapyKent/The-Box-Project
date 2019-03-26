using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
	public GridManager.Color CurrentColor
	{
		get
		{
			return m_currentColor;
		}
	}

	#region Private

	private void Start()
	{
		InputManager.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, true);
		m_fxConfig = PlatformFXManager.Instance.GetPlatformFXConfig(m_currentColor);
	}

	private void OnDestroy()
	{
		InputManager.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, false);
	}

	private void OnColorButtonPressed(GridManager.Color newColor)
	{
		if (m_currentColor != GridManager.Color.NONE)
		{
			bool sameColor = newColor == m_currentColor;
			m_collider.enabled = sameColor;
			if (sameColor && !m_active)
				FXAppear();
			else if (!sameColor && m_active)
				StartCoroutine(FXDisappear());
			m_active = sameColor;
		}
	}

	private IEnumerator FXDisappear()
	{
		float elapsedTime = 0.0f;
		float oldElapsedTime = 0.0f;
		m_offFx?.Play();
		m_renderer.material.SetFloat("_UVScale", m_fxConfig.DisappearUVScale);
		while (elapsedTime < 1.0f || oldElapsedTime < 1.0f)
		{
			float dissolveAmount = Mathf.Lerp(m_fxConfig.BeginDisappearDissolveAmount, m_fxConfig.EndDisappearDissolveAmount, elapsedTime);
			m_renderer.material.SetFloat("_DissolveAmount", dissolveAmount);
			yield return null;
			oldElapsedTime = elapsedTime;
			elapsedTime += Time.deltaTime * m_fxConfig.PlatformDissolveSpeed;
		}
	}

	private void FXAppear()
	{
		m_onFx?.Play();
		m_renderer.material.SetFloat("_UVScale", m_fxConfig.AppearUVScale);
		m_renderer.material.SetFloat("_DissolveAmount", m_fxConfig.AppearDissolveAmount);
	}

	[SerializeField]
	private GridManager.Color m_currentColor = GridManager.Color.NONE;
	[SerializeField]
	private Collider2D m_collider = null;

	[Header("Graphics Settings")]
	[SerializeField]
	private Renderer m_renderer = null;

	[Header("FX Settings")]
	[SerializeField]
	private ParticleSystem m_onFx = null;
	[SerializeField]
	private ParticleSystem m_offFx = null;

	private PlatformFXConfig m_fxConfig = null;
	private bool m_active = false;

	#endregion Private
}