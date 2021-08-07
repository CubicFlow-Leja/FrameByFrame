using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    public static int ActiveScene;
    public static int SceneToLoad;

    public static void RequestSceneLoad(int SceneIndex)
    {
        ActiveScene = SceneManager.GetActiveScene().buildIndex;
        
        
        _LoadScene(SceneIndex);
        
        SceneManager.sceneLoaded += LoadingFinished;

    }
    public static void _LoadScene(int _Scene)
    {
       
        SceneManager.LoadSceneAsync(_Scene,LoadSceneMode.Single);
    }


    
    public static void LoadingFinished(Scene _Scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;
        SceneManager.sceneLoaded -= LoadingFinished;
    
    }

}

