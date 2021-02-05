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
    private bool phase1Complete = false;    //Hero attacks and gets stuck in Big Blob
    private bool phase2Complete = false;    //Big Blob attacks and is divided in two.
    private bool phase3Complete = false;
    private bool phase4Complete = false;
    private bool phase5Complete = false;
    //LISTS
    List<string> intents = new List<string>();
    List<Action> actions = new List<Action>();
    List<string> outcomes = new List<string>();

    //OBJECT NAMES
    private string hero = "Hero";
    private string assistant = "Assistant";
    private string bigBlob = "Big Blob";
    private string notSoBigBlobA = "Not-so-Big Blob A";
    private string notSoBigBlobB = "Not-so-Big Blob B";    

    public void runTurn(){
        runTurn(Action.SKIP, "");
    }

    public void runTurn(Action playerAction, string target){
        intents.Clear();
        actions.Clear();
        outcomes.Clear();
        if(!phase1Complete){
            Debug.Log("Entered the correct phase, and my actions is "+playerAction);
            turnOne(playerAction, target);
        }else if(!phase2Complete){
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
        }else if(!phase3Complete){
            //TODO
        }
    }

    public List<string> getTurnIntent(){
        return intents;
    }
    public Action getHeroAction(){
        Debug.Log("Calling action list.");
        Debug.Log(actions[0]);
        return actions[0];
    }
    public List<Action> getEnemyActions(){
        actions.RemoveAt(0);
        return actions;
    }
    public List<string> getTurnOutcome(){        
        return outcomes;
    }

///////////////////////////////////////TURNS///////////////////////////////////////
    private void turnOne(Action playerAction, string target){
        //hero
        intents.Add("The Hero prepares to slash the Big Blob.");
        //enemy
        intents.Add("The Big Blob bobbles menacingly.");
        if(playerAction == Action.SKIP)
            return; //No need to process rest of behaviour.
        if(playerAction == Action.STOP){
            if(target.Equals(hero)){    //Try to stop the hero
                actions.Add(Action.SKIP);   //Hero
                outcomes.Add("Having stopped the Hero from acting with your persuasive speech...");
                outcomes.Add("You both look at one another and contemplate your life choices.");
                outcomes.Add("Specifically your choice of stopping him and his of listening to you.");
                actions.Add(Action.SKIP);   //Big Blob
                if(deadTurn > 0){
                    outcomes.Add("Really, you're both rather puzzled at this point.");
                    outcomes.Add("The Big Blob bobbles in puzzlement.");
                }else{
                    outcomes.Add("The Big Blob continues to bobble menacingly, moved by your speech.");
                }
                deadTurn++;
                return;
            }else{  //Try to stop anything else
                outcomes.Add("Your efforts at stopping something that has no intention to move were successful!");
            }                            
        }else if (playerAction == Action.PUSH){
            if(target.Equals(hero)){    //Try to push the hero
                outcomes.Add("The Hero's target did change: to the one and only Big Blob."
                +"\nI mean, what did you expect?");
            }else if(target.Equals(bigBlob)){
                //TODO: FAIL GAME - PRINT ENDING, ETC
            }    
            //Pushing other things does nothing.        
        }
        //Using items does nothing.
        phase1Complete = true;
        deadTurn = 0; //Each flag/fase set resets the dead turn.
        actions.Add(Action.SKIP);   //Hero
        outcomes.Add("Hero: “Take this, you big blob!”");
        outcomes.Add("The Big Blob seems offended by the slander.");
        outcomes.Add("The Hero slashes the Big Blob powerfully! ");
        outcomes.Add("The Hero’s sword gets predictably stuck in the Big Blob.");
        outcomes.Add("Hero: “Why, you! Give me my sword back!");
        outcomes.Add("...Well, no matter! Let me just… try to… drag it out of there…”");
        actions.Add(Action.SKIP);   //Big Blob
        outcomes.Add("The Big Blob nonchalantly bobbles the sword around.");        
    }

}
