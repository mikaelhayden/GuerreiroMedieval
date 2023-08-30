using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class tesouro : MonoBehaviour
{
    public GameObject fim;
    public controlGame over;

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
            over.mouse();
            over.pauseIsNot = true;
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set a new object
            EventSystem.current.SetSelectedGameObject(over.endButtonFirst);
        }
    }

    public void menuPauseOne()
    {
        isPause = !isPause;
        fim.SetActive(isPause);
        SceneManager.LoadScene(0);
    }
}
