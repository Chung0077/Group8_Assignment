using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StickingArrowToSurface : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SphereCollider myCollider;

    [SerializeField]
    private GameObject stickingArrow;

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
            love.EnableAI(false);
            love.agent.isStopped=(true); 

            if(ArrowController.shootedLove==null)
            {
            ArrowController.shootedLove = love;
            //love.FallLove(0);
            }
            else if(ArrowController.shootedLove!=null)
            {

                if(ArrowController.shootedLove.gameObject == love.gameObject)
                {
                    ArrowController.shootedLove=null;
                                love.EnableAI(true);
                                love.agent.isStopped=(false); 

                    }
                else
                {
                    ArrowController.shootedLove.agent.SetDestination(love.transform.position);
                    love.agent.SetDestination(ArrowController.shootedLove.transform.position);



                    //ArrowController.shootedLove.FindLove(love.transform);
                    //love.FindLove(ArrowController.shootedLove.transform);
                    love.agent.isStopped=(false); 
                    ArrowController.shootedLove.agent.isStopped=(false); 
                    
                }
            }
            

        }
        collision.collider.GetComponent<IHittable>()?.GetHit();

        Destroy(gameObject);

    }



    void Kiss(Transform love1, Transform love2)
    {
        NavMeshAgent nav1 =  love1.gameObject.GetComponent<NavMeshAgent>();
        NavMeshAgent nav2 =  love2.gameObject.GetComponent<NavMeshAgent>();
        nav1.destination = love2.position;
        nav2.destination = love1.position;
    }
}
