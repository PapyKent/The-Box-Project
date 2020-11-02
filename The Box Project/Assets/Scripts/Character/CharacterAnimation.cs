using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	#region Private

	private void Start()
	{
		m_controller.GroundedRelay.RegisterListener(OnCharacterGrounded, true);
		m_controller.JumpRelay.RegisterListener(OnCharacterJump, true);
	}

	private void OnDestroy()
	{
		m_controller.GroundedRelay.RegisterListener(OnCharacterGrounded, false);
		m_controller.JumpRelay.RegisterListener(OnCharacterJump, false);
	}

	private void Update()
	{
		m_animator.SetFloat("Speed", Mathf.Abs(m_controller.XSpeed));
		m_animator.SetFloat("YVelocity", m_controller.YVelocity);
		HandleSpriteDirection();
	}

	private void HandleSpriteDirection()
	{
		bool flipX = !m_controller.FaceRight;
		m_renderer.flipX = flipX;
	}

	private void OnCharacterGrounded(bool isGrounded)
	{
		m_animator.SetBool("IsGrounded", isGrounded);
	}

	private void OnCharacterJump(bool isJumping, bool isBouncing)
	{
		m_animator.SetBool("IsJumping", isJumping);
	}

	[SerializeField]
	private CharacterController m_controller = null;
	[SerializeField]
	private Animator m_animator = null;
	[SerializeField]
	private SpriteRenderer m_renderer = null;

	#endregion Private
}