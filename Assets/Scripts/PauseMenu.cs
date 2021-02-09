using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenu;
    public GameObject mainArrow;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            SoundManagerScript.playArrowSelectSound();
            if (GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            SoundManagerScript.playArrowSelectSound();
            if (GameIsPaused){
                switch (PauseMenuArrowControl.pos){
                    case 1: //RESUME
                        Resume();
                        break;
                    case 2: //RESTART
                        Restart();
                        break;
                    case 3: //QUIT
                        Quit();
                        break;
                    
                }
                PauseMenuArrowControl.pos = 1;
            }
        }
    }

    void Pause(){
        pauseMenu.SetActive(true);
        if(mainArrow != null)
        {
            mainArrow.SetActive(false);
        }
        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void Resume(){
        pauseMenu.SetActive(false);
        if (mainArrow != null)
        {
            mainArrow.SetActive(true);
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Quit(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.ChangeScene(GameManager.mainMenuScene);
    }
    void Restart(){
        pauseMenu.SetActive(false);
        DialogueBox.CleanMessages();
        Time.timeScale = 1f;
        GameManager.ChangeScene(GameManager.cutScene1);
    }
}
