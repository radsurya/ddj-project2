using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Generalise for multiple scripts later.
public class BattleScript{
    /*The battle progresses in phases. The first phase is the hero attacking the blob.
    The second phase is the Blob attacking us.
    Therefore, we can keep track of how many turns the current phase took (for dialogue or any other reason).*/
    int deadTurn = 0;
    //FLAGS
    private bool heroStuck = false;
    //LISTS
    List<string> intents = new List<string>();
    List<Action> actions = new List<Action>();
    List<string> outcomes = new List<string>();

    public void playerAction(Action playerAction){
        //TODO: runTurn again but with action in mind.
    }

    public void runTurn(){
        intents.Clear();
        actions.Clear();
        outcomes.Clear();
        if(!heroStuck){
            //hero
            intents.Add("The Hero prepares to slash the Big Blob.");
            actions.Add(Action.SKIP);
            heroStuck = true;
            deadTurn = 0; //Each flag/fase set resets the dead turn.
            outcomes.Add("Hero: “Take this, you big blob!”");
            outcomes.Add("The Big Blob seems offended by the slander.");
            outcomes.Add("The Hero slashes the Big Blob powerfully! ");
            outcomes.Add("The Hero’s sword gets predictably stuck in the Big Blob.");
            outcomes.Add("Hero: “Why, you! Give me my sword back!");
            outcomes.Add("...Well, no matter! Let me just… try to… drag it out of there…”");
            //enemy
            intents.Add("The Big Blob bobbles menacingly.");
            actions.Add(Action.SKIP);
            outcomes.Add("The Big Blob nonchalantly bobbles the sword around.");
        }else{
            //hero
            intents.Add("Hero: “What’s this stuff made of?? The darn thing’s really stuck in there.”");
            intents.Add("The Hero grips and pulls the sword, but cannot remove it from the Big Blob.");
            actions.Add(Action.TAUNT);
            outcomes.Add("The Hero continues to grip his sword in hopes that it somehow comes off.\nEventually.");
            //enemy
            intents.Add("The Big Blob seems to be preparing to jump at you!");
            intents.Add("Assistant: “Wait, me??");
            actions.Add(Action.ATTACK);
            outcomes.Add("The Big Blob jumps in your general direction!");
            outcomes.Add("Because the Hero was gripping their sword, it tears through the Big Blob when it jumps!");
            outcomes.Add("The Big Blob is divided into two Not-so-Big Blobs.");
            outcomes.Add("As you hid behind the Hero, the Not-so-Big Blobs hit them in the face.");
            outcomes.Add("Hero: “Ack! My beautiful nose!”");
        }
    }

    public List<string> getTurnIntent(){
        return intents;
    }
    public Action getHeroAction(){
        return actions[0];
    }
    public List<Action> getEnemyActions(){
        actions.RemoveAt(0);
        return actions;
    }
    public List<string> getTurnOutcome(){        
        return outcomes;
    }
}
