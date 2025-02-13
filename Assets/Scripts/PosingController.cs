using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PosingController : MonoBehaviour
{
    public delegate bool CheckCombo(List<int> listOfInputs);
    public event CheckCombo CheckPlayerPoseInput;
    private List<int> _abilityCombo;
    private CharacterMovement _playerMovement;
    // this crap will need to be replaced with actual animator later, too busy
    private SpriteRenderer _playerSprite;
    // not this below
    private Rigidbody2D _playerRigidbody;
    private Rigidbody2D _playerOtherRigidbody;
    private UnityEngine.Vector3 _zeroVector;
    [SerializeField] private float _injuredTimer;
    [SerializeField] private Sprite[] _sprites;

    void Start()
    {
        _abilityCombo = new List<int>();
        _playerMovement = this.GetComponent<CharacterMovement>();
        _playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        _playerOtherRigidbody = this.GetComponentInChildren<Rigidbody2D>();
        _playerRigidbody = this.GetComponent<Rigidbody2D>();
        _zeroVector = UnityEngine.Vector3.zero;

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerStates.StateInstance.GetSetPlayerState);
        // needed so that player doesn't fall through shadow
        // if player jumps then presses E, they will fall through the shadow base
        if(PlayerStates.StateInstance.GetSetPlayerState == PlayerStates.StatesOfPlayer.Jumping)
        {
            // as long as the character is in the air, no code below will be called
            return;
        }

        // if player is injured 
        if(PlayerStates.StateInstance.GetSetPlayerState == PlayerStates.StatesOfPlayer.Injured)
        {
            // knock them out of pose for x amount of time
            PlayerKnockedOutOfPosing();
        }
        

        // player press E to hold pose
        if(Input.GetKeyDown(KeyCode.E))
        {
            PlayerIsPosing();
            Debug.Log("hi");
            CheckPlayerPoseInput?.Invoke(_abilityCombo);
        }
        // as long as the character is in their posing stances, this code can be reached
        if(PlayerStates.StateInstance.GetSetPlayerState == PlayerStates.StatesOfPlayer.Posing)
        {
            // get all inputs of the direction arrows
            if(Input.GetKey(KeyCode.RightArrow))
            {
                // store button press in list
                _abilityCombo.Add((int)KeyCode.RightArrow);
                // change pose to right pose
                _playerSprite.sprite = _sprites[1];
            }
            if(Input.GetKey(KeyCode.UpArrow))
            {
                // store button press in list
                _abilityCombo.Add((int)KeyCode.UpArrow);
                // change pose to up pose
                _playerSprite.sprite = _sprites[2];
            }
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                // store button press in list
                _abilityCombo.Add((int)KeyCode.LeftArrow);
                // change pose to left pose
                _playerSprite.sprite = _sprites[3];
            }
            if(Input.GetKey(KeyCode.DownArrow))
            {
                // store button press in list
                _abilityCombo.Add((int)KeyCode.DownArrow);
                // change pos to down pose
                _playerSprite.sprite = _sprites[4];
            }
            

            if(Input.GetKey(KeyCode.J))
            {
                PlayerStates.StateInstance.GetSetPlayerState = PlayerStates.StatesOfPlayer.Injured;
            }


            if(Input.GetKeyUp(KeyCode.E))
            {
                PlayerStopPosing();
            }
        }
    }
    

    private void PlayerIsPosing()
    {
        PlayerStates.StateInstance.GetSetPlayerState = PlayerStates.StatesOfPlayer.Posing;
        _playerRigidbody.velocity = _zeroVector;
        _playerOtherRigidbody.velocity = _zeroVector;
        _playerMovement.enabled = false;
        return;
    }


    private void PlayerKnockedOutOfPosing()
    {
        _injuredTimer -= Time.deltaTime;
        if(_injuredTimer <= 0)
        {
            PlayerStopPosing();
            _injuredTimer = 3f;
            return;
        }
        
    }


    private void PlayerStopPosing()
    {
        PlayerStates.StateInstance.GetSetPlayerState = PlayerStates.StatesOfPlayer.Moving;
        _playerMovement.enabled = true;
        // reset list after to not have old combination data
        _abilityCombo = new List<int>();
        _playerSprite.sprite = _sprites[0];
        return;
    }
}
