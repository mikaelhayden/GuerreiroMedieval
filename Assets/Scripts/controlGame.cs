using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StarterAssets;

public class controlGame : MonoBehaviour
{

    public GameObject pauseObj;
    public GameObject gameOverObj;
    public GameObject infoPlayer;
    public GameObject player;

    private bool isPause;
    private bool pauseButton;

    public Slider slider;

    player player1;

    void Start()
    {
        player1 = FindObjectOfType<player>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        pauseGame();
        slider.value = player1.health;
    }

    public void clickPause()
    {
        pauseButton = true;
    }
    public void pauseGame() //função para salvar o jogo
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            pauseObj.SetActive(isPause);
            infoPlayer.SetActive(!isPause);

        }

        else if(pauseButton == true)
        {
            isPause = !isPause;
            pauseObj.SetActive(isPause);
            infoPlayer.SetActive(!isPause);
            pauseButton = false;
        }

        if (isPause)
        {
            Time.timeScale = 0f;    //o que realmente faz pausar o jogo
            player.GetComponent<ThirdPersonController>().enabled = false;
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
        isPause = !isPause;
        pauseObj.SetActive(isPause);
        infoPlayer.SetActive(!isPause);
    }

    public void gameOver()
    {
        gameOverObj.SetActive(true);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void menu()
    {
        SceneManager.LoadScene(0);
    }

}
