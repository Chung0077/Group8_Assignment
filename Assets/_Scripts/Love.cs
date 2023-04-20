using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Love : MonoBehaviour
{
    AISystem.ItemSystem.Item items;
    AISystem.Civil.NeedSystem.NeedController need;
    [SerializeField] Transform head;
    public int interest; 
    public Bubble bubble;
    [SerializeField] GameObject bubblePrefab;
    public NavMeshAgent agent;
    [SerializeField] Transform loveTarget = null;
    public bool love
    {
        get{return _love;}
        set{_love = value; FallLove(_love);}
    }
    private bool _love = false;
    Animator ani;
    void Awake()
    {
        bubble = Instantiate(bubblePrefab, this.transform).gameObject.GetComponent<Bubble>();
        bubble.target = head;
        ani = this.gameObject.GetComponent<Animator>();
        items= this.gameObject.GetComponent<AISystem.ItemSystem.Item>();
        need = this.gameObject.GetComponent<AISystem.Civil.NeedSystem.NeedController>();
    }

private void Start() {
            agent = this.gameObject.GetComponent<NavMeshAgent>();
}
    /*public void FallLove(int love=0)
    {
        if(items.itemName == AISystem.ITEMS.NULL)
        {
        if(love==0)items.itemName = AISystem.ITEMS.FAKELOVE1;
        if(love==1)items.itemName = AISystem.ITEMS.FAKELOVE2;
        if(love==2)items.itemName = AISystem.ITEMS.TRUELOVE1;
        if(love==3)items.itemName = AISystem.ITEMS.TRUELOVE2;
        need.ChangeNeedValue("LOVE",-95000);
        }else if(items.itemName == AISystem.ITEMS.FAKELOVE1 && (love==0||love ==1))
        {
        items.itemName = AISystem.ITEMS.NULL;
        need.ChangeNeedValue("LOVE",95000);
        }
    }*/

    public void FindLove(Transform target)
    {
        loveTarget = target;
        agent.destination = loveTarget.position;
        ani.SetBool("Moving",true);
        ani.SetBool("Dynamic",false);
    }
    void FallLove(bool love)
    {
        if(love==true)
        {
            EnableAI(false);
            if(LoveBow.shootedLove==null){
                LoveBow.shootedLove=this;
                ani.SetTrigger("Love");
                agent.isStopped=true; 
            }
            else if(LoveBow.shootedLove==this)
            {
                LoveBow.shootedLove=null;
                EnableAI(true);
                ani.SetTrigger("UnLove");
                agent.isStopped=false; 
            }
            else if(LoveBow.shootedLove!=this)
            {
                ani.SetTrigger("Love");
                loveTarget=LoveBow.shootedLove.transform;
                LoveBow.shootedLove.loveTarget = this.transform;
                LoveBow.shootedLove=null;
                
            }
        }
    }

    public void EnableAI(bool enable)
    {
        this.GetComponent<AISystem.AIController>().enabled=enable;
        this.GetComponent<AISystem.AIDataBoard>().enabled=enable;

    }
/*
    private void OnCollisionEnter(Collision other) {
                Debug.Log("Collision");
                
    if(loveTarget!=null && other.transform == loveTarget.transform)
    {
        Debug.Log("CollisionLove");
        agent.velocity=Vector3.zero;
        this.transform.LookAt(loveTarget);
        //ani.SetBool("Moving",false);
        //ani.SetBool("Dynamic",true);
        //ani.SetTrigger("Kiss");
        bool truthLove = (loveTarget.GetComponent<Love>().interest == interest)?true:false;
        ani.SetBool("Love",truthLove);
        if(truthLove==true)
        {
            //score +1;
        }
        else
        {
            EnableAI(true);
            LoveBow.shootedLove=null;
            
        }
    }
    }*/
    private void Update() {
        ani.SetFloat("Speed",agent.velocity.magnitude);
        if(loveTarget){
            if(Vector3.Distance(this.transform.position,loveTarget.transform.position)>=1)
            agent.SetDestination(loveTarget.transform.position);
            agent.isStopped=false; 
        }
    }
}
