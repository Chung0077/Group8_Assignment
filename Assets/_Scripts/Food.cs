using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    void Update()
    {
        if(this.transform.parent==null)EatFood();
    }
public void EatFood() {
    Destroy(this.gameObject);
}
}
