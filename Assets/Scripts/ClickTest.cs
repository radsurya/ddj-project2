using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickTest : MonoBehaviour
{
    public ArrowControl arrow;
    public TextMeshProUGUI m_Text;
    public string message = "";
    float lettertime;
    private Coroutine textboxCoroutine = null;

    void Start()
    {
        textboxCoroutine = StartCoroutine(arrow.TypeText());
    }

    public void OnMouseDown() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine(textboxCoroutine);
            string firstText = this.gameObject.ToString();
            string[] objs = firstText.Split(' ');
            message = objs[0];
            m_Text.text = message;
            textboxCoroutine = StartCoroutine(arrow.TypeText());
        }
    }
}