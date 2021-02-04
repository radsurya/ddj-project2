using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Generalise for multiple scripts later.
public class BattleScript{
    private bool heroStuck = false;
    public string getHeroIntent(){
        return "The Hero prepares to slash the Big Blob.";
    }
    public void getHeroAction(){
        heroStuck = true;
    }

    public void getEnemyAction(){
        
    }
}
