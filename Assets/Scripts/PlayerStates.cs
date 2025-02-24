using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    
    private StatesOfPlayer _playerState;
    // singleton that allows us to get and set the player states 
    void Start()
    {
        _playerState = StatesOfPlayer.Idle;
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
