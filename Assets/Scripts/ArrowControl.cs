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

    void Start()
    {
        message = m_Text.text;
        m_Text.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            m_Text.text += letter;
            yield return new WaitForSeconds(0.08f);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
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
        if (Input.GetKeyDown(KeyCode.DownArrow))
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
                    m_Text.text = "Hide";
                    break;
                case 2:
                    m_Text.text = "Stop";
                    break;
                case 3:
                    m_Text.text = "Push";
                    break;
                case 4:
                    m_Text.text = "Item";
                    break;
            }
        }
    }
}
