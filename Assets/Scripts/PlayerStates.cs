using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public static PlayerStates StateInstance {get; private set;}
    private StatesOfPlayer _playerState;
    // singleton that allows us to get and set the player states 
    void Awake()
    {
        if(StateInstance != null && StateInstance != this)
        {
            Destroy(this);
            return;
        }
        StateInstance = this;
    }
    public enum StatesOfPlayer {
        Moving, Attacking, Jumping, Posing, Injured, Idle
    }
    
    public StatesOfPlayer GetSetPlayerState
    {
        get { return _playerState; }
        set { _playerState = value; }
    } 
}
