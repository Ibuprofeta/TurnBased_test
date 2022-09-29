using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Act(){
        int dieRoll = Random.Range(0,3);
        Character target = BattleController.Instance.GetRandomPlayer();
        switch(dieRoll){
            case 0:
                Defend();
                break;
            case 1:
                Spell spellToCast = GetRandomSpell();
                if(spellToCast.spellType == Spell.SpellType.Heal){
                    target = BattleController.Instance.GetWeakestEnemy();
                } 
                if (!CastSpell(spellToCast, target)){
                    BattleController.Instance.DoAttack(this, target);
                }
                break;
            case 2:
                BattleController.Instance.DoAttack(this, target);
                break;
        }
    }

    Spell GetRandomSpell(){
        return spells[Random.Range(0, spells.Count - 1)];
    }

    public override void Die(){
        base.Die();
        BattleController.Instance.characters[1].Remove(this);
    }
}
