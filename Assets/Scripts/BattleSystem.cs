using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, HEROTURN, ENEMYTURN, WON, LOST }

public enum Action { ATTACK, DEFEND, TAUNT, IDLE, SKIP, HIDE, STOP, PUSH, ITEM }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    private Action currentAction = Action.SKIP;

    public BattleHud heroHud;
    public BattleHud assistantHud;
    public BattleHud[] enemyHUDs = new BattleHud[4];

    public Unit heroUnit;
    public Unit assistantUnit;
    public Unit[] enemies = new Unit[4];

    public DialogueBox dialogueText;

    private BattleScript battleScript = new BattleScript();  
    //Game level. Tutorial is 0.
    //int level = 0;
    //Game turn.
    //int turn = 0;

    // Start is called before the first frame update
    void Start(){
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }
    //IEnumerator to turn setup into Coroutine and make it wait.
    IEnumerator SetupBattle(){
        //WARNING: Each scene needs to be manually prepared, for different levels.

        //Initialise assistant object.
        assistantHud.SetHUD(assistantUnit);
        //Initialise hero object.
        heroHud.SetHUD(heroUnit);
        //Initialise enemy objects
        List<Unit> livingEnemies = new List<Unit>();
        for (int i = 0; i < enemies.Length && enemies[i] != null; i++){
            livingEnemies.Add(enemies[i]);
            enemyHUDs[i].SetHUD(enemies[i]);
            //TODO: Custom enemy walk in text
            dialogueText.UpdateText("A "+enemies[i].unitName+" politely approaches.");  
            yield return new WaitForSeconds(2f);    //Wait time should match enemy entrance text.
        }
        enemies = livingEnemies.ToArray();     
        
        //Setup finished, change state
        changeState();
    }

//////////////////////////////////////////////TURN FUNCTIONS//////////////////////////////////////////////

    void PlayerTurn(){
        dialogueText.UpdateText("Your time to act!");
        //The game does nothing during our turn, moving on only through inputs.
    }

    IEnumerator HeroTurn(){
        Action action = battleScript.getHeroAction();
        bool isDead = false;
        yield return StartCoroutine(processGenericAction(battleScript.getHeroAction(), heroUnit, enemies[0], 
            enemyHUDs[0], boolean => isDead = boolean));
        //yield return new WaitForSeconds(1f);
        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }else{
            changeState();
        }        
    }

    IEnumerator EnemyTurn(){
        List<Action> actions = battleScript.getEnemyActions();
        bool isDead = false;
        for (int i = 0; i < enemies.Length; i++){
            yield return StartCoroutine(processGenericAction(actions[i], enemies[i], assistantUnit, 
                assistantHud, boolean => isDead = boolean));
            //yield return new WaitForSeconds(1f);
            if(isDead){
                state = BattleState.LOST;
                EndBattle();
            }
        }
        if(!isDead){
            //End of turn.
            changeState();
        }         
    }

    void EndBattle(){
        if(state == BattleState.WON){
            dialogueText.UpdateText("You have survived yet another ordeal!");
        }else if (state == BattleState.LOST){
            dialogueText.UpdateText("The situation has developed not necessarily to your advantage.");
        }
    }

