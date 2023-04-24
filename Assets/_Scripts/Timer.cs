using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField]public int countdownTime
    {
        get{
        return _countdownTime;
        }
        set{
            _countdownTime = value;
            timeText.text =  _countdownTime.ToString();
            if(_countdownTime<=0)TheEnd();
        }
    }

    [SerializeField]private int _countdownTime = 120;
    private float timer=1;
    [SerializeField] UnityEvent endEvent;
    void Start()
    {
        timeText.text = countdownTime.ToString();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer=1;
            countdownTime-=1;
        }
    }
    public void TheEnd()
    {
        endEvent.Invoke();
    }
}
