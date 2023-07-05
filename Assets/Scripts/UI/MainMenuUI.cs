using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    private void Awake()
    {
        //ocemo da slusamo onclick event za play dugme
        //koristicemo lambda ekspresn to je isto kao da napravimo novu fju i ubacimo njeno ime u addListener(PlayClick)
        // ( ..nema paremetara ..) => { telo.. }
        playButton.onClick.AddListener( () =>
        {
            Loader.Load(Loader.Scene.ScenaBurger);

        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        //ovo je kad smo u igri i pauziramo i odemo na main menu pa ako opet kliknemo play timescale je ostao 0 sve je smrznuto
        Time.timeScale = 1f;
    }


    //private void PlayClick()
   // {

   // }
}

