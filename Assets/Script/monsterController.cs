using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterController : MonoBehaviour
{
    public float delay = 0;
    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // delay = 5;
        InvokeRepeating("ChangePosition", 0, 3); //calls ChangePosition() every 2 secs
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Increase the value of 'timer' by deltaTime:
        timer = timer + Time.deltaTime;

        if (timer > delay)
        {
            Debug.Log("Teste");
             transform.position += new Vector3(0, 1);

        }*/
    }

    void ChangePosition()
    {
        // transform.position += new Vector3(0, 1);
    }

}
