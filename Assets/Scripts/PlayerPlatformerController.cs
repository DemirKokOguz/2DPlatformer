using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : CustomPhysics
{
    public float m_JumpTakeOffSpeed = 7f;
    public float m_MaxSpeed = 7f;

    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");
        m_Velocity.x = move.x * m_MaxSpeed;

        if (Input.GetButtonDown("Jump") && m_Grounded)
        {
            m_Velocity.y = m_JumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (m_Velocity.y > 0)
                m_Velocity.y *= 0.5f;
        }


        bool flipSprite = m_SpriteRenderer.flipX ? move.x > 0.1f : move.x < -0.1f;
        if (flipSprite)
        {
            m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
        }


        m_Animator.SetBool("Grounded", m_Grounded);
        m_Animator.SetFloat("VelocityX", Mathf.Abs(m_Velocity.x / m_MaxSpeed));
        m_Animator.SetFloat("VelocityY", m_Velocity.y);
    }
}
