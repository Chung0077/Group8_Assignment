using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
private void OnCollisionExit(Collision other) {
    if(other.transform.tag=="NPC")Destroy(this.gameObject);
}
}
