using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickTest : MonoBehaviour
{
    public BattleSystem battleSystem;

    void Start()
    {
        //textboxCoroutine = StartCoroutine(arrow.TypeText());
    }

    public void OnMouseDown() 
    {
        //If clicked on an object, send it to BattleSystem.
        if (Input.GetMouseButtonDown(0))
        {
            battleSystem.objectSelected(this.gameObject);
            /*StopCoroutine(textboxCoroutine);
            string firstText = this.gameObject.ToString();
            string[] objs = firstText.Split(' ');
            message = objs[0];
            m_Text.text = message;
            textboxCoroutine = StartCoroutine(arrow.TypeText());*/
        }
    }
}