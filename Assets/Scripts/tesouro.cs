using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class tesouro : MonoBehaviour
{
    public GameObject fim;
    public bool isPause;

    void Update()
    {
        if (isPause)
        {
            Time.timeScale = 0f;    
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "casa")
        {
            isPause = true;
            fim.SetActive(true);
        }
    }

    public void menuPauseOne()
    {
        isPause = !isPause;
        fim.SetActive(isPause);
        SceneManager.LoadScene(0);
    }
}
