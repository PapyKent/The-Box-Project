using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class MainCamera : Singleton<MainCamera>
{
	#region Private

	private void LateUpdate()
	{
		if (InputManager.Instance.DirectionalInput.y != 0 && InputManager.Instance.DirectionalInput.x == 0.0f)
		{
			m_elapsedTimeOffset += Time.deltaTime;
		}
		else
		{
			m_elapsedTimeOffset = 0.0f;
		}
		CheckOffset();
	}

	private void CheckOffset()
	{
		if (!m_isOffseted && m_elapsedTimeOffset >= m_timeOffsetUpOrDown)
		{
			StopAllCoroutines();
			StartCoroutine(ApplyOffset(InputManager.Instance.DirectionalInput.y > 0, true));
			m_isOffseted = true;
		}
		else if (m_isOffseted && m_elapsedTimeOffset < m_timeOffsetUpOrDown)
		{
			StopAllCoroutines();
			StartCoroutine(ApplyOffset(InputManager.Instance.DirectionalInput.y > 0, false));
			m_isOffseted = false;
		}
	}

	private IEnumerator ApplyOffset(bool up, bool apply)
	{
		CinemachineCameraOffset offset = m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineCameraOffset>();
		float elapsedTime = 0.0f;
		float startOffset = offset.m_Offset.y;
		float endOffset = apply ? (up ? m_maxOffset : -1 * m_maxOffset) : 0.0f;
		while (elapsedTime <= m_offsetTravelTime)
		{
			elapsedTime += Time.deltaTime;
			offset.m_Offset.y = Mathf.Lerp(startOffset, endOffset, elapsedTime / m_offsetTravelTime);
			yield return null;
		}
	}

	[SerializeField]
	private Cinemachine.CinemachineBrain m_cinemachineBrain = null;
	[SerializeField]
	private float m_timeOffsetUpOrDown = 1.0f;
	[SerializeField]
	private float m_maxOffset = 5.0f;
	[SerializeField]
	private float m_offsetTravelTime = 1.0f;

	private bool m_isOffseted = false;
	private float m_elapsedTimeOffset = 0.0f;

	#endregion Private
}