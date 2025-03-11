using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
//using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;
using TMPro;

public class PosingController : MonoBehaviour
{
    public delegate bool PlayerUsedAbility(List<KeyCode> listOfInputs);
    public event PlayerUsedAbility AbilityUsed;
    private List<KeyCode> _abilityCombo;
    // this crap will need to be replaced with actual animator later, too busy
    private SpriteRenderer _playerSprite;
    // not this below
    private Rigidbody2D _playerRigidbody;
    private bool _abilityInUse;
    [SerializeField] private Animator animator;


    private UnityEngine.Vector2 _zeroVector;
    [SerializeField] private float _injuredTimer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Rigidbody2D _playerOtherRigidbody;
    [SerializeField] private CharacterMovement _playerMovement;


    void Start()
    {
        _abilityCombo = new List<KeyCode>();
        _playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        _playerRigidbody = this.GetComponent<Rigidbody2D>();
        _zeroVector = UnityEngine.Vector2.zero;
        _abilityInUse = false;
        Locator.Instance.StatesOfPlayer.GetSetPlayerState = PlayerStates.StatesOfPlayer.Idle;

    }
    // Update is called once per frame
    void Update()
    {
        if(_abilityInUse)
        {
            Invoke("PlayerStopPosing", 2f);
            return;
        }

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
        

        if(Locator.Instance.StatesOfPlayer.GetSetPlayerState == PlayerStates.StatesOfPlayer.Idle)
        {
            PlayerStopPosing();
        }
        
        
        // player press E to hold pose
        if(Input.GetKeyDown(KeyCode.E) && Locator.Instance.StatesOfPlayer.GetSetPlayerState != PlayerStates.StatesOfPlayer.Posing)
        {
            _playerSprite.sprite = _sprites[5];
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
        }


        if(Input.GetKeyUp(KeyCode.E))
        {
            if(AbilityUsed(_abilityCombo))
            {
                _abilityInUse = true;
            }
            else
            {
                PlayerStopPosing();
                _playerMovement.enabled = true;
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
        _abilityInUse = false;
        // reset list after to not have old combination data
        _abilityCombo = new List<KeyCode>();
        _playerSprite.sprite = _sprites[0];
        return;
    }
}
