using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Scene
    {
        MainMenuScene,
        ScenaBurger,
        LoadingScene
    }
    //posto je static ne moze biti povezana ni za koji objekat, ne moze da ima instance, unutra mozemo da dodamo static fje i polja

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {

        Loader.targetScene = targetScene; 
        SceneManager.LoadScene(Scene.LoadingScene.ToString());

      
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
