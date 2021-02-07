using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTextSystem : MonoBehaviour
{
    public DialogueBox dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupDialog());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetupDialog() 
    {
        string[] messages = {
            "HERO: Why hello there little man! Are you lost?!", //0
            "YOU: Oh... Hi Mr... Big... Hero Guy...", //1
            "HERO: Listen friend, I don't know how you got here, but you should know that this place is pretty dangerous!", //2
            "HERO: There are all kinds of menacing, and not so menacing monsters out here!", //3
            "YOU: Humm... Actually I was just passing here on my way home... I come this way pretty often...", //4
            "HERO: Worry not my friend! I shall help guide you across this journey!", //5
            "HERO: Follow me, we shall take a shortcut through that completely harmless looking cave, that is surely not filled with monsters that could threaten your life!", //6
            "YOU: Wait... Hum... Actually, I think this other way is faster...", //7
            "HERO: Come now! If we do find any enemies, I shall them with ease!", //8
            "YOU: Oh... Ok then... Sure...", //9
            "YOU: (I got a bad feeling about this)", //10
            "HERO: Look out a menacing looking blob... thing... approaches!", //11
            };

        dialogueText.UpdateText(messages[0]);
        dialogueText.UpdateText(messages[1]);
        //if(dialogueText.numberOfMessages() )
        GameObject.FindWithTag("Player").transform.localScale = new Vector3(45, 45, 45); // Virar assistente para a direita

        for (int i = 0; i < messages.Length; i++)
        {
            dialogueText.UpdateText(messages[i]);
            if (i == 1) 
            {
                GameObject.FindWithTag("Player").transform.localScale = new Vector3(45, 45, 45); // Virar assistente para a direita
            }

            if (i == 6) 
            {
                GameObject.FindWithTag("Hero").transform.localScale = new Vector3(45, 45, 45); // Virar heroi para a direita
            }
        }
        yield return new WaitForSeconds(2f);
    }
}
