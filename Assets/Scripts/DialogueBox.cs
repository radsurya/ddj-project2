using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    static float lettertime = 0.05f;
    
    private string message;

    public void UpdateText(string m){
        //Stop coroutine here if two routines run simultaneously.
        StopCoroutine("TypeText");
        message = m;
        StartCoroutine("TypeText");
    }

    private IEnumerator TypeText()
    {
        dialogueText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(lettertime);
        }
    }
}
