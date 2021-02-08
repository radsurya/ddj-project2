using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTextSystem : MonoBehaviour
{
    public DialogueBox dialogueText;
    public int count;
    public int count2;
    public bool aux = true;
    bool playerFlip = false;
    bool heroFlip = false;
    public bool typing = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupDialog());
    }

    // Update is called once per frame
    void Update()
    {
        typing = dialogueText.typing;
        if (dialogueText.typing && aux) 
        {
            count++;
            Debug.Log("Typing");
            aux = false;
        }

        if (!dialogueText.typing && !aux) 
        {
            count2++;
            Debug.Log("Stopped Typing");
            
            aux = true;
               
        }

        if (count == 15 && !dialogueText.typing && aux) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Delay());
            }
        }

        if (count == 2 && !playerFlip) 
        {
            GameObject.FindWithTag("Player").transform.localScale = new Vector3(45, 45, 45);
            playerFlip = true;
        }

        if (count == 8 && playerFlip)
        {
            GameObject.FindWithTag("Player").transform.localScale = new Vector3(-45, 45, 45);
            playerFlip = false;
        }

        if (count == 9 && !playerFlip)
        {
            GameObject.FindWithTag("Player").transform.localScale = new Vector3(45, 45, 45);
            playerFlip = true;
        }

        if (count == 7 && !heroFlip)
        {
            GameObject.FindWithTag("Hero").transform.localScale = new Vector3(45, 45, 45);
            heroFlip = true;
        }
    }

    IEnumerator SetupDialog() 
    {

        count = 0;

        string[] messages = {
            "??? (On the right): Why, hello there, little man! Are you lost?!", //0
            "YOU: Ah, no. I'm here on a Guild assignment to assist-", //1
            "???: Assistant, you said? I see! I do indeed see! You must be my guild-assigned assistant!", //2
            "ASSISTANT: Ah, well, my name is actually-",   //3
            "???: Excellent news! Fantastic! You shall aid me immensely, I tell you!", //4
            "ASSISTANT: Okay, I think I'm getting the kind of person you are, Mr. Hero...", //5
            "HERO: But of course! You shall witness my heroic feats as we venture into the forest ahead!", //6
            "HERO: Our objective? Well! Well, well, well... I'm sure... that'll become apparent as we explore!", //7
            "ASSISTANT: Ah, yes, the guild assignment. As you know, I've been employed to assist you with this rescue operation.", //8
            "HERO: Ah, that was it! We're rescuing a young boy, kidnapped by foul monsters, was it not?", //9
            "ASSISTANT: Well, reportedly, he made his way into the forest on his own.", //10
            "HERO: I see, and the terrible forest-dwellers keep him captive against his will? Unforgivable!", //11
            "ASSISTANT: Or, perhaps more likely, he has gotten lost. I suggest-", //12
            "HERO: Look out! A menacing looking blob... thing... approaches! Those foul monsters! I shall let them do evil no longer!", //13
            "ASSISTANT: Ah, these creatures do tend to be peaceful unless... provoked... Oh dear."//14
            };

        for (int i = 0; i < messages.Length; i++)
        {
            dialogueText.UpdateText(messages[i]);
        }
      
        yield return new WaitForSeconds(2f);
    }

    IEnumerator Delay()
    {
        SoundManagerScript.playArrowSelectSound();
        yield return new WaitForSeconds(1);

        GameManager.ChangeScene(GameManager.levelScene);
    }
}
