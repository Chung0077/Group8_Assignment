using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickArrow : MonoBehaviour
{
    private void Start() {
        Invoke("DestorySelf",5f);
    }

    void DestorySelf()
    {
        Destroy(this.gameObject);
    }
}
