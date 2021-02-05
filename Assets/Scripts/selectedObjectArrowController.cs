using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedObjectArrowController : MonoBehaviour
{
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetArrow(Vector3 objectPos)
    {
        objectPos.y += (objectPos.y / 2) + 0.15f; 
        arrow.transform.position = objectPos;
    }
}
