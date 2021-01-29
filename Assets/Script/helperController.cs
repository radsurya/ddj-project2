using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helperController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I am alive!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 1);
        }


        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(1, 0);
        }

    }
}
