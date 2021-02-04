using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    static float lettertime = 0.05f;

    bool typing = false;
    
    private Queue<string> messages = new Queue<string>();

    public void UpdateText(List<string> m){
        //Receive lists.
        foreach (string s in m){
            UpdateText(s);
        }
        //TODO: Make sure this iterator does not make multiple coroutines start simultaneously.
    }

    public void UpdateText(string m){
        //Must wait for message to finish before starting next one.
        messages.Enqueue(m);        
        if(!typing)
            StartCoroutine("TypeText");
    }

    private IEnumerator TypeText()
    {
        typing = true;
        while (messages.Count > 0)
        {
            string message = messages.Dequeue();
            dialogueText.text = "";
            foreach (char letter in message.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(lettertime);
            }
            yield return new WaitForSeconds(1f); //Always wait 1 second after typing.
        }   
        typing = false;     
    }
}
