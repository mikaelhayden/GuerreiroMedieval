using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Script do menu do game, o menu conta com o botão Play, settings e Info
//O play inicia a cena do game principal 
//O botão settings abre uma tela para as configurações do game
//o botão info abre uma tela para as informações do desenvolvedor e agradecimentos

public class menu : MonoBehaviour
{
    public GameObject inforObj;
    public GameObject settingsObj;
    public GameObject menuInicial;


    private bool informa;
    private bool informaSettings;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //se o jogador estiver no menu principal e pressione esc,  ele sai do game
            if (informa == false && informaSettings == false)
            {
                Application.Quit();
            }

            //caso o jogador pressione esc e esteja na tela de info ele volta pra tela principal
            else
            {
                inforObj.SetActive(false);      //active de info fica false e variável para saber se está na tela de info fica false
                settingsObj.SetActive(false);
                informa = false;
                informaSettings = false;
                menuInicial.SetActive(true);
            }

        }

    }

    public void startGame()
    {
        informa = false;
        SceneManager.LoadScene(1);
    }
    public void info()
    {
        inforObj.SetActive(true);
        informa = true;
        menuInicial.SetActive(false);
    }

    public void settings()
    {
        settingsObj.SetActive(true);
        informaSettings = true;
        menuInicial.SetActive(false);
    }

    public void voltar()    //Volta todas as telas se estiver em alguma tela, irá voltar
    {
        inforObj.SetActive(false);      
        settingsObj.SetActive(false);
        informa = false;
        informaSettings = false;
        menuInicial.SetActive(true); ;            //Pausa o game para não entrar em duas telas ao mesmo tempo
    }
}
