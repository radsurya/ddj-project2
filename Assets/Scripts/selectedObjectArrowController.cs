using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedObjectArrowController : MonoBehaviour
{
    public GameObject arrow;
    private bool active;

    void Start(){
        removeArrow();
    }

    public bool isActive(){
        return active;
    }

    public void removeArrow(){
        //Makes arrow so small as to be invisible.
        arrow.SetActive(false);
        active = false;
    }

    public void SetArrow(Vector3 objectPos)
    {
        //Make arrow visible.
        arrow.SetActive(true);
        active = true;
        objectPos.y += (objectPos.y / 2) + 0.15f; 
        arrow.transform.position = objectPos;
    }
}
