using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowControl : MonoBehaviour
{
    int pos = 1;
    static float moveOne = 0.18f;
    static float changeSide = moveOne*3; 

    public GameObject arrow;
    public BattleSystem battleSystem; 

    void Start(){
        arrow.GetComponent<SpriteRenderer>().enabled = false;
    }
    
    void Update()
    {
        if(!battleSystem.isInputAllowed() || battleSystem.isSelectAllowed()){
            //Ignore input during text display or mouse selection.
            return;
        }
        arrow.GetComponent<SpriteRenderer>().enabled = true;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SoundManagerScript.playArrowMoveSound();
            if (pos == 1)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - changeSide;
                arrow.transform.position = position;
                pos = 4;
            }
            else
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y + moveOne;
                arrow.transform.position = position;
                pos--;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SoundManagerScript.playArrowMoveSound();
            if (pos == 4)
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
                pos++;
            }

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManagerScript.playArrowSelectSound();
            arrow.GetComponent<SpriteRenderer>().enabled = false;
            switch (pos)
            {
                case 1: //HIDE
                    battleSystem.OnHideButton();//Debug.Log("HIDE");
                    break;
                case 2: //STOP
                    battleSystem.OnStopButton();//Debug.Log("STOP");
                    break;
                case 3: //PUSH
                    battleSystem.OnPushButton();  //Debug.Log("PUSH");                
                    break;
                case 4: //ITEM
                    battleSystem.OnItemButton();  //Debug.Log("ITEM");               
                    break;
            }
        }
    }
}
