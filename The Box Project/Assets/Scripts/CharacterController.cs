using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Private

    private void Start()
    {
        
    }

    private void Update()
    {
        m_inputs = new Vector2(Input.GetAxis("Horizontal") * m_groundSpeed, 0.0f);
        HandleSpriteDirection();
        m_collisions.Reset();
        m_groundCollider.CheckVerticalCollisions(ref m_collisions);
        HandleJump();
        ApplyGravity();
        m_inputs.y = m_velocity.y * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        transform.Translate(m_inputs);
    }

    private void HandleSpriteDirection()
    {
        if(m_inputs.x != 0.0f)
        {
            bool flipX = m_inputs.x > 0.0f;
            m_sprite.flipX = flipX;
        }
    }

    private void HandleJump()
    {
        m_pressedJump = Input.GetButtonDown("Jump");
        m_releasedJump = Input.GetButtonUp("Jump");

        if(!m_isJumping && m_pressedJump)
        {
            m_isJumping = true;
            m_velocity.y = m_maxJumpSpeed;
        }
        else if(m_isJumping && m_releasedJump)
        {
            if(m_velocity.y < m_minJumpSpeed)
            {
                m_velocity.y = m_minJumpSpeed;
            }
        }
        else if(m_isJumping && m_collisions.below)
        {
            m_isJumping = false;
        }
    }

    private void ApplyGravity()
    {
        if(!m_collisions.below)
        {
            m_velocity.y += m_gravity * Time.fixedDeltaTime;
        }
        else if(!m_isJumping)
        {
            m_velocity.y = 0.0f;
        }
    }

    [SerializeField]
    private float m_groundSpeed = 5.0f;
    [SerializeField]
    private float m_maxJumpSpeed = 20.0f;
    [SerializeField]
    private float m_minJumpSpeed = 10.0f;
    [SerializeField]
    private float m_gravity = -9.81f;
    [SerializeField]
    private SpriteRenderer m_sprite = null;
    [SerializeField]
    private GroundCharacterCollider m_groundCollider = null;

    private Vector2 m_inputs = Vector2.zero;
    private Vector2 m_velocity = Vector2.zero;
    private GroundCharacterCollider.CollisionInfo m_collisions;
    private bool m_isJumping = false;
    private bool m_pressedJump = false;
    private bool m_releasedJump = false;
    #endregion
}
