using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;

public class PosingController : MonoBehaviour
{
    public delegate bool PlayerUsedAbility(List<KeyCode> listOfInputs);
    public event PlayerUsedAbility AbilityUsed;
    private List<KeyCode> _abilityCombo;
    private CharacterMovement _playerMovement;
    // this crap will need to be replaced with actual animator later, too busy
    private SpriteRenderer _playerSprite;
    // not this below
    private Rigidbody2D _playerRigidbody;
    
    private UnityEngine.Vector2 _zeroVector;
    [SerializeField] private float _injuredTimer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Rigidbody2D _playerOtherRigidbody;

    void Start()
    {
        _abilityCombo = new List<KeyCode>();
        _playerMovement = this.GetComponent<CharacterMovement>();
        _playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        _playerRigidbody = this.GetComponent<Rigidbody2D>();
        _zeroVector = UnityEngine.Vector2.zero;
        Locator.Instance.StatesOfPlayer.GetSetPlayerState = PlayerStates.StatesOfPlayer.Idle;

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Locator.Instance.StatesOfPlayer.GetSetPlayerState);
        // needed so that player doesn't fall through shadow
        // if player jumps then presses E, they will fall through the shadow base
        if(Locator.Instance.StatesOfPlayer.GetSetPlayerState == PlayerStates.StatesOfPlayer.Jumping)
        {
            // as long as the character is in the air, no code below will be called
            return;
        }

        // if player is injured 
        if(Locator.Instance.StatesOfPlayer.GetSetPlayerState == PlayerStates.StatesOfPlayer.Injured)
        {
            // knock them out of pose for x amount of time
            PlayerKnockedOutOfPosing();
        }
        

        // player press E to hold pose
        if(Input.GetKeyDown(KeyCode.E))
        {
            PlayerIsPosing();
        }
        // as long as the character is in their posing stances, this code can be reached
        if(Locator.Instance.StatesOfPlayer.GetSetPlayerState == PlayerStates.StatesOfPlayer.Posing)
        {
            // get all inputs of the direction arrows
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                // store button press in list
                _abilityCombo.Add(KeyCode.RightArrow);
                // change pose to right pose
                _playerSprite.sprite = _sprites[1];
            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                // store button press in list
                _abilityCombo.Add(KeyCode.UpArrow);
                // change pose to up pose
                _playerSprite.sprite = _sprites[2];
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // store button press in list
                _abilityCombo.Add(KeyCode.LeftArrow);
                // change pose to left pose
                _playerSprite.sprite = _sprites[3];
            }
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                // store button press in list
                _abilityCombo.Add(KeyCode.DownArrow);
                // change pos to down pose
                _playerSprite.sprite = _sprites[4];
            }
    

            if(Input.GetKeyUp(KeyCode.E))
            {
                //AbilityUsed?.Invoke(_abilityCombo);
                if(AbilityUsed(_abilityCombo))
                {
                    Invoke("PlayerStopPosing", 1);
                }
                else
                {
                    PlayerStopPosing();
                }
            }
        }
    }
    


    private void PlayerIsPosing()
    {
        Locator.Instance.StatesOfPlayer.GetSetPlayerState = PlayerStates.StatesOfPlayer.Posing;
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
        Locator.Instance.StatesOfPlayer.GetSetPlayerState = PlayerStates.StatesOfPlayer.Idle;
        _playerMovement.enabled = true;
        // reset list after to not have old combination data
        _abilityCombo = new List<KeyCode>();
        _playerSprite.sprite = _sprites[0];
        return;
    }
}
