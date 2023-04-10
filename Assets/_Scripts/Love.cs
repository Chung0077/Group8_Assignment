using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Love : MonoBehaviour
{
    AISystem.ItemSystem.Item items;
    AISystem.Civil.NeedSystem.NeedController need;
    public int interest; 
    public Bubble bubble;
    [SerializeField]Transform head;
    [SerializeField] GameObject bubblePrefab;
    void Awake()
    {
        bubble = bubblePrefab.gameObject.GetComponent<Bubble>();
        bubble.target = head.transform;
        items= this.gameObject.GetComponent<AISystem.ItemSystem.Item>();
        need = this.gameObject.GetComponent<AISystem.Civil.NeedSystem.NeedController>();
    }

    public void FallLove(int love=0)
    {
        if(items.itemName == AISystem.ITEMS.NULL)
        {
        if(love==0)items.itemName = AISystem.ITEMS.FAKELOVE1;
        if(love==1)items.itemName = AISystem.ITEMS.FAKELOVE2;
        if(love==2)items.itemName = AISystem.ITEMS.TRUELOVE2;
        need.ChangeNeedValue("LOVE",-95000);
        }else if(items.itemName == AISystem.ITEMS.FAKELOVE1)
        {
        items.itemName = AISystem.ITEMS.NULL;
        need.ChangeNeedValue("LOVE",95000);
        }
    }

}
