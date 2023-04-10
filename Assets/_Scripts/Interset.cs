using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interset : MonoBehaviour
{
    public List<Sprite> icon;
    int childCount;
        void Start()
    {
        childCount = transform.childCount;
        Debug.Log("Child count: " + childCount);
        for (int i = 0; (2*i) < childCount; i++)
        {
            for (int j = 0; i < 2; i++)
            {
                Debug.Log("2*i+j="+ (int)(2*i+j));
                Love love =transform.GetChild(2*i+j).GetComponent<Love>();
                love.interest = i;
                love.bubble.icon = icon[i];
            }

        }

    }
}
