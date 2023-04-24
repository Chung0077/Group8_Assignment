using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public int score
    {
        get{return _score;}
        set{
            _score=value;
            scoreText.text = _score.ToString()+ "/"+ maxScore.ToString();
            if(_score>=maxScore)Win();
            }
    }
    [SerializeField]private int _score = 0;
    public int maxScore;
    [SerializeField] UnityEvent winEvent;

    private void Start() {
    scoreText.text = _score.ToString()+ "/"+ maxScore.ToString();
    }
    public void Win()
    {
        winEvent.Invoke();
    }
}
