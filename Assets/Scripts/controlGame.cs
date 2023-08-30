using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StarterAssets;
using UnityEngine.EventSystems;

public class controlGame : MonoBehaviour
{

    public GameObject pauseObj;
    public GameObject gameOverObj;
    public GameObject infoPlayer;
    public GameObject player;
    public GameObject resumeButtonFirst;
    public GameObject overButtonFirst;
    public GameObject endButtonFirst;

    private bool pauseButton;

    public bool isPause;
    public bool isMouse;
    public bool pauseIsNot;

    public Slider slider;

    player player1;

    void Start()
    {
        player1 = FindObjectOfType<player>();
        player = GameObject.FindWithTag("Player");
        isMouse = false;
        pauseIsNot = false;
        Cursor.lockState = CursorLockMode.Locked; //Deixa travado ao centro da tela
        Cursor.lockState = CursorLockMode.Confined; //Deixa travado na janela
    }

    void Update()
    {
        pauseGame();
        slider.value = player1.health;
        Cursor.visible = isMouse;
    }

    public void clickPause()
    {
        pauseButton = true;
    }
    public void pauseGame() //função para pausar o jogo
    {
        if (Input.GetButtonDown("Cancel") && pauseIsNot == false)
        {
            mouse();
            isPause = !isPause;
            pauseObj.SetActive(isPause);
            infoPlayer.SetActive(!isPause);
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set a new object
            EventSystem.current.SetSelectedGameObject(resumeButtonFirst);
        }

        else if(pauseButton == true && pauseIsNot == false)
        {
            mouse();
            isPause = !isPause;
            pauseObj.SetActive(isPause);
            infoPlayer.SetActive(!isPause);
            pauseButton = false;
        }

        if (isPause)
        {
            Time.timeScale = 0f;    //o que realmente faz pausar o jogo
            player.GetComponent<ThirdPersonController>().enabled = false;
            player.GetComponent<ThirdPersonController>()._animator.SetFloat("Speed", 0);
        }
        else
        {
            Time.timeScale = 1f;
            player.GetComponent<ThirdPersonController>().enabled = true;
        }
    }

    public void menuPause()
    {
        isPause = !isPause;
        pauseObj.SetActive(isPause);
        SceneManager.LoadScene(0);
    }

    public void resume()
    {
        mouse();
        isPause = !isPause;
        pauseObj.SetActive(isPause);
        infoPlayer.SetActive(!isPause);
        pauseIsNot = false;
    }

    public void gameOver()
    {
        mouse();
        gameOverObj.SetActive(true);
        pauseIsNot = true;
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new object
        EventSystem.current.SetSelectedGameObject(overButtonFirst);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void menu()
    {
        SceneManager.LoadScene(0);
    }

    public void mouse()
    {
        isMouse = !isMouse;
    }

}
