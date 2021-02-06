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
    //Actions and targets are in order: Hero > Enemy A > Enemy B > ... > last Enemy
    List<Action> actions = new List<Action>();
    List<string> targets = new List<string>();
    List<string> outcomes = new List<string>();

    //OBJECT NAMES
    private string hero = "Hero";
    private string assistant = "Assistant";
    private string bigBlob = "Big Blob";
    private string notSoBigBlobA = "Not-so-Big Blob A";
    private string notSoBigBlobB = "Not-so-Big Blob B"; 
    private string rock = "Rock";

    public int end = 0; //0 - not finished, 1 - won, 2 - lost

    public void runTurn(){
        runTurn(Action.SKIP, "");
    }

    public void runTurn(Action playerAction, string target){
        intents.Clear();
        actions.Clear();
        outcomes.Clear();
        if(!phase1Complete){
            Debug.Log("Entered phase 1, and my action is "+playerAction);
            turnOne(playerAction, target);
        }else if(!phase2Complete){
            turnTwo(playerAction, target);
        }else if(!phase3Complete){
            turnThree(playerAction, target);
        }
    }

    public List<string> getTurnIntent(){
        return intents;
    }
    public Action getNextAction(){
        Action nextAction = actions[0];
        actions.RemoveAt(0);
        return nextAction;
    }
    public string getNextTarget(){
        string nextTarget = targets[0];
        targets.RemoveAt(0);
        return nextTarget;
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
                outcomes.Add("Specifically your choice of stopping them and their choice of listening to you.");
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
                end = 2;
                return;
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
        outcomes.Add("Hero: “Why, you! Give me my sword back!"
            +"\n...Well, no matter! Let me just… try to… drag it out of there…”");
        actions.Add(Action.SKIP);   //Big Blob
        outcomes.Add("The Big Blob nonchalantly bobbles the sword around.");    
        //Hiding does nothing.     
    }

    private void turnTwo(Action playerAction, string target){
        //hero
        intents.Add("Hero: “What’s this stuff made of?? The darn thing’s really stuck in there.”");
        intents.Add("The Hero grips and pulls the sword, but cannot remove it from the Big Blob.");
        //enemy
        intents.Add("The Big Blob seems to be preparing to jump at you!");
        intents.Add("Assistant: “Wait, me??");
        if(playerAction == Action.SKIP)
            return; //No need to process rest of behaviour.
        if (playerAction == Action.STOP)
        {
            if (target.Equals(hero))
            {    //Try to stop the hero
                outcomes.Add("Unfortunately, the Hero was too busy faffing about with his swords to listen to you.");
                outcomes.Add("In any case, what the Hero is currently doing is not much different from nothing.");
            }
            else if (target.Equals(bigBlob))
            {
                outcomes.Add("As such, your measured argument was like blob in one ear and blob out the other.");
            }
            else
            {  //Try to stop anything else
                outcomes.Add("Your efforts at stopping something that has no intention to move were successful!");
            }
        }
        else if (playerAction == Action.PUSH)
        {
            if (target.Equals(hero))
            {    //Try to push the hero
                outcomes.Add("Yet, however much they'd like to attack, they are otherwise engaged for the moment.");
            }
            else if (target.Equals(bigBlob))
            {
                end = 2;
            } //Pushing other things does nothing.      

        }
        phase2Complete = true;
        deadTurn = 0; //Each flag/fase set resets the dead turn.

        actions.Add(Action.SKIP);
        outcomes.Add("The Hero continues to grip their sword in hopes that it somehow comes off.\nEventually.");

        actions.Add(Action.ATTACK); targets.Add(hero);
        outcomes.Add("The Big Blob jumps in your general direction!");
        outcomes.Add("Because the Hero was gripping their sword, it tears through the Big Blob when it jumps!");
        outcomes.Add("The Big Blob is divided into two Not-so-Big Blobs.");
        if (playerAction == Action.HIDE){
            if(target.Equals(hero)){    //Try to hide behind the hero
                outcomes.Add("As you hid behind the Hero, the Not-so-Big Blobs hit them in the face.");  
            }else if(target.Equals(rock)){
                outcomes.Add("As you hid behind the rock, the Not-so-Big Blobs perform an atletic feat of gravity defying turns and head straight for the Hero!");
            }    
            //Hiding elsewhere does nothing.        
        }else{
            outcomes.Add("Their jump leads them to land on your head, where they join back into a Big Blob.");
            outcomes.Add("Assistant: “Oh, no. My head…!");
            outcomes.Add("Due to the weight of the Big Blob, it starts sliding down, completely enveloping you.");
            outcomes.Add("Assistant: “I knew it... “");
            outcomes.Add("You swiftly turn into a blob of your own. Congratulations.");
            outcomes.Add("Hero: “I got my sword back! Yes!\nNow, prepare yourself at once you fiendish…!”");
            outcomes.Add("Hero: “Wait a second. There’s more of you now??”");
            outcomes.Add("Hero: “Ha! Simply a chance to flex my skills. Ready yourself, you Glasses-Wearing Blob!”");
            outcomes.Add("Glasses-Wearing Blob: “Haahh…”");
            outcomes.Add("You spend the rest of your life bobbing around the dungeon, trying not to look menacing.");
            outcomes.Add("Occasionally you are visited by your friend the Hero, who may or may not be trying to slay you.");
            end = 2;
            return; //FAIL: LOSE - Turn into a blob.
        }
        outcomes.Add("Hero: “Ack! My beautiful nose!”");
        //outcomes.Add("Scene2");
    }
    private void turnThree(Action playerAction, string target){
        intents.Add("The Hero prepares to slash at "+notSoBigBlobA+", his nose covered in snot.");
        intents.Add("Hero: “It’s not snot! It’s blob! You covered my nose in blob, one of you!”");
        intents.Add("The Blobs bobble mockingly.");
        intents.Add("You try to avoid mocking the Hero, since that sword is looking pretty sharp, as the now divided blob can attest to.");
        if(playerAction == Action.SKIP)
            return; //No need to process rest of behaviour.
        if(playerAction == Action.STOP){
            if(target.Equals(hero)){    //Try to stop the hero
                actions.Add(Action.SKIP);   //Hero
                outcomes.Add("You briefly make the Hero reconsider the value of blob as a fashion statement.");
                outcomes.Add("The Hero refrains from attacking as he ponders your valuable advice.");
                actions.Add(Action.SKIP);   //notSoBigBlobA
                actions.Add(Action.SKIP);   //notSoBigBlobB
            }
            if(deadTurn > 0){
                outcomes.Add("You yourself ponder at the Hero's propensity to ponder at your command.");
                outcomes.Add("The Not-so-Big Blobs ponder on whether to bobble menacingly.");
            }else{
                outcomes.Add("The Not-so-Big Blobs continue to bobble menacingly, ready to push the fashion world forward everywhere.");
            }
            deadTurn++;
            return;

        }else if (playerAction == Action.PUSH){
            if(target.Equals(hero)){    //Try to push the hero
                targets.Add(notSoBigBlobB);
            }else if(target.Equals(bigBlob)){
                end = 2;
            }     
        }//Using items does nothing.

        //Attacks A if target was not changed.
        if (playerAction != Action.PUSH || !target.Equals(hero)){
            targets.Add(notSoBigBlobA);
        }

        outcomes.Add("Hero: “Take this, you… small blob!?”");
        outcomes.Add("The Not-so-Big Blob is thinking of hiring a lawyer to sue the Hero for slander.");
        outcomes.Add("The Hero slashes "+targets[0]+" powerfully!");
        outcomes.Add("The slash is mildly uncomfortable on its blobby body. Another dose of sword is likely to persuade it to leave.");
    }
}
/*  TEMPLATE
private void turnThree(Action playerAction, string target){
        //hero
        //enemy
        if(playerAction == Action.SKIP)
            return; //No need to process rest of behaviour.
        if(playerAction == Action.STOP){
            if(target.Equals(hero)){    //Try to stop the hero
            }else if(target.Equals(bigBlob)){
            }else{  //Try to stop anything else
            }                            
        }else if (playerAction == Action.PUSH){
            if(target.Equals(hero)){    //Try to push the hero
            }else if(target.Equals(bigBlob)){
                //TODO: FAIL GAME - PRINT ENDING, ETC
            } //Pushing other things does nothing.        
        }//Using items does nothing.
        if (playerAction == Action.HIDE){
            if(target.Equals(hero)){    //Try to hide behind the hero
            }else if(target.Equals(rock)){
            }    
            //Hiding elsewhere does nothing.        
        }else{
        }
    }
*/
