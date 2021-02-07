using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTextSystem : MonoBehaviour
{
    public DialogueBox dialogueText;
    public int count;
    bool aux = true;
    bool playerFlip = false;
    bool heroFlip = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupDialog());
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueText.typing && aux) 
        {
            count++;
            Debug.Log("Typing");
            aux = false;
        }

        if (!dialogueText.typing && !aux) 
        {
            Debug.Log("Stopped Typing");
            aux = true;
        }

        if (count == 12 && !dialogueText.typing) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.ChangeScene(GameManager.levelScene);
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
            "HERO: Why hello there little man! Are you lost?!", //0
            "YOU: Oh... Hi Mr... Big... Hero Guy...", //1
            "HERO: Listen friend, I don't know how you got here, but you should know that this place is pretty dangerous!", //2
            "HERO: There are all kinds of menacing, and not so menacing monsters out here!", //3
            "YOU: Humm... Actually I was just passing here on my way home... I come this way pretty often...", //4
            "HERO: Worry not my friend! I shall help guide you across this journey!", //5
            "HERO: Follow me, we shall take a shortcut through that completely harmless looking cave, that is surely not filled with monsters that could threaten your life!", //6
            "YOU: Wait... Hum... Actually, I usually go this other way, its a bit faster...", //7
            "HERO: Come now! If we do find any enemies, I shall them with ease!", //8
            "YOU: Oh... Ok then... Sure I guess...", //9
            "YOU: I got a bad feeling about this...", //10
            "HERO: Look out a menacing looking blob... thing... approaches! (Click to continue)", //11
            };

        for (int i = 0; i < messages.Length; i++)
        {
            dialogueText.UpdateText(messages[i]);
        }
      
        yield return new WaitForSeconds(2f);
    }
}
