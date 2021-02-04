using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Generalise for multiple scripts later.
public class BattleScript{
    /*The battle progresses in phases. The first phase is the hero attacking the blob.
    The second phase is the Blob attacking us.
    Therefore, we can keep track of how many turns the current phase took (for dialogue or any other reason).*/
    int deadTurn = 0;
    private bool heroStuck = false;
    public string getHeroIntent(){        
        if(!heroStuck){
            return "The Hero prepares to slash the Big Blob.";
        }else{
            if(deadTurn == 0)
                return "Hero: “What’s this stuff made of?? The darn thing’s really stuck in there.” The Hero grips and pulls the sword, but cannot remove it from the Big Blob.";
            else
                return "The Hero continues to grip his sword in hopes that it somehow comes off. Eventually.";
        }
    }
    public Action getHeroAction(){
        if(!heroStuck){
            heroStuck = true;
            deadTurn = 0; //Each flag/fase set resets the dead turn.
            return Action.ATTACK;
        }else{
            deadTurn++;
            return Action.TAUNT;
        }
    }
    public List<string> getHeroOutcome(){
        List<string> sentences = new List<string>();
        if(!heroStuck){
            sentences.Add("Hero: “Take this, you big blob!”");
            sentences.Add("The Big Blob seems offended by the slander.");
            sentences.Add("The Hero slashes the Big Blob powerfully! ");
            sentences.Add("The Hero’s sword gets predictably stuck in the Big Blob.");
            sentences.Add("Hero: “Why, you! Give me my sword back!\n...Well, no matter! Let me just… try to… drag it out of there…”");
        }
        return sentences;
    }

    public void getEnemyAction(){
        
    }
}
