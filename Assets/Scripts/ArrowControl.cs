using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowControl : MonoBehaviour
{
    int pos = 1;
    public GameObject arrow;

    public BattleSystem battleSystem; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (pos == 1)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - 3.3f;
                arrow.transform.position = position;
                pos = 4;
            }
            else
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y + 1.1f;
                arrow.transform.position = position;
                pos--;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (pos == 4)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y + 3.3f;
                arrow.transform.position = position;
                pos = 1;
            }
            else
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - 1.1f;
                arrow.transform.position = position;
                pos++;
            }

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (pos)
            {
                case 1: //HIDE
                    battleSystem.OnHideButton();
                    break;
                case 2: //STOP
                    battleSystem.OnStopButton();
                    break;
                case 3: //PUSH
                    battleSystem.OnPushButton();                  
                    break;
                case 4: //ITEM
                    battleSystem.OnItemButton();                 
                    break;
            }
        }
        //TODO: Implement instant text display.
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            lettertime = 0f;
        }*/
    }
}
