using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickingArrowToSurface : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SphereCollider myCollider;

    [SerializeField]
    private GameObject stickingArrow;

    [SerializeField]
    private Love shootedLove;

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        myCollider.isTrigger = true;

        if(collision.transform.tag!="NPC")
        {
        GameObject arrow = Instantiate(stickingArrow);
        arrow.transform.position = transform.position;
        arrow.transform.forward = transform.forward;
        
        if (collision.collider.attachedRigidbody != null)
        {
            arrow.transform.parent = collision.collider.attachedRigidbody.transform;
        }
        }
        else
        {
            
            Love love = collision.transform.GetComponent<Love>();
            if(shootedLove==null)
            {
            shootedLove = love;
            love.FallLove(0);
            }
            else if(shootedLove!=null)
            {
                

            }

            

        }
        collision.collider.GetComponent<IHittable>()?.GetHit();

        Destroy(gameObject);

    }
}
