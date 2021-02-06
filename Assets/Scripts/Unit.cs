using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : ClickableObject
{
	public string walkInString;
	//TODO: Implement these in Battle system, and take into account assistant does not use them.
	public string attackString;
	public string defendString;
	public string idleString;
    
	public int damage;
	public int maxHP;
	public int currentHP;

	public BattleHud battleHud;

	public bool TakeDamage(int damage){
		currentHP -= damage;
		Debug.Log("Health is at "+currentHP);
		if(currentHP <= 0){
			Debug.Log("Is dead");
			return true;
		}else{
			return false;
		}		
	}

	public abstract void decideAction(List<Unit> targets);
}
