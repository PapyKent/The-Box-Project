using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : RaycastCollisionDetector
{
	[Serializable]
	public enum Color
	{
		NONE,
		BLUE,
		RED,
		YELLOW
	}

	public Color CurrentColor { get { return m_currentColor; } }

	#region Private

	protected override void Awake()
	{
		base.Awake();
		RegisterInputListeners(true);

		m_fxConfig = PlatformFXManager.Instance?.GetPlatformFXConfig(m_currentColor);
		//HACK TO DISABLE PLATFORMS ON START
		EnablePlatform(false, true);
	}

	protected void OnDestroy()
	{
		RegisterInputListeners(false);
	}

	private void RegisterInputListeners(bool register)
	{
		if (GameManager.Instance.SwitchMode == GameManager.ESwitchMode.TRICOLOR)
		{
			LocalInputManager.Instance.RegisterKeyListener(LocalInputManager.EKey.SWITCH_BLUE, OnColorSwitchInput, register);
			LocalInputManager.Instance.RegisterKeyListener(LocalInputManager.EKey.SWITCH_RED, OnColorSwitchInput, register);
			LocalInputManager.Instance.RegisterKeyListener(LocalInputManager.EKey.SWITCH_YELLOW, OnColorSwitchInput, register);
		}
		else
		{
			LocalInputManager.Instance.OnSwitchRelay.RegisterListener(OnSwitchInput, register);
		}
	}

	private void OnColorSwitchInput(LocalInputManager.EKey inputKey, LocalInputManager.EKeyInputEvent inputEvent)
	{
		if (inputEvent != Yube.InputManager<LocalInputManager.EAxis, LocalInputManager.EKey>.EKeyInputEvent.DOWN)
		{
			return;
		}
		switch (inputKey)
		{
			case LocalInputManager.EKey.SWITCH_BLUE:
				OnColorButtonPressed(Platform.Color.BLUE);
				break;

			case LocalInputManager.EKey.SWITCH_RED:
				OnColorButtonPressed(Platform.Color.RED);
				break;

			case LocalInputManager.EKey.SWITCH_YELLOW:
				OnColorButtonPressed(Platform.Color.YELLOW);
				break;
		}
	}

	private void OnSwitchInput(Platform.Color currentColor)
	{
		OnColorButtonPressed(currentColor);
	}

	protected virtual void OnColorButtonPressed(Color newColor)
	{
		if (m_currentColor != Color.NONE)
		{
			bool sameColor = newColor == m_currentColor;
			EnablePlatform(sameColor);
		}
	}

	protected void EnablePlatform(bool enable, bool force = false)
	{
		if (m_currentColor == Color.NONE)
		{
			m_active = true;
			return;
		}
		m_collider.enabled = enable;
		if (enable && (!m_active || force))
		{
			//play sfx
			AudioManager.Instance.PlaySFX(AudioManager.SFXType.PLATFORMCHANGE);
			StopAllCoroutines();
			FXAppear();
		}
		else if (!enable && (m_active || force))
		{
			//play sfx
			AudioManager.Instance.PlaySFX(AudioManager.SFXType.PLATFORMCHANGE);
			StopAllCoroutines();
			StartCoroutine(FXDisappear());
		}
		m_active = enable;
	}

	private IEnumerator FXDisappear()
	{
		float elapsedTime = 0.0f;
		float oldElapsedTime = 0.0f;
		PlayFxInList(m_offFx);
		m_renderer.ForEach(renderer => renderer.material.SetFloat("_UVScale", m_fxConfig.DisappearUVScale));
		while (elapsedTime < 1.0f || oldElapsedTime < 1.0f)
		{
			float dissolveAmount = Mathf.Lerp(m_fxConfig.BeginDisappearDissolveAmount, m_fxConfig.EndDisappearDissolveAmount, elapsedTime);
			m_renderer.ForEach(renderer => renderer.material.SetFloat("_Dissolveamount", dissolveAmount));
			yield return null;
			oldElapsedTime = elapsedTime;
			elapsedTime += Time.deltaTime * m_fxConfig.PlatformDissolveSpeed;
		}
	}

	private void FXAppear()
	{
		PlayFxInList(m_onFx);
		m_renderer.ForEach(renderer =>
		{
			renderer.material.SetFloat("_UVScale", m_fxConfig.AppearUVScale);
			renderer.material.SetFloat("_Dissolveamount", m_fxConfig.AppearDissolveAmount);
		});
	}

	private void PlayFxInList(List<ParticleSystem> fxs)
	{
		if (fxs.Count > 0)
		{
			foreach (ParticleSystem fx in fxs)
			{
				fx.Play();
			}
		}
	}

	[Header("Platform Settings")]
	[SerializeField]
	protected Color m_currentColor = Color.NONE;

	[Header("Graphics Settings")]
	[SerializeField]
	protected List<Renderer> m_renderer = null;

	[Header("FX Settings")]
	[SerializeField]
	protected List<ParticleSystem> m_onFx = null;
	[SerializeField]
	protected List<ParticleSystem> m_offFx = null;

	protected PlatformFXConfig m_fxConfig = null;
	protected bool m_active = false;

	#endregion Private
}