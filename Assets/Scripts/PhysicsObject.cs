using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PhysicsObject : MonoBehaviour
{
    public float m_GravityModifier = 1f; //allows us to set low gravity

    public LayerMask m_Mask; //obstacle mask

    protected BoxCollider2D m_BoxCollider2D; //It is necessary for boxcasting; we need its size.
    protected Rigidbody2D m_Rigidbody2D; //we move the object with rigidbody.position

    protected Vector2 m_Velocity;  //Velocity vector

    protected RaycastHit2D[] m_HitInfos = new RaycastHit2D[16]; //physics.boxcastnonalloc return it. this has the infos of the surface normals
                                                                //and hit distance. we stop the object before object goes through an object
    protected bool m_Grounded; //If the ground normal is equal to 1, it means we are on the ground.

    protected const float m_ShellDistance = 0.01f; //This prevents colliders from clipping into each other.

    // Start is called before the first frame update
    void Awake()
    {
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    

    private void FixedUpdate()
    {
        m_Grounded = false;


        //law of physics: Last Velocity = First Velocity + Acceleration * Change of time
        //delta means change time of between two frame
        m_Velocity += m_GravityModifier * Physics2D.gravity * Time.fixedDeltaTime;

        //change of position = Velocity * Change of Time.
        Vector2 deltaPosition = m_Velocity * Time.fixedDeltaTime;
        
        //Why are there two Move functions. Because if the character try to move horizontal
        //rigidbody.position try to go inside of the ground. We must calculate it seperately.
        Vector2 moveHorizontal = Vector2.right * deltaPosition.x;  //Horizontal Movement vector
        Move(moveHorizontal);

        
        Vector2 moveVertical = deltaPosition.y * Vector2.up;  //Vertical movement vector
        Move(moveVertical);

        //Move(deltaPosition); if you delete two Move functions and write this Move function you will see what i mean.

    }

    void Move(Vector2 move)
    {
        float distance = move.magnitude;

        Vector2 origin = m_Rigidbody2D.position + m_BoxCollider2D.offset;

        int count = Physics2D.BoxCastNonAlloc(origin, m_BoxCollider2D.size, 0f, move, m_HitInfos, distance + m_ShellDistance, m_Mask);
        

        for (int i = 0; i < count; i++)
        {
            Vector2 currentNormal = m_HitInfos[i].normal;

            if (currentNormal.y == 1)
            {
                m_Grounded = true;
            }

            float project = Vector2.Dot(currentNormal, m_Velocity);
            if (project < 0)
            {
                m_Velocity -= project * currentNormal;
            }

            float modifiedDistance = m_HitInfos[i].distance - m_ShellDistance;
            distance = modifiedDistance < distance ? modifiedDistance : distance;
        }

        m_Rigidbody2D.position += move.normalized * distance;
    }
}
