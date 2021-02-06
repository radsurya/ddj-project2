using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, HEROTURN, ENEMYTURN, WON, LOST }

public enum Action { ATTACK, DEFEND, TAUNT, IDLE, SKIP, HIDE, STOP, PUSH, ITEM }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    private Action currentAction = Action.SKIP;

    /*public BattleHud heroHud;
    public BattleHud assistantHud;
    public BattleHud[] enemyHUDs = new BattleHud[4];*/

    public Unit heroUnit;
    public Unit assistantUnit;
    private Vector3 assistantPosition;
    private SpriteRenderer assistantSpriteRenderer;
    public Unit[] enemies = new Unit[4];    

    public DialogueBox dialogueText;

    private BattleScript battleScript = new BattleScript();
    private Dictionary<string,Unit> unitNameDict = new Dictionary<string, Unit>();  
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

        //Initialise hero object.
        addUnit(heroUnit);
        Debug.Log("Hero successful.");
        //Initialise assistant object.
        addUnit(assistantUnit); 
        assistantPosition = assistantUnit.transform.position;
        assistantSpriteRenderer = assistantUnit.GetComponent<SpriteRenderer>();
        //Initialise enemy objects
        List<Unit> livingEnemies = new List<Unit>();
        for (int i = 0; i < enemies.Length && enemies[i] != null; i++){
            livingEnemies.Add(enemies[i]);
            addUnit(enemies[i]);
            yield return new WaitForSeconds(2f);    //Wait time should match enemy entrance text.
        }
        enemies = livingEnemies.ToArray();     
        
        //Setup finished, change state
        changeState();
    }

    private void addUnit(Unit unit){
        unitNameDict.Add(unit.unitName, unit);
        unit.battleHud.SetHUD(unit);
        dialogueText.UpdateText(unit.walkInString);
    }

//////////////////////////////////////////////TURN FUNCTIONS//////////////////////////////////////////////

    IEnumerator PlayerTurn(){
        //Wait a reasonable time for message display:
        yield return new WaitForSeconds(dialogueText.numberOfMessages()/2);
        state = BattleState.PLAYERTURN;
        //Reset assistant position.
        assistantUnit.transform.position = assistantPosition;
        assistantSpriteRenderer.sortingOrder = 0;
        
        dialogueText.UpdateText("Your time to act!");
        //The game does nothing during our turn, moving on only through inputs.
    }

    IEnumerator HeroTurn(){
        state = BattleState.HEROTURN;
        bool isDead = false;
        yield return StartCoroutine(processGenericAction(battleScript.getNextAction(), heroUnit, 
             boolean => isDead = boolean));
        //yield return new WaitForSeconds(1f);
        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }else{
            changeState();
        }        
    }

    IEnumerator EnemyTurn(){
        state = BattleState.ENEMYTURN;
        bool isDead = false;
        foreach (Unit enemy in enemies){
            yield return StartCoroutine(processGenericAction(battleScript.getNextAction(), enemy, 
                boolean => isDead = boolean));
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
            dialogueText.UpdateText("Victory!");
        }
        else if (state == BattleState.LOST){
            dialogueText.UpdateText("The situation has developed not necessarily to your advantage.");
            dialogueText.UpdateText("GAME OVER!");
            //dialogueText.semaphoreObject.WaitOne();
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit();
        }
    }

