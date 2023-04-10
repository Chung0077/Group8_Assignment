using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
private void OnCollisionEnter(Collision other) {
    if(other.transform.tag=="Terrain")Destroy(this.gameObject);
}
}
