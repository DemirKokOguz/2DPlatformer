using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CustomPhysics : MonoBehaviour
{
    public float m_GravityScaler = 1f; //Allows us to set low gravity

    public LayerMask m_Mask; //Obstacle mask

    protected BoxCollider2D m_BoxCollider2D; //It is necessary for boxcasting; we need its size.
    protected Rigidbody2D m_Rigidbody2D; //We move the object with rigidbody.position

    protected Vector2 m_Velocity;  //Velocity vector

    protected const int m_MaxHitCount = 8;

    protected float m_MinSurfaceAngle = .75f;

    protected RaycastHit2D[] m_HitInfos = new RaycastHit2D[m_MaxHitCount]; //physics.boxcastnonalloc takes it as parameter.
                                                                           
    protected bool m_Grounded; //If the ground normal is equal to 1, it means we are on the ground.

    protected const float m_MinCastDistance = 0.01f; //This prevents colliders from clipping into each other.
    
    void Awake()
    {
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;


        //law of physics: Last Velocity = First Velocity + Acceleration * Change of time
        m_Velocity += m_GravityScaler * Physics2D.gravity * Time.fixedDeltaTime;

        //change of position = Velocity * Change of Time.
        Vector2 deltaPosition = m_Velocity * Time.fixedDeltaTime;
        
        Vector2 moveHorizontal = Vector2.right * deltaPosition.x;  //Horizontal Movement vector
        MoveObject(moveHorizontal);


        Vector2 moveVertical = deltaPosition.y * Vector2.up;  //Vertical movement vector
        MoveObject(moveVertical);

    }

    void MoveObject(Vector2 move)
    {
        float distance = move.magnitude;

        Vector2 origin = m_Rigidbody2D.position + m_BoxCollider2D.offset;

        float angle = 0f;
        int count = Physics2D.BoxCastNonAlloc(origin, m_BoxCollider2D.size, angle, move, m_HitInfos, distance + m_MinCastDistance, m_Mask);


        for (int i = 0; i < count; i++)
        {
            Vector2 currentNormal = m_HitInfos[i].normal;

            if (currentNormal.y > m_MinSurfaceAngle)
            {
                m_Grounded = true;
            }

            float project = Vector2.Dot(currentNormal, m_Velocity);
            if (project < 0)
            {
                m_Velocity -= project * currentNormal;
            }

            float modifiedDistance = m_HitInfos[i].distance - m_MinCastDistance;
            distance = Mathf.Min(modifiedDistance, distance);
        }

        m_Rigidbody2D.position += move.normalized * distance;
    }
}
