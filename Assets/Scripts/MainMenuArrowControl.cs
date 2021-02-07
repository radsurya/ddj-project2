using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuArrowControl : MonoBehaviour
{
    int pos = 1;
    static float moveOne = 0.28f;
    static float changeSide = moveOne;

    public GameObject arrow;
    //public BattleSystem battleSystem;

    void Start()
    {
        arrow.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        /**if (!battleSystem.isInputAllowed() || battleSystem.isSelectAllowed())
        {
            //Ignore input during text display or mouse selection.
            return;
        }*/
        arrow.GetComponent<SpriteRenderer>().enabled = true;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SoundManagerScript.playArrowMoveSound();
            if (pos == 1)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - changeSide;
                arrow.transform.position = position;
                pos = 2;
            }
            else
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y + moveOne;
                arrow.transform.position = position;
                pos = 1;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SoundManagerScript.playArrowMoveSound();
            if (pos == 2)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y + changeSide;
                arrow.transform.position = position;
                pos = 1;
            }
            else
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - moveOne;
                arrow.transform.position = position;
                pos = 2;
            }

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Delay());
        }
    }


    public void startGame()
    {
        GameManager.ChangeScene(GameManager.cutScene1);
    }

    public void quitGame()
    {
        #if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }

}



    IEnumerator Delay()
    {
        SoundManagerScript.playArrowSelectSound();
        arrow.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        Debug.Log("wait is over");
      
        switch (pos)
        {
            case 1: //PLAY
                startGame();
                break;
            case 2: //QUIT
                quitGame();
                break;

        }
    }
}

