using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance {get; set;}

    public Dictionary<int, List<Character>> characters = 
    new Dictionary<int, List<Character>>();
    public int characterTurnIndex;
    public Spell playerSelectedSpell;
    public bool playerIsAttacking;

    [SerializeField] private BattleSpawnPoint[] spawnPoints;

    private int actTurn;

    [SerializeField] private BattleUIController uiController;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }

        characters.Add(0, new List<Character>());
        characters.Add(1, new List<Character>());
        FindObjectOfType<BattleLauncher>().Launch();

        FindObjectOfType<BattleUIController>().UpdateCharacterUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NextTurn(){
        actTurn = actTurn == 0 ? 1 : 0;
    }

    public Character GetRandomPlayer(){
        return characters[0][Random.Range(0, characters[0].Count - 1)];
    }

    public Character GetWeakestEnemy(){
        Character weakestEnemy = characters[1][0];
        foreach(Character character in characters[1]){
            if (character.health < weakestEnemy.health){
                weakestEnemy = character;
            }
        }

        return weakestEnemy;
    }

    public void StartBattle(List<Character> players, List<Character> enemies){
        Debug.Log("Setup battle!");
        for (int i = 0; i < players.Count; i++){
            characters[0].Add(spawnPoints[i+3].Spawn(players[i]));
        }
        for (int i = 0; i < enemies.Count; i++){
            characters[1].Add(spawnPoints[i].Spawn(enemies[i]));
        }
    }

    private void NextAct(){
        if (characters[0].Count > 0 && characters[1].Count > 0){
            if (characterTurnIndex < characters[actTurn].Count - 1){
                characterTurnIndex++;
            } else{
                NextTurn();
                characterTurnIndex = 0;
                Debug.Log("turn: " + actTurn);
            }

            switch(actTurn){
                case 0:
                    uiController.ToogleActionState(true);
                    uiController.BuildSpellList(GetCurrentCharacter().spells);
                    break;
                case 1:
                    StartCoroutine(PerformAct());
                    uiController.ToogleActionState(false);
                    break;
            }
        } else {
            Debug.Log("Battle over");
        }
    }

    IEnumerator PerformAct(){
        yield return new WaitForSeconds(.75f);
        if (GetCurrentCharacter().health > 0){
            GetCurrentCharacter().GetComponent<Enemy>().Act();
        }
        uiController.UpdateCharacterUI();
        yield return new WaitForSeconds(1f);
        NextAct();
    }

    public void SelectCharacter (Character character){
        if (playerIsAttacking){
            DoAttack(GetCurrentCharacter(), character);
        } 
        else if (playerSelectedSpell != null)
        {
            if (GetCurrentCharacter().
            CastSpell(playerSelectedSpell, character))
            {
                uiController.UpdateCharacterUI();
                NextAct();
            } 
            else 
            {
                Debug.LogWarning("Not enough mana to cast that spell");
            }
        }
    }

    public Character GetCurrentCharacter()
    {
        return characters[actTurn][characterTurnIndex];
    }

    public void DoAttack(Character attacker, Character target){
        Debug.Log("do attack");
        target.Hurt(attacker.attackPower);
        if (actTurn == 0)
            NextAct();
    }

    public void NextActDefend (){
        if (actTurn == 0)
            NextAct();
    }
}
