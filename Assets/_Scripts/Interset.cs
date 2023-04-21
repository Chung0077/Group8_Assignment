using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AISystem.Common.Objects;
public class Interset : MonoBehaviour
{
    public List<Sprite> icon;
    public INavMeshAgent iNav;
    int childCount;
        void Start()
    {
        childCount = transform.childCount;
        Debug.Log("Child count: " + childCount);
        for (int i = 0; (2*i) < childCount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                //Debug.Log("2*i+j="+ (int)(2*i+j));
                Love love =transform.GetChild(2*i+j).GetComponent<Love>();
                love.interest = i;
                love.bubble.icon = icon[i];
            }

        }

    }

}
