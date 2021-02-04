using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, PUSHSELECT, ENEMYTURN, WON, LOST }

//public enum UnitType { HERO, ASSISTANT, BIGBLOB }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public BattleHud heroHud;
    public BattleHud assistantHud;
    public BattleHud enemyHud1;
    public BattleHud enemyHud2;
    public BattleHud enemyHud3;
    public BattleHud enemyHud4;  

    public Unit heroUnit;
    public Unit assistantUnit;
    public Unit enemyUnit1;
    public Unit enemyUnit2;
    public Unit enemyUnit3;
    public Unit enemyUnit4;

    public DialogueBox dialogueText;
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
        //TODO: Figure out how to receive this array externally or set up scenes so as to not need them.
        //First two setup allies, the rest are enemies.
        //UnitType[] unitTypes = {UnitType.HERO, UnitType.ASSISTANT, UnitType.BIGBLOB};

        //Initialise assistant object.
        assistantHud.SetHUD(assistantUnit);
        //Initialise hero object.
        heroHud.SetHUD(heroUnit);
        //Initialise enemy objects
        Unit[] enemies = {enemyUnit1, enemyUnit2, enemyUnit3, enemyUnit4};
        BattleHud[] enemyHuds = {enemyHud1, enemyHud2, enemyHud3, enemyHud4};
        for (int i = 0; i < enemies.Length && enemies[i] != null; i++){
            enemyHuds[i].SetHUD(enemies[i]);
            //TODO: Custom enemy walk in text
            dialogueText.UpdateText("A "+enemies[i].unitName+" politely approaches.");  
            yield return new WaitForSeconds(2f);    //Wait time should match enemy entrance text.
        }        
        
        //Setup finished, change state
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn(){
        dialogueText.UpdateText("Your time to act!");
    }

    IEnumerator EnemyTurn(){
        dialogueText.UpdateText(enemyUnit1.unitName+" attacks!");
        yield return new WaitForSeconds(1f);
        bool isDead = assistantUnit.TakeDamage(enemyUnit1.damage);
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

    IEnumerator PlayerAttack(){
        bool isDead = enemyUnit1.TakeDamage(assistantUnit.damage);
        enemyHud1.SetHP(enemyUnit1.currentHP);
        dialogueText.UpdateText(enemyUnit1.unitName+" has just taken "+assistantUnit.damage+" damage!");
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

    public void objectSelected(GameObject gameObject){
        if(state == BattleState.PUSHSELECT){
            Debug.Log(gameObject.ToString()+" selected!");
        }
    }
}
