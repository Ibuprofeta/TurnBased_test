using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die(){
        base.Die();
        BattleController.Instance.characters[0].Remove(this);
    }
    
}
