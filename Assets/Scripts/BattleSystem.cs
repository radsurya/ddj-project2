using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, HEROTURN, ENEMYTURN, WON, LOST, PUSHSELECT }

public enum Action { ATTACK, DEFEND, TAUNT }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

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
        
        //Setup finished, change state TODO: Maybe create nextTurn() function
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

//////////////////////////////////////////////TURN FUNCTIONS//////////////////////////////////////////////

    void PlayerTurn(){
        battleScript.runTurn();
        dialogueText.UpdateText(battleScript.getTurnIntent());
        dialogueText.UpdateText("Your time to act!");
        //The game does nothing during our turn, moving on only through inputs.
    }

    IEnumerator HeroTurn(){
        Action action = battleScript.getHeroAction();
        bool isDead = false;
        if(action == Action.ATTACK){
            dialogueText.UpdateText(heroUnit.unitName+" attacks!");
            yield return new WaitForSeconds(1f);
            isDead = enemies[0].TakeDamage(heroUnit.damage);
            enemyHUDs[0].SetHP(enemies[0].currentHP);
        }else if (action == Action.DEFEND){
            dialogueText.UpdateText(heroUnit.unitName+" defends!");
            yield return new WaitForSeconds(1f);
        }else if (action == Action.TAUNT){
            dialogueText.UpdateText(heroUnit.unitName+" taunts!");
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }else{
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }        
    }

    IEnumerator EnemyTurn(){
        List<Action> actions = battleScript.getEnemyActions();
        bool isDead = false;
        for (int i = 0; i < enemies.Length; i++){
            Action action = actions[i];
            if(action == Action.ATTACK){
                dialogueText.UpdateText(enemies[i].unitName+" attacks!");
                yield return new WaitForSeconds(1f);
                isDead = assistantUnit.TakeDamage(enemies[0].damage);
                assistantHud.SetHP(assistantUnit.currentHP);
            }else if (action == Action.DEFEND){
                dialogueText.UpdateText(enemies[i].unitName+" defends!");
                yield return new WaitForSeconds(1f);
            }else if (action == Action.TAUNT){
                dialogueText.UpdateText(enemies[i].unitName+" taunts!");
                yield return new WaitForSeconds(1f);
            }
            if(isDead){
                state = BattleState.LOST;
                EndBattle();
            }
        }
        if(!isDead){
            //End of turn.
            dialogueText.UpdateText(battleScript.getTurnOutcome());   
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
         
    }

    void EndBattle(){
        //We could check state for safety
        if(state == BattleState.WON){
            dialogueText.UpdateText("You have survived yet another ordeal!");
        }else if (state == BattleState.LOST){
            dialogueText.UpdateText("The situation has developed not necessarily to your advantage.");
        }
    }

//////////////////////////////////////////////PROCESS ACTIONS//////////////////////////////////////////////

    IEnumerator PlayerAttackAction(){
        bool isDead = enemies[0].TakeDamage(assistantUnit.damage);
        enemyHUDs[0].SetHP(enemies[0].currentHP);
        dialogueText.UpdateText(new List<string>(1){enemies[0].unitName+" has just taken "+assistantUnit.damage+" damage!"});
        //Wait
        yield return new WaitForSeconds(2f);
        
        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }else{
            state = BattleState.HEROTURN;
            StartCoroutine(HeroTurn());
        }      
    }


    IEnumerator PlayerHideAction()
    {
        dialogueText.UpdateText(new List<string>(1) { " You Hide!" });
        //Wait
        yield return new WaitForSeconds(2f);
    }

    IEnumerator PlayerPushAction()
    {
        dialogueText.UpdateText(new List<string>(1) { " You Push!" });
        //Wait
        yield return new WaitForSeconds(2f);
    }

    IEnumerator PlayerStopAction()
    {
        dialogueText.UpdateText(new List<string>(1) { " You Stop!" });
        //Wait
        yield return new WaitForSeconds(2f);
    }

    IEnumerator PlayerUseItemAction()
    {
        dialogueText.UpdateText(new List<string>(1) { " You use a Item!" });
        //Wait
        yield return new WaitForSeconds(2f);
    }

    //////////////////////////////////////////////BUTTON SELECTED//////////////////////////////////////////////

    public void OnAttackButton(){
        Debug.Log("Button clicked!");
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
            //Select target
            state = BattleState.PUSHSELECT;
            //StartCoroutine(PlayerAttack());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
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

    public selectedObjectArrowController selectArrowController;

    public void objectSelected(GameObject gameObject){
        if(state == BattleState.PUSHSELECT){

            Vector3 objectPos = gameObject.transform.position;
            selectArrowController.SetArrow(objectPos);
            Debug.Log(gameObject.ToString()+" selected!");
        }
    }
}
