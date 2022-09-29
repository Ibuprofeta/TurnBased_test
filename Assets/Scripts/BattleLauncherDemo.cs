using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLauncherDemo : MonoBehaviour
{
    [SerializeField]
    private List<Character> players, enemies;
    [SerializeField]
    private BattleLauncher launcher;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Launch(){
        launcher.PrepareBattle(enemies, players);
    }
}
