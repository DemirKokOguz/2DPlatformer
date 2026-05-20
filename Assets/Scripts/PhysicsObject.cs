using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PhysicsObject : MonoBehaviour
{
    public float m_GravityModifier = 1f; //we can make low gravity or not

    public LayerMask m_Mask; //obstacle mask

    protected BoxCollider2D m_BoxCollider2D; //it is necessery for boxcasting, we need size of it
    protected Rigidbody2D m_rigidbody2D; //we move the oject with rigidbody.position
    
    protected Vector2 m_Velocity;  //Velocity vector

    protected RaycastHit2D[] m_HitInfos = new RaycastHit2D[16]; //physics.boxcastnonalloc return it. this has the infos of the surface normals
                                                                //and hit distance. we stop the object before object goes through an object
    protected bool m_Grounded; //if ground normal equel to 1 that means that we are on the ground
    
    protected const float m_ShellDistance = 0.01f; //this do colliders dont get inside each other

    // Start is called before the first frame update
    void Awake()
    {
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }
    

    private void FixedUpdate()
    {
        m_Grounded = false;


        //law of physics: Last Velocity = First Velocity + Acceleration * Change of time
        //delta means change time of between two frame
        m_Velocity += m_GravityModifier * Physics2D.gravity * Time.deltaTime;

        //change of position = Velocity * Change of Time. these two equations are basic highscool level infos of physics
        Vector2 deltaPosition = m_Velocity * Time.deltaTime;
        
        //Now we are at the most painful place. Why There are two Move functions. Because if character try to move horizontal
        //rigidbody.position try to go inside of the ground. We must calculate seperately.
        Vector2 moveVertical = Vector2.right * deltaPosition.x;  //vertical Movement vector
        Move(moveVertical);

        
        Vector2 moveHorizontal = deltaPosition.y * Vector2.up;  //Horizontal movement vector
        Move(moveHorizontal);

        //Move(deltaPosition); if you delete two Move functions and write this Move function you will see what i mean.

    }

    void Move(Vector2 move)
    {
        float distance = move.magnitude;

        Vector2 origin = m_rigidbody2D.position + m_BoxCollider2D.offset;

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

        m_rigidbody2D.position += move.normalized * distance;
    }
}
