using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AbilityController : MonoBehaviour
{
    // list of available abilities in the game
    [SerializeField] private List<Ability> _playerAbilitiesList;
    [SerializeField] private BoxCollider2D _playerSecondAbilityHitBox;
    [SerializeField] private BoxCollider2D _playerFirstAbilityHitBox;
    [SerializeField] private Rigidbody2D _playerRigidBody;
    // replace 
    [SerializeField] private GameObject _secondAbilitySprite;
    [SerializeField] private float _secondAbilityTimeOut;
    [SerializeField] private float _firstAbilityMagnitude;
    [SerializeField] private float _firstAbilityDistanceTimer;
    [SerializeField] private CharacterMovement _playerMovement;
    private Vector2 _firstAbilityForce;
    private List<string> _abilityNames;
    private Dictionary<string, List<KeyCode>> _playerAbilities;
    private string _abilityCalled;
    private bool _isValid;
    private bool _isLookingRight;
    private float timer;
    private IEnumerator coroutine;

   

    void Start()
    {
        // list to hold the ability names 
        _abilityNames = new List<string>();
        _firstAbilityForce = Vector2.left;
        // dictionary to hold the correct input combinations, can be acquired by their names
        _playerAbilities = new Dictionary<string, List<KeyCode>>();
        // basic intializer of string variable
        _abilityCalled = "";
        // tie method to event in posingcontroller
        Locator.Instance.PosingControll.AbilityUsed += HandlePlayerAbility;
        // store data from Ability list into dictionary and name list
        StoreAbilitiesData();
        timer = 0;
    }


    void Update()
    {
        timer += Time.deltaTime;
    }

    /// <summary>
    /// Calls the ability based on correct combination of key presses 
    /// </summary>
    /// <param name="playerInput"></param> the list of keycodes that the player has inputted 
    public bool HandlePlayerAbility(List<KeyCode> playerInput) 
    {
        if(IsValidCombo(playerInput))
        {
            //Debug.Log($"Player has called the {_abilityCalled} ability!");
            CallAbility();
            return true;
        }
        return false; 
    }

    public bool IsValidCombo(List<KeyCode> playerInput)
    {
        foreach(string abilityName in _abilityNames)
        {
            if(_playerAbilities.TryGetValue(abilityName, out List<KeyCode> abilityCombo))
            {
                if(playerInput.SequenceEqual(abilityCombo))
                {
                    _isValid = true;
                    _abilityCalled = abilityName;
                    break;
                }
                else
                {
                    _isValid = false;
                    _abilityCalled = "";
                    Locator.Instance.StatesOfPlayer.GetSetPlayerState = PlayerStates.StatesOfPlayer.Idle;
                }
            }
        }
        
        return _isValid;
    }

    private void CallAbility()
    {
        // put check cases here
        // if have enough energy break and continue with code below otherwise return

        switch(_abilityCalled)
        {
            case "Left Slide Kick Ability":
                LeftSlideKickAbility();
                break;
            case "Right Slide Kick Ability":
                RightSlideKickAbility();
                break;
            case "Push Back Ability":
                PushBackAbility(); 
                break;
            default:
                Debug.Log("////ERROR//// \n ABILITY CALLED DOES NOT EXIST");
                break;
        }
    }


    private void StoreAbilitiesData()
    {
        foreach(Ability playerAbility in _playerAbilitiesList)
        {
            _playerAbilities.Add(playerAbility.abilityName, playerAbility.keycodeCombinations);
            _abilityNames.Add(playerAbility.abilityName);
        }
    }


    ////////////////////////////////////// Abilities ///////////////////////////////////////////////////
    private void PushBackAbility()
    {
        // enable hitbox and then have the value scale up to 5 and then revert and disable
        _playerSecondAbilityHitBox.enabled = true;
        // call ability attack for second ability
        Locator.Instance.CombatControl.AbilityAttack(0);
        // turn on sprite effect
        _secondAbilitySprite.SetActive(true);
        // turn off player movement
        _playerMovement.enabled = false;
        // need a timer to wait here: one second
        Invoke("TurnOff2ndAbilityHitbox", _secondAbilityTimeOut);
    }


    private void TurnOff2ndAbilityHitbox()
    {
        _playerSecondAbilityHitBox.enabled = false;
        _playerMovement.enabled = true;
        _secondAbilitySprite.SetActive(false);
    }

    
    private void LeftSlideKickAbility()
    {
        // the direction we want to go 
        _firstAbilityForce = Vector2.left;
        // check direction player is facing 
        _isLookingRight = _playerMovement.FacingRight;
        // if looking right turn left
        if(_isLookingRight)
            Flip();
        // turn off movement
        _playerMovement.enabled = false;
        // get coroutine
        coroutine = Func();
        // do coroutine
        StartCoroutine(coroutine);
        // then flip back to original direction
        if(_isLookingRight)
            Invoke("Flip", 2.5f);
    }


    private void RightSlideKickAbility()
    {
        _firstAbilityForce = Vector2.right;
        _isLookingRight = _playerMovement.FacingRight;
        if(!_isLookingRight)
            Flip();
        _playerMovement.enabled = false;
        coroutine = Func();
        StartCoroutine(coroutine);
        if(!_isLookingRight)
            Invoke("Flip", 2.5f);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        // if thing is enemy
        if(collision.gameObject.tag == "Enemy")
        {
            // do ability attack method from combat tester class for first ability
            Locator.Instance.CombatControl.AbilityAttack(1);
            Debug.Log($"{timer}");
        }
    }


    private void Flip()
    {
        this.transform.Rotate(0,180,0);
    }


    private void TurnOffFirstAbilityHitbox()
    {
        _playerMovement.enabled = true;
        _playerFirstAbilityHitBox.gameObject.SetActive(false);
    }


    private IEnumerator Func()
    {
        // ignore enemy layer when slide starts so that player doesn't bounce off enemy
        Physics2D.IgnoreLayerCollision(6,7, true);
        // slide
        _playerRigidBody.AddForce(_firstAbilityForce * _firstAbilityMagnitude);
        // wait
        yield return new WaitForSeconds(_firstAbilityDistanceTimer);
        // stop slide
        _playerRigidBody.AddForce(-_firstAbilityForce * _firstAbilityMagnitude);
        // stop ignore enemy layer
        Physics2D.IgnoreLayerCollision(6,7, false);
        // turn on hitbox
        _playerFirstAbilityHitBox.gameObject.SetActive(true);
        // turn off hitbox
        Invoke("TurnOffFirstAbilityHitbox", 2);
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        timer = 0; 
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        timer = 0; 
    }
}