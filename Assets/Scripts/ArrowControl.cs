using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowControl : MonoBehaviour
{
    int pos = 1;
    public GameObject arrow;
    public TextMeshProUGUI m_Text;
    string message;
    float lettertime;

    void Start()
    {

    }

    public IEnumerator TypeText()
    {
        message = m_Text.text;
        m_Text.text = "";
        lettertime = 0.05f;
        foreach (char letter in message.ToCharArray())
        {
            m_Text.text += letter;
            yield return new WaitForSeconds(lettertime);
        }
    }

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
                case 1:
                    StopCoroutine("TypeText");
                    m_Text.text = "Hide";
                    StartCoroutine("TypeText");
                    break;
                case 2:
                    StopCoroutine("TypeText");
                    m_Text.text = "Stop";
                    StartCoroutine("TypeText");
                    break;
                case 3:
                    StopCoroutine("TypeText");
                    m_Text.text = "Push";
                    StartCoroutine("TypeText");
                    break;
                case 4:
                    StopCoroutine("TypeText");
                    m_Text.text = "Item";
                    StartCoroutine("TypeText");
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lettertime = 0f;
        }
    }
}