//////////////////////////////////////////////PROCESS ACTIONS//////////////////////////////////////////////

    /*Processes generic actions and returns whether target was killed with callback.*/
    IEnumerator processGenericAction(Action action, Unit myself,
        System.Action<bool> callbackOnFinish){
        
        bool isDead = false;
        if(action == Action.ATTACK){
            Unit target = unitNameDict[battleScript.getNextTarget()];
            dialogueText.UpdateText(myself.unitName+" attacks!");
            yield return new WaitForSeconds(1f);
            isDead = target.TakeDamage(myself.damage);
            target.battleHud.SetHP(target.currentHP);
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

    /*IEnumerator PlayerAttackAction(){
        bool isDead = enemies[0].TakeDamage(assistantUnit.damage);
        //enemyHUDs[0].SetHP(enemies[0].currentHP);
        dialogueText.UpdateText(enemies[0].unitName+" has just taken "+assistantUnit.damage+" damage!");
        //Wait
        yield return new WaitForSeconds(2f);
        
        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }else{
            changeState();
        }      
    }*/

    //TODO: Check if waiting truly makes sense in the following functions.

    IEnumerator PlayerHideAction(ClickableObject target)
    {
        dialogueText.forceDialogueAdvance(target.hideString);
        battleScript.runTurn(currentAction,target.unitName);

        assistantUnit.transform.position = target.transform.position;
        assistantSpriteRenderer.sortingOrder -= 5; 

        //Wait
        yield return new WaitForSeconds(0f);
        if (battleScript.end == 1)
        {
            state = BattleState.WON;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else if (battleScript.end == 2)
        {
            state = BattleState.LOST;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else
        {
            changeState();
        }
    }

    IEnumerator PlayerPushAction(ClickableObject target)
    {
        dialogueText.forceDialogueAdvance(target.pushString);
        battleScript.runTurn(currentAction,target.unitName);
        //Wait
        yield return new WaitForSeconds(0f);
        if (battleScript.end == 1)
        {
            state = BattleState.WON;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else if (battleScript.end == 2)
        {
            state = BattleState.LOST;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else
        {
            changeState();
        }
    }

    IEnumerator PlayerStopAction(ClickableObject target)
    {
        dialogueText.forceDialogueAdvance(target.stopString);
        battleScript.runTurn(currentAction,target.unitName);
        //Wait
        yield return new WaitForSeconds(0f);
        if (battleScript.end == 1)
        {
            state = BattleState.WON;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else if (battleScript.end == 2)
        {
            state = BattleState.LOST;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else
        {
            changeState();
        }
    }

    IEnumerator PlayerUseItemAction(ClickableObject target)
    {
        dialogueText.forceDialogueAdvance(target.itemString);
        battleScript.runTurn(currentAction,target.unitName);
        //Wait
        yield return new WaitForSeconds(0f);
        if (battleScript.end == 1)
        {
            state = BattleState.WON;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else if (battleScript.end == 2)
        {
            state = BattleState.LOST;
            dialogueText.UpdateText(battleScript.getTurnOutcome());
            EndBattle();
        }
        else
        {
            changeState();
        }
    }

//////////////////////////////////////////////BUTTON SELECTED//////////////////////////////////////////////

    //Deprecated: This button does not exist, and this action is not necessary.
    /*public void OnAttackButton(){
        //Debug.Log("Button clicked!");
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerAttackAction());

        }else if(state == BattleState.ENEMYTURN){

        }else if(state == BattleState.HEROTURN){

        }
    }*/
    //TODO: The if's may be unecessary, but are better left in, for safety.
    public void OnHideButton(){
        if(state == BattleState.PLAYERTURN){
            //Sets flag and waits for target selection.
            currentAction = Action.HIDE;
            dialogueText.forceDialogueAdvance("Select something to hide behind.");

        }
    }

    public void OnPushButton(){
        if(state == BattleState.PLAYERTURN){
            //Sets flag and waits for target selection.
            currentAction = Action.PUSH;
            dialogueText.forceDialogueAdvance("Select something to push.");
        }
    }

    public void OnStopButton(){
        if(state == BattleState.PLAYERTURN){
            //Sets flag and waits for target selection.
            currentAction = Action.STOP;
            dialogueText.forceDialogueAdvance("Select something to stop.");
        }
    }

    public void OnItemButton(){
        if(state == BattleState.PLAYERTURN){
            //Sets flag and waits for target selection.
            currentAction = Action.ITEM;
            dialogueText.forceDialogueAdvance("Select something to use an item on.");
            //TODO: Potentially implement an item select, though I expect that will never be done before the 8th
        }
    }

//////////////////////////////////////////////AUXILARY FUNCTIONS//////////////////////////////////////////////

    public selectedObjectArrowController selectArrowController;

    public void objectSelected(ClickableObject clickedObject){
        //Clickable objects can self determine this.
        /*if(!isInputAllowed()){            
            Debug.Log("Input disabled whilst typing");
            return; //Input disabled whilst typing.
        }*/
        if(currentAction == Action.PUSH){
            Debug.Log(gameObject.ToString()+" selected to push!");
            StartCoroutine(PlayerPushAction(clickedObject));
        } else if(currentAction == Action.HIDE){
            Debug.Log(gameObject.ToString() + " selected to hide!");
            StartCoroutine(PlayerHideAction(clickedObject));
        } else if(currentAction == Action.STOP){
            Debug.Log(gameObject.ToString() + " selected to stop!");
            StartCoroutine(PlayerStopAction(clickedObject));
        } else if(currentAction == Action.ITEM){
            Debug.Log(gameObject.ToString() + " selected to use item!");
            StartCoroutine(PlayerUseItemAction(clickedObject));
        }
    }

    /*Auxilary function to be called at the end of turns.*/
    private void changeState(){
        switch (state){
            case BattleState.PLAYERTURN:
                currentAction = Action.SKIP;    //Reset current action.
                StartCoroutine(HeroTurn());
                break;
            case BattleState.HEROTURN:
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMYTURN:
                dialogueText.UpdateText(battleScript.getTurnOutcome()); 
                goto default;
            default:    //Not a normal state, start player turn.                
                battleScript.runTurn();
                dialogueText.UpdateText(battleScript.getTurnIntent());
                StartCoroutine(PlayerTurn());
                break;
        }
        //Could check health of all units and decide if game end that way.
    }

    /*Whether input is allowed.*/
    public bool isInputAllowed(){
        return dialogueText.isIdle() && state == BattleState.PLAYERTURN;
    }

    /*Whether mouse selection is allowed.*/
    public bool isSelectAllowed(){
        return currentAction == Action.HIDE || currentAction == Action.STOP 
        || currentAction == Action.PUSH || currentAction == Action.ITEM;
    }
}
