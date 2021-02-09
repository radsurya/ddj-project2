using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickableObject : MonoBehaviour
{
    public BattleSystem battleSystem;
    public string unitName;

    public string hideString;
    public string stopString;
    public string pushString;
    public string itemString;

    public void OnMouseEnter(){
        if(battleSystem.isInputAllowed() && battleSystem.isSelectAllowed())
            battleSystem.selectArrowController.SetArrow(gameObject.transform.position);
        //Alternatively, use OnMouseOver() and check if arrow is active, but that's a lot of calls.
    }

    public void OnMouseExit(){
        battleSystem.selectArrowController.removeArrow();
    }

    public void OnMouseDown() 
    {
        //If clicked on an object, send it to BattleSystem.
        if (Input.GetMouseButtonDown(0) 
            && battleSystem.isInputAllowed() && battleSystem.isSelectAllowed())
        {
            battleSystem.objectSelected(this);
        }
    }
}