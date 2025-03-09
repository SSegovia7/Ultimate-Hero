using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    public static Locator Instance { get ; private set; }
    public PosingController PosingControll { get ; private set ; }
    public PlayerStates StatesOfPlayer { get ; private set; }
    
    public CombatTester CombatControl { get ; private set ; }
    public AbilityController AbilityControl { get ; private set ; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }    
        Instance = this;
        


        GameObject playerGameObject = GameObject.Find("Player Character");
        PosingControll = playerGameObject.GetComponent<PosingController>();
        StatesOfPlayer = playerGameObject.GetComponent<PlayerStates>();
        CombatControl = playerGameObject.GetComponent<CombatTester>();
        AbilityControl = playerGameObject.GetComponent<AbilityController>();
    }
}
