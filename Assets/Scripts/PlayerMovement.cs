using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_JumpTakeOffSpeed = 7f;
    public float m_MaxSpeed = 7f;

    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;

    private Vector2 m_Move = Vector2.zero;

    private CustomPhysics m_CustomPhysics;
    
    private bool m_JumpPressed;
    private bool m_JumpCancelled;

    private int m_GroundedHashPara = Animator.StringToHash("Grounded");
    private int m_VerticalHashPara = Animator.StringToHash("VelocityY");
    private int m_HorizontalHashPara = Animator.StringToHash("VelocityX");

    private void Awake()
    {
        m_CustomPhysics = GetComponent<CustomPhysics>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
    }
    

    private void Update()
    {
        m_Move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && m_CustomPhysics.Grounded && !m_JumpPressed)
        {
            m_JumpPressed = true;
        }
        else if (Input.GetButtonUp("Jump") && !m_JumpCancelled)
        {
            m_JumpCancelled = true;
        }


        bool flipSprite = m_SpriteRenderer.flipX ? m_Move.x > 0.1f : m_Move.x < -0.1f;
        if (flipSprite)
        {
            m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
        }


        m_Animator.SetBool(m_GroundedHashPara, m_CustomPhysics.Grounded);
        m_Animator.SetFloat(m_HorizontalHashPara, Mathf.Abs(m_CustomPhysics.Velocity.x / m_MaxSpeed));
        m_Animator.SetFloat(m_VerticalHashPara, m_CustomPhysics.Velocity.y);
    }

    private void FixedUpdate()
    {
        if (m_JumpPressed)
        {
            m_CustomPhysics.SetVerticalVelocity(m_JumpTakeOffSpeed);
            m_JumpPressed = false;
        }
        else if (m_JumpCancelled)
        {
            if (m_CustomPhysics.Velocity.y > 0)
                m_CustomPhysics.SetVerticalVelocity(m_CustomPhysics.Velocity.y * 0.5f);

            m_JumpCancelled = false;
        }

        m_CustomPhysics.SetHorizontalVelocity(m_Move.x * m_MaxSpeed);
    }
}
