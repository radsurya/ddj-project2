using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            if(GameIsPaused){
                switch (MainMenuArrowControl.pos){
                    case 1: //PLAY
                        Resume();
                        break;
                    case 2: //QUIT
                        Quit();
                        break;
                    
                }
            }
        }
    }

    public void Resume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Quit(){
        #if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }
}
