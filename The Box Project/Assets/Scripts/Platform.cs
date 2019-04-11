using System.Collections;
using UnityEngine;

public class Platform : RaycastCollisionDetector
{
	public GridManager.Color CurrentColor { get { return m_currentColor; } }

	#region Private

	protected override void Start()
	{
		base.Start();
		InputManager.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, true);
		m_fxConfig = PlatformFXManager.Instance?.GetPlatformFXConfig(m_currentColor);
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
			{
				//play sfx
				AudioManager.Instance.PlaySFX(AudioManager.SFXType.PLATFORMCHANGE);

				FXAppear();
			}
			else if (!sameColor && m_active)
			{
				//play sfx
				AudioManager.Instance.PlaySFX(AudioManager.SFXType.PLATFORMCHANGE);

				StartCoroutine(FXDisappear());
			}
			m_active = sameColor;
		}
	}

	private IEnumerator FXDisappear()
	{
		float elapsedTime = 0.0f;
		float oldElapsedTime = 0.0f;
		if (m_offFx != null)
			m_offFx.Play();
		m_renderer.material.SetFloat("_UVScale", m_fxConfig.DisappearUVScale);
		while (elapsedTime < 1.0f || oldElapsedTime < 1.0f)
		{
			float dissolveAmount = Mathf.Lerp(m_fxConfig.BeginDisappearDissolveAmount, m_fxConfig.EndDisappearDissolveAmount, elapsedTime);
			m_renderer.material.SetFloat("_Dissolveamount", dissolveAmount);
			yield return null;
			oldElapsedTime = elapsedTime;
			elapsedTime += Time.deltaTime * m_fxConfig.PlatformDissolveSpeed;
		}
	}

	private void FXAppear()
	{
		if (m_onFx != null)
			m_onFx?.Play();
		m_renderer.material.SetFloat("_UVScale", m_fxConfig.AppearUVScale);
		m_renderer.material.SetFloat("_Dissolveamount", m_fxConfig.AppearDissolveAmount);
	}

	[Header("Platform Settings")]
	[SerializeField]
	protected GridManager.Color m_currentColor = GridManager.Color.NONE;

	[Header("Graphics Settings")]
	[SerializeField]
	protected Renderer m_renderer = null;

	[Header("FX Settings")]
	[SerializeField]
	protected ParticleSystem m_onFx = null;
	[SerializeField]
	protected ParticleSystem m_offFx = null;

	protected PlatformFXConfig m_fxConfig = null;
	protected bool m_active = false;

	#endregion Private
}