//////////////////////////////////////////////PROCESS ACTIONS//////////////////////////////////////////////

    /*Processes generic actions and returns whether target was killed with callback.*/
    IEnumerator processGenericAction(Action action, Unit myself, Unit target, BattleHud targetHud,
        System.Action<bool> callbackOnFinish){
        
        bool isDead = false;
        if(action == Action.ATTACK){
                dialogueText.UpdateText(myself.unitName+" attacks!");
                yield return new WaitForSeconds(1f);
                isDead = target.TakeDamage(myself.damage);
                targetHud.SetHP(target.currentHP);
            }else if (action == Action.DEFEND){
                dialogueText.UpdateText(myself.unitName+" defends!");
                //yield return new WaitForSeconds(1f);
            }else if (action == Action.TAUNT){
                dialogueText.UpdateText(myself.unitName+" taunts!");
                //yield return new WaitForSeconds(1f);
            }else if (action == Action.IDLE){
                dialogueText.UpdateText(myself.unitName+" does absolutely nothing!");
                //yield return new WaitForSeconds(1f);
            }//else SKIP
        callbackOnFinish(isDead);   //Calls a function so as to return this result.
    }

    IEnumerator PlayerAttackAction(){
        bool isDead = enemies[0].TakeDamage(assistantUnit.damage);
        enemyHUDs[0].SetHP(enemies[0].currentHP);
        dialogueText.UpdateText(enemies[0].unitName+" has just taken "+assistantUnit.damage+" damage!");
        //Wait
        yield return new WaitForSeconds(2f);
        
        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }else{
            changeState();
        }      
    }

    //TODO: Check if waiting truly makes sense in the following functions.

    IEnumerator PlayerHideAction()
    {
        dialogueText.forceDialogueAdvance("You Hide!");
        //Wait
        yield return new WaitForSeconds(0f);
        changeState();
    }

    IEnumerator PlayerPushAction(Unit target)
    {
        dialogueText.forceDialogueAdvance("You pushed "+target.name+".");
        //Wait
        yield return new WaitForSeconds(0f);
        currentAction = Action.SKIP;
        changeState();
    }

    IEnumerator PlayerStopAction()
    {
        dialogueText.forceDialogueAdvance("You Stop!");
        //Wait
        yield return new WaitForSeconds(0f);
        changeState();
    }

    IEnumerator PlayerUseItemAction()
    {
        dialogueText.forceDialogueAdvance("You use a Item!");
        //Wait
        yield return new WaitForSeconds(0f);
        changeState();
    }

//////////////////////////////////////////////BUTTON SELECTED//////////////////////////////////////////////

    public void OnAttackButton(){
        //Debug.Log("Button clicked!");
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerAttackAction());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
    }

    public void OnHideButton(){
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerHideAction());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
    }

    public void OnPushButton(){
        if(state == BattleState.PLAYERTURN){
            //Sets flag and waits for target selection.
            currentAction = Action.PUSH;
            dialogueText.forceDialogueAdvance("Select your target.");
        }
    }

    public void OnStopButton(){
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerStopAction());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
    }

    public void OnItemButton(){
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerUseItemAction());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
    }

//////////////////////////////////////////////AUXILARY FUNCTIONS//////////////////////////////////////////////

    public selectedObjectArrowController selectArrowController;

    public void objectSelected(GameObject gameObject){
        //Clickable objects can self determine this.
        /*if(!isInputAllowed()){            
            Debug.Log("Input disabled whilst typing");
            return; //Input disabled whilst typing.
        }*/
        if(currentAction == Action.PUSH){
            //Picks up current object position and sends it to Arrow.
            selectArrowController.SetArrow(gameObject.transform.position);

            Debug.Log(gameObject.ToString()+" pushed!");
            StartCoroutine(PlayerPushAction(gameObject.GetComponent<Unit>()));

        }
    }

    /*Auxilary function to be called at the end of turns.*/
    private void changeState(){
        switch (state){
            case BattleState.PLAYERTURN:
                state = BattleState.HEROTURN;
                StartCoroutine(HeroTurn());
                break;
            case BattleState.HEROTURN:
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMYTURN:
                dialogueText.UpdateText(battleScript.getTurnOutcome()); 
                goto default;
            default:    //Not a normal state, start player turn.                
                battleScript.runTurn();
                dialogueText.UpdateText(battleScript.getTurnIntent());
                state = BattleState.PLAYERTURN;
                PlayerTurn();
                break;
        }
        //Could check health of all units and decide if game end that way.
    }

    public bool isInputAllowed(){
        return dialogueText.isIdle() && state == BattleState.PLAYERTURN;
    }

    public Action getCurrentAction(){
        return currentAction;
    }
}
