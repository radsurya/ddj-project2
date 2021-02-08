using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Generalise for multiple scripts later.
public class BattleScript{
    //TODO: BattleSystem could call setup and update functions in the Script rather than the
    //script having to know the battlesystem.
    private BattleSystem battleSystem; 
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
    
    public BattleScript(BattleSystem battleSystem){
        this.battleSystem = battleSystem;
    }

    public void runTurn(){
        runTurn(Action.SKIP, "");
    }

    public void runTurn(Action playerAction, string target){
        intents.Clear();
        actions.Clear();
        outcomes.Clear();
        if(!phase1Complete){
            turnOne(playerAction, target);
        }else if(!phase2Complete){
            turnTwo(playerAction, target);
        }else if(!phase3Complete){
            turnThree(playerAction, target);
        }else if(!phase4Complete){
            turnFour(playerAction, target);
        }else if(!phase5Complete){
            turnFive(playerAction, target);
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
///////////////////////////////////////BEHAVIOUR FUNCTIONS///////////////////////////////////////
    private string findHighestHealth(){
        string highestHealth = null;
        int max = 0;
        Debug.Log("Find highest health: "+battleSystem.enemies);
        for (int i = 0; i < battleSystem.enemies.Length && battleSystem.enemies[i] != null; i++){
            if(battleSystem.enemies[i].currentHP > max){
                highestHealth = battleSystem.enemies[i].unitName;
                max = battleSystem.enemies[i].currentHP;
            }
        }
        return highestHealth;
    }

    private string getNextUnit(string current){
        string next = current;
        foreach (Unit enemy in battleSystem.enemies){
            if(!enemy.unitName.Equals(current))
                next = enemy.unitName;
        }
        return next;
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

        actions.Add(Action.SKIP);
        actions.Add(Action.ATTACK); targets.Add(hero);
        if (playerAction == Action.STOP){
            if (target.Equals(hero)){    //Try to stop the hero
                outcomes.Add("Unfortunately, the Hero was too busy faffing about with his swords to listen to you.");
                outcomes.Add("In any case, what the Hero is currently doing is not much different from nothing.");
            }else if (target.Equals(bigBlob)){
                outcomes.Add("As such, your measured argument was like blob in one ear and blob out the other.");
            }else{  //Try to stop anything else
                outcomes.Add("Your efforts at stopping something that has no intention to move were successful!");
            }
        }else if (playerAction == Action.PUSH){
            if (target.Equals(hero)){    //Try to push the hero
                outcomes.Add("Yet, however much they'd like to attack, they are otherwise engaged for the moment.");
            }else if (target.Equals(bigBlob)){
                end = 2;
            } //Pushing other things does nothing. 
        }
        phase2Complete = true;
        deadTurn = 0; //Each flag/fase set resets the dead turn.

        outcomes.Add("The Hero continues to grip their sword in hopes that it somehow comes off.\nEventually.");

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
    }

    private void turnThree(Action playerAction, string target){
        string heroTarget = findHighestHealth();
        intents.Add("The Hero prepares to slash at "+heroTarget+", his nose covered in snot.");
        intents.Add("Hero: “It’s not snot! It’s blob! You covered my nose in blob, one of you!”");
        intents.Add("The Blobs bobble mockingly.");
        intents.Add("You try to avoid mocking the Hero, since that sword is looking pretty sharp, as the now divided blob can attest to.");
        if(playerAction == Action.SKIP)
            return; //No need to process rest of behaviour.
        if(playerAction == Action.STOP){
            if(target.Equals(hero)){    //Try to stop the hero
                outcomes.Add("You briefly make the Hero reconsider the value of blob as a fashion statement.");
                outcomes.Add("The Hero refrains from attacking as he ponders your valuable advice.");
                if(deadTurn > 0){
                    outcomes.Add("You yourself ponder at the Hero's propensity to ponder at your command.");
                    outcomes.Add("The Not-so-Big Blobs ponder on whether to bobble menacingly.");
                }else{
                    outcomes.Add("The Not-so-Big Blobs continue to bobble menacingly, ready to push the fashion world forward everywhere.");
                }
                deadTurn++;
                actions.Clear(); targets.Clear();
                actions.Add(Action.SKIP);
                actions.Add(Action.SKIP);  
                actions.Add(Action.SKIP);
                return;  
            }
        }else if (playerAction == Action.PUSH){
            if(target.Equals(hero)){    //Try to push the hero
                heroTarget = getNextUnit(heroTarget);
            }     
        }//Using items does nothing.
        //ACTIONS
        actions.Add(Action.ATTACK); targets.Add(heroTarget);
        actions.Add(Action.SKIP);
        actions.Add(Action.SKIP);

        phase3Complete = true;
        deadTurn = 0;

        outcomes.Add("Hero: “Take this, you… small blob!?”");
        outcomes.Add("The Not-so-Big Blob is thinking of hiring a lawyer to sue the Hero for slander.");
        outcomes.Add("The Hero slashes "+targets[0]+" powerfully!");
        outcomes.Add("The slash is mildly uncomfortable on its blobby body. Another dose of sword is likely to persuade it to leave.");
    }
    
    private void turnFour(Action playerAction, string target){
        string heroTarget = findHighestHealth();
        intents.Add("The Hero prepares to slash at Not-so-Big Blob A, his nose covered in snot.");
        intents.Add("Hero: “Achoo!”");
        intents.Add("The Blobs, disgusted at the Hero’s snot, prepare to jump at you. Two blobs of their size is too much for you to handle.");
        intents.Add("Hero: “It’s your fault, dammit! Prepare to taste steel!”");
        intents.Add("You overcome your instincts and stop yourself from mentioning the Hero’s sword is not made of steel.");
        if(playerAction == Action.SKIP)
            return; //No need to process rest of behaviour.
        if(playerAction == Action.STOP){
            if(target.Equals(hero)){    //Try to stop the hero                
                outcomes.Add("You briefly make the Hero reconsider the value of blob as a fashion statement.");
                outcomes.Add("The Hero refrains from attacking as he ponders your valuable advice.");
                actions.Add(Action.SKIP);   //Hero
                actions.Add(Action.ATTACK); targets.Add(assistant);
                actions.Add(Action.ATTACK); targets.Add(assistant);
                end = 2;
                return;
            }
        }else if(playerAction == Action.HIDE){
            if (target.Equals(hero)){ 
                outcomes.Add("The Hero slashes Not-so-Big Blob B powerfully!");
                outcomes.Add("The slash is mildly uncomfortable on its blobby body. Another dose of sword is likely to persuade it to leave.");
                outcomes.Add("Both blobs jump at you!");
                outcomes.Add("But you hid behind the Hero, so they each hit one of their arms, sticking there like the sticky blobs they are.");
                outcomes.Add("Hero: “What the!? Let go of me at once! They explicitly told me last time not to come back home full of mucous!”");
                outcomes.Add("The Hero starts flailing around to get rid of the blobs.");
                outcomes.Add("It suddenly hits you that you’re right behind the Hero. Like, literally hits you. With his arm.");
                outcomes.Add("And it’s the last thing you remember.");
                outcomes.Add("When you next awake, you find yourself on a comfortable bed. A springy bed. A viscous bed. And you’re sinking into it!");
                outcomes.Add("As it turns out, that bed became your new home, for your new life as the best friend of a giant blob.");
                outcomes.Add("At least your new big and strong friend doesn’t attack you accidentally as much.");
                actions.Add(Action.ATTACK); targets.Add(heroTarget);
                actions.Add(Action.ATTACK); targets.Add(hero);
                actions.Add(Action.ATTACK); targets.Add(hero);
                end = 2;
                return; //FAIL - Hid behind hero and being emprisoned by the blob
            }if(target.Equals(rock)){

            }
        }else if (playerAction == Action.PUSH){
            if(target.Equals(hero)){    //Try to push the hero
                heroTarget = getNextUnit(heroTarget);
            }    
        }//Using items does nothing.

        if(playerAction != Action.HIDE && !(playerAction == Action.PUSH && target.Equals(hero))){
            outcomes.Add("The Hero slashes Not-so-Big Blob B powerfully!");
            outcomes.Add("The slash is mildly uncomfortable on its blobby body. Another dose of sword is likely to persuade it to leave.");
            outcomes.Add("Both blobs jump at you!");
            outcomes.Add("They each hit one of your arms, getting stuck to them and immobilizing you.");
            outcomes.Add("Hero: “Blast! It escaped. Where did you- Goodness! My friend, they’re eating you!”");
            outcomes.Add("Assistant: “No they’re not.”");
            outcomes.Add("Hero: “Rest assured, you’ll be avenged.”");
            outcomes.Add("Assistant: “No they’re not. No they’re not. No they’re not. There’s nothing to avenge!”");
            outcomes.Add("As your arms are immobilized, there’s nothing you can do to stop the Hero from raising his sharp, scary, dangerous sword.");
            outcomes.Add("Assistant: “No. No! Stop! Ack! I’m outta here!”");
            outcomes.Add("Realising you still have two limbs which aren’t immobilised, you make like a tree and leave.");
            outcomes.Add("You spend the rest of your days cohabiting the dungeon with your new blobby neighbours, Lefty and Righty, names they decided based on their respective arms.");
            outcomes.Add("On some days you can still hear the Hero wandering around, looking for his next opportunity to save you.");
            end = 2; //FAIL - Doing nothing and being emprisoned by the blob
        }else{
            outcomes.Add("Hero: “Take this, you hypocritical blob!”");
            outcomes.Add("The Not-so-Big Blob regrets not having a cellphone to call its lawyer.");
            outcomes.Add("The Hero slashes at the blob they aimed for, but your shove makes them hit " + heroTarget + "!");
            outcomes.Add("The slashes are getting to be annoying, so the blob bobbles away.");
            outcomes.Add("Hero: “Ha! A hypocritical coward!”");
            outcomes.Add(getNextUnit(heroTarget) + " jumps in your general direction!");
            outcomes.Add("The Blob hits you in the arm, absorbing it!");
            outcomes.Add("Assistant: “Oh no, I can’t move my arm. And the Hero will probably...”");
            outcomes.Add("Hero: “I got one, my friend. Now it’s just…”");
            outcomes.Add("Hero: “Goodness! It ate your arm!”");
            outcomes.Add("Hero: “Worry not! I shall avenge you at once!”");
            outcomes.Add("Assistant: “I’m still alive and it didn’t eat my arm!”");

        }
        phase4Complete = true;
        deadTurn = 0;
        
        actions.Add(Action.ATTACK); targets.Add(heroTarget);
        actions.Add(Action.SKIP);
        actions.Add(Action.SKIP);
    }

    private void turnFive(Action playerAction, string target){
        string heroTarget = findHighestHealth();
        intents.Add("The Hero prepares to slash… well, you!");
        intents.Add("Hero: “I’ll save what’s left of you, friend!”");
        intents.Add("Assistant: “Everything’s left of me, so put that sword away!”");
        intents.Add("The Blob does not bobble, since it’s stuck on your arm, not on the ground. But it’s stuck on your arm mockingly.”");
        intents.Add("You realise getting close to the Hero when they’re preparing to slash your arm off might not be the brightest idea.");
        if(playerAction == Action.SKIP)
            return; //No need to process rest of behaviour.
        if(playerAction == Action.HIDE){
            if (target.Equals(hero)){ 
                outcomes.Add("You run behind the Hero.");
                outcomes.Add("Hero: “Wait, I can’t see you like this. You must let me save you, friend!”");
                outcomes.Add("Assistant: “I must question your definition of save!”");
                outcomes.Add("The Hero starts turning around, but you stick to his back, your arm on the line.");
                outcomes.Add("Hero: “Oh my, this is making me dizzy… Wait, I got an idea.”");
                outcomes.Add("Suddenly, the Hero starts spinning like a ballerina, their sword held out!");
                outcomes.Add("You decide it’s best to close your eyes and not think of what happens next.");
                outcomes.Add("Thankfully, the Hero held the sword with the flat side of the blade up, so no heads went flying off, only your consciousness.");
                outcomes.Add("When you next awake, you find yourself on a comfortable bed. A springy bed. A viscous bed. And you’re sinking into it!");
                outcomes.Add("As it turns out, that bed became your new home, for your new life as the best friend of a giant blob.");
                outcomes.Add("At least your new big and strong friend doesn’t attack you accidentally as much.");
                actions.Add(Action.ATTACK); targets.Add(heroTarget);
                actions.Add(Action.SKIP);
                actions.Add(Action.SKIP);
                end = 2;
                return; //FAIL - Hid behind hero and being emprisoned by the blob
            }if(target.Equals(rock)){

            }
        }else if (playerAction == Action.PUSH){
            if(target.Equals(hero)){    //Try to push the hero
                heroTarget = getNextUnit(heroTarget);
            }    
        }//Using items does nothing.

        if(playerAction == Action.STOP){
            if(target.Equals(hero)){    //Try to stop the hero                
                outcomes.Add("You calmly and eloquently ask the Hero to stop, holding out your free arm.");
                outcomes.Add("Assistant: “STOOOOOOOP! AAAAHHHHH! PLEASEEEEE!!!”");
                outcomes.Add("The Hero is stunned by the sudden shout.");
                outcomes.Add("Hero: “Ho-ho! I didn’t know you could shout that loud! Ow. My ears.”");
                outcomes.Add("The Blob heard your calm request with its nonexistent ears. Deafened, it decides this really isn’t worth it, sliding off your arm and bobbling away.");
            }
        }
        phase5Complete = true;
        deadTurn = 0;
        
        end = 1; //win the game
        actions.Add(Action.SKIP);
        actions.Add(Action.SKIP);
        actions.Add(Action.SKIP);
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
