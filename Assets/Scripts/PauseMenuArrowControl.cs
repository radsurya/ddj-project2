using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuArrowControl : MonoBehaviour
{
    public static int pos = 1;
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
            if (pos == 1)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - changeSide;
                position.y = position.y - changeSide;
                arrow.transform.position = position;
                pos = 3;
            }
            else if(pos == 2){
                Vector3 position = arrow.transform.position;
                position.y = position.y + changeSide;
                arrow.transform.position = position;
                pos = 1;
            }else
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y + moveOne;
                arrow.transform.position = position;
                pos = 2;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (pos == 3)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y + changeSide;
                position.y = position.y + changeSide;
                arrow.transform.position = position;
                pos = 1;
            }
            else if(pos == 2)
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - changeSide;
                arrow.transform.position = position;
                pos = 3;
            }else
            {
                Vector3 position = arrow.transform.position;
                position.y = position.y - moveOne;
                arrow.transform.position = position;
                pos = 2;
            }

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            arrow.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
