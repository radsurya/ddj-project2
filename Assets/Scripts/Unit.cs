using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public string unitName;

    public int damage;
	public int maxHP;
	public int currentHP;

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
