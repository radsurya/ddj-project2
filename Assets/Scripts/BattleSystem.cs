using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, PUSHSELECT, ENEMYTURN, WON, LOST }

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
        //enemies = {enemyUnit1, enemyUnit2, enemyUnit3, enemyUnit4};
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
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

//////////////////////////////////////////////TURN FUNCTIONS//////////////////////////////////////////////

    void PlayerTurn(){
        //TODO: Get intent from each enemy.
        dialogueText.UpdateText(battleScript.getHeroIntent());
        dialogueText.UpdateText("Your time to act!");
        //The game does nothing during our turn, moving on only through inputs.
    }

    IEnumerator HeroTurn(){
        //heroUnit.decideAction();
        dialogueText.UpdateText(enemies[0].unitName+" attacks!");
        yield return new WaitForSeconds(1f);
        bool isDead = assistantUnit.TakeDamage(enemies[0].damage);
        assistantHud.SetHP(assistantUnit.currentHP);
        yield return new WaitForSeconds(1f);
        if(isDead){
            state = BattleState.LOST;
            EndBattle();
        }else{
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }        
    }

    IEnumerator EnemyTurn(){
        dialogueText.UpdateText(enemies[0].unitName+" attacks!");
        yield return new WaitForSeconds(1f);
        bool isDead = assistantUnit.TakeDamage(enemies[0].damage);
        assistantHud.SetHP(assistantUnit.currentHP);
        yield return new WaitForSeconds(1f);
        if(isDead){
            state = BattleState.LOST;
            EndBattle();
        }else{
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

    IEnumerator PlayerAttack(){
        bool isDead = enemies[0].TakeDamage(assistantUnit.damage);
        enemyHUDs[0].SetHP(enemies[0].currentHP);
        dialogueText.UpdateText(enemies[0].unitName+" has just taken "+assistantUnit.damage+" damage!");
        //Wait
        yield return new WaitForSeconds(2f);
        
        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }else{
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }      
    }

//////////////////////////////////////////////BUTTON SELECTED//////////////////////////////////////////////

    public void OnAttackButton(){
        Debug.Log("Button clicked!");
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerAttack());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
    }

    public void OnHideButton(){
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerAttack());

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
            StartCoroutine(PlayerAttack());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
    }

    public void OnItemButton(){
        if(state == BattleState.PLAYERTURN){
            //Select target
            StartCoroutine(PlayerAttack());

        }else if(state == BattleState.ENEMYTURN){

        }/*else if(state == BattleState.HEROTURN){

        }*/
    }

    public void objectSelected(GameObject gameObject){
        if(state == BattleState.PUSHSELECT){
            Debug.Log(gameObject.ToString()+" selected!");
        }
    }
}
