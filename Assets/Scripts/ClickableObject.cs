using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickableObject : MonoBehaviour
{
    public BattleSystem battleSystem;

    public void OnMouseDown() 
    {
        //If clicked on an object, send it to BattleSystem.
        if (Input.GetMouseButtonDown(0))
        {
            battleSystem.objectSelected(this.gameObject);
        }
    }
}