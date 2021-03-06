﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.SceneManagement;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    static float lettertime = 0.05f;

    public bool typing = false;
    
    private static Queue<string> messages = new Queue<string>();
    private string currentMessage;

    /*Updates text to display the following messages.*/
    public void UpdateText(List<string> m){
        //Receive lists.
        foreach (string s in m){
            UpdateText(s);
        }
    }
public bool dialogueFinished = false;
    /*Updates text to display the following message.*/
    public void UpdateText(string m){
        //Must wait for message to finish before starting next one.
        messages.Enqueue(m);
    }
    
    /*Logic for typing the text in textbox character by character.*/
    private IEnumerator TypeText()
    {
        typing = true;
        if (messages.Count > 0) //In theory this check is redundant.
        {
            currentMessage = messages.Dequeue();

            /*if(currentMessage.Contains("Scene")){   //WARNING: Very rough method.
                SceneManager.LoadScene(currentMessage);
            }*/

            if(currentMessage.Contains("The Big Blob is divided into two Not-so-Big Blobs.")){   //WARNING: Very rough method.
                GameObject.Find("BattleSystem").GetComponent<BattleSystem>().changeScene();
            }

            if (currentMessage.Contains("You have survived yet another ordeal!"))
            {   
                BattleMusic.muteMusic();
                SoundManagerScript.playVictoryMusic();
            }
            if (currentMessage.Contains("GAME OVER"))
            {
                BattleMusic.muteMusic();
                SoundManagerScript.playLossMusic();
            }

            dialogueText.text = "";
            foreach (char letter in currentMessage.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(lettertime);
            }
            yield return new WaitForSeconds(1f); //Always wait 1 second after typing.
        }   
        typing = false;
    }

    /*Function that registers text box clicks to advance the text or move to next line.*/
    public void OnMouseDown(){
        if (Input.GetMouseButtonDown(0)){
            if(typing){
                StopCoroutine("TypeText");
                typing = false;
                dialogueText.text = currentMessage;
            }else if(messages.Count > 0){
                StartCoroutine("TypeText");
            }
        }
    }

    public void forceDialogueAdvance(string m){  
        UpdateText(m);
        //TODO: Check if calling this with no coroutine is safe.
        StopCoroutine("TypeText");  
        StartCoroutine("TypeText");
    }

    /*Function for identifying if dialogue box has no further tasks.*/
    public bool isIdle(){
        return messages.Count <= 0 && !typing; 
    }

    public int numberOfMessages(){
        return messages.Count;
    }

    public static void CleanMessages(){
        messages.Clear();
    }
}
