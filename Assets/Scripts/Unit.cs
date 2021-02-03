using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public string unitName;

	public int damage;

	public int maxHP;
	public int currentHP;

	public UnitBehaviour unitBehaviour;

	public void SetUnitType(UnitType t){
		//Depending on UnitType create correct behaviour.
	}

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

/*public class Blob : Unit
{

}*/

//Common class to group behaviours.
public abstract class UnitBehaviour{
	public abstract void DoAction();
}

//Specific class for Hero
public class Hero : UnitBehaviour
{
	override public void DoAction(){

	}
}

//Specific class for BigBlob
public class BigBlob : UnitBehaviour
{
	override public void DoAction(){

	}
}
