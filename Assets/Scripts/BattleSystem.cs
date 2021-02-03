using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum UnitType { HERO, ASSISTANT, BIGBLOB }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    /*public Transform heroLocation;
    public Transform assistantLocation;
    public Transform enemyLocation1;
    public Transform enemyLocation2;
    public Transform enemyLocation3;
    public Transform enemyLocation4;*/

    /*public GameObject heroPrefab;
    public GameObject assistantObject;
    public GameObject enemyObject1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;*/

    public BattleHud heroHud;
    public BattleHud assistantHud;
    public BattleHud enemyHud1;
    public BattleHud enemyHud2;
    public BattleHud enemyHud3;
    public BattleHud enemyHud4;    

    Unit heroUnit;
    public Unit assistantUnit;
    public Unit enemyUnit1;
    Unit enemyUnit2;
    Unit enemyUnit3;
    Unit enemyUnit4;

    public TextMeshProUGUI dialogueText;
    //Game level. Tutorial is 0.
    int level = 0;
    //Game turn.
    int turn = 0;

    // Start is called before the first frame update
    void Start(){
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }
    //IEnumerator to turn setup into Coroutine and make it wait.
    IEnumerator SetupBattle(){      
        //TODO: Figure out how to receive this array externally or set up scenes so as to not need them.
        //First two setup allies, the rest are enemies.
        UnitType[] unitTypes = {UnitType.HERO, UnitType.ASSISTANT, UnitType.BIGBLOB};

        //Initialise assistant object.
        //GameObject assistantObject = Instantiate(assistantPrefab, assistantLocation);
        //assistantUnit = assistantObject.GetComponent<Unit>();
        //assistantHud.SetHUD(assistantUnit);

        //TODO: Initialise hero object.

        //Initialise enemy objects
        //GameObject enemyObject1 = Instantiate(enemyPrefab1, enemyLocation1);
        //enemyUnit1 = enemyObject1.GetComponent<Unit>();
        //enemyHud1.SetHUD(enemyUnit1);
        dialogueText.text = "A "+enemyUnit1.unitName+" politely approaches.";        
        //TODO: FOR each prefab != null initialise and add to list             
        
        //Wait 2 seconds.
        yield return new WaitForSeconds(2f);
        
        //Setup finished, change state
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn(){
        dialogueText.text = "Your time to act!";
    }

    IEnumerator EnemyTurn(){
        dialogueText.text = enemyUnit1.unitName+" attacks!";
        yield return new WaitForSeconds(1f);
        bool isDead = assistantUnit.TakeDamage(enemyUnit1.damage);
        //assistantHud.SetHP(assistantUnit.currentHP);
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
            dialogueText.text = "You have survived yet another ordeal!";
        }else if (state == BattleState.LOST){
            dialogueText.text = "The situation has developed not necessarily to your advantage.";
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

    IEnumerator PlayerAttack(){
        bool isDead = enemyUnit1.TakeDamage(assistantUnit.damage);
        //enemyHud1.SetHP(enemyUnit1.currentHP);
        dialogueText.text = enemyUnit1.unitName+" has just taken "+assistantUnit.damage+" damage!";
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
}
