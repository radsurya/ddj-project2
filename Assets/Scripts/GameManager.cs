using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static int mainMenuScene = 0;
    public static int cutScene1 = 1;
    public static int levelScene = 2;

    public static void ChangeScene(int i) 
    {
        SceneManager.LoadScene(i);
    }
}
