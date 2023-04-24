using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField]GameObject rightRay, leftRay,otherUI;
    void OnEnable()
    {
        rightRay.SetActive(true);
        leftRay.SetActive(true);
        otherUI.SetActive(false);
        Time.timeScale=0;
    }
    void OnDisable()
    {
        Time.timeScale=1;
        rightRay.SetActive(false);
        leftRay.SetActive(false);
    }
    
    public void GoMenu()
    {
                Time.timeScale=1;
        SceneManager.LoadScene(0);
    }
}
