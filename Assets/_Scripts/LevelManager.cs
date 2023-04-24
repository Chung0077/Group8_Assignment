using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    static LevelManager instance;

    public static LevelManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new LevelManager();
            }
            return instance;
        }
    }

    private void Awake() {
    instance = this;
}
    [SerializeField] Timer timer;
    [SerializeField] Score score;

    public int maxTime
    {
        get
        {
            return timer.countdownTime;
        }
        set
        {
            timer.countdownTime = value;
        }
    }
    public int currentScore
    {
        get
        {
            return score.score;
        }
        set
        {
            score.score = value;
        }
    }
    public int maxScore
    {
        get
        {
            return score.maxScore;
        }
        set
        {
            score.maxScore = value;
        }
    }

}
