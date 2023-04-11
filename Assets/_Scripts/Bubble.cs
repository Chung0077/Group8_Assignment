using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
   public Transform target;
   public Sprite icon;
   [SerializeField] Image image;
   private void Start() {
            transform.position = target.position;
            image.sprite = icon;
   }
    void Update()
    {
        if(icon && !image.sprite)image.sprite = icon;
        transform.position = target.position;
        transform.LookAt(Camera.main.transform.position);
    }
}
