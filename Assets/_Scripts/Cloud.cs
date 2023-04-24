using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
    public float radius = 10f; // 圓形半徑
    public float speed = 1f; // 移動速度

    public Vector3 center; // 圓心位置
    Vector3 target;
    [SerializeField]Transform canvas;
    float nextTime;
    void Start()
    {
        center = canvas.transform.position;
    }
    void Update()
    {
       if(nextTime<=0)
       {
        nextTime=RandomTime();
        target = UpdateTarget();
       }
       else
       {
        nextTime-=Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime); // 移動到目標位置
        if(Vector3.Distance(this.transform.position,center)==radius)
        {
            nextTime=0;
            UpdateTarget();
        }
       }
    }


    float RandomTime()
    {
        return Random.Range(0f,3f);
    }
    Vector3 UpdateTarget()
    {
         Vector2 randomDirection = Random.insideUnitCircle.normalized; // 隨機方向
        Vector3 targetPosition = center + new Vector3(randomDirection.x,randomDirection.y,0) * radius; // 目標位置
        return targetPosition;
    }
    
}

