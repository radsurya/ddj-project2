using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickTest : MonoBehaviour
{
    public TextMeshProUGUI m_Text;

    private void OnMouseDown() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            string firstText = this.gameObject.ToString();
            string[] objs = firstText.Split(' ');
            string obj = objs[0];
            m_Text.text = obj;
        }
    }
}

