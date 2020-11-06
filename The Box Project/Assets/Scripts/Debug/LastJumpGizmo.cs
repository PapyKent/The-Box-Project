using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastJumpGizmo : MonoBehaviour
{
	#region Private

	private void Start()
	{
		m_controller.JumpRelay.AddListener(OnJump);
	}

	private void OnDestroy()
	{
		m_controller.JumpRelay.RemoveListener(OnJump);
	}

	private void OnJump(bool jump, bool bounce)
	{
		m_isJumping = jump;
		if (m_isJumping)
		{
			m_jumpPositions.Clear();
			StartCoroutine(RecordJumpPosition());
		}
	}

	private void OnDrawGizmos()
	{
		if (m_jumpPositions.Count > 0 && !m_isJumping)
		{
			foreach (Vector3 jumpPos in m_jumpPositions)
			{
				Gizmos.DrawSphere(jumpPos, 0.2f);
			}
		}
	}

	private IEnumerator RecordJumpPosition()
	{
		while (m_isJumping)
		{
			m_jumpPositions.Add(m_controller.transform.position);
			yield return new WaitForSeconds(m_recordJumpPosEachTime);
		}
		m_jumpPositions.Add(m_controller.transform.position);
	}

	[SerializeField]
	private CharacterController m_controller = null;
	[SerializeField]
	private float m_recordJumpPosEachTime = 0.1f;

	private bool m_isJumping = false;
	private List<Vector3> m_jumpPositions = new List<Vector3>();

	#endregion Private
}