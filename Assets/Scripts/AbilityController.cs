using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;


public class AbilityController : MonoBehaviour
{
    // list of available abilities in the game
    [SerializeField] private List<Ability> _playerAbilitiesList;
    [SerializeField] private BoxCollider2D _playerAbilityHitBox;
    // replace 
    [SerializeField] private GameObject _secondAbilitySprite;

    // 
    [SerializeField] private float _secondAbilityTimeOut;
    
    private List<string> _abilityNames;
    private Dictionary<string, List<KeyCode>> _playerAbilities;
    private string _abilityCalled;
    private BoxCollider2D _hitboxTemp;
    private bool isValid;
   

    void Start()
    {
        // list to hold the ability names 
        _abilityNames = new List<string>();
        // dictionary to hold the correct input combinations, can be acquired by their names
        _playerAbilities = new Dictionary<string, List<KeyCode>>();
        // basic intializer of string variable
        _abilityCalled = "";
        _hitboxTemp = _playerAbilityHitBox;
        // tie method to event in posingcontroller
        Locator.Instance.PosingControll.AbilityUsed += HandlePlayerAbility;
        // store data from Ability list into dictionary and name list
        StoreAbilitiesData();
    }


    void Update()
    {

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

// broken
    public bool IsValidCombo(List<KeyCode> playerInput)
    {
        foreach(string abilityName in _abilityNames)
        {
            if(_playerAbilities.TryGetValue(abilityName, out List<KeyCode> abilityCombo))
            {
                if(playerInput.SequenceEqual(abilityCombo))
                {
                    isValid = true;
                    _abilityCalled = abilityName;
                    break;
                }
                else
                {
                    isValid = false;
                    _abilityCalled = "";
                }
            }
        }
        
        return isValid;
    }


    private void CallAbility()
    {
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

    // Abilities
    private void PushBackAbility()
    {
        // enable hitbox and then have the value scale up to 5 and then revert and disable
        _playerAbilityHitBox.enabled = true;
        Locator.Instance.CombatControl.AbilityAttack();
        _secondAbilitySprite.SetActive(true);
        // need a timer to wait here: one second
        Invoke("TurnOffHitbox", _secondAbilityTimeOut);
        

        // also need to have image move alongside with hitbox as it changes scale values
          
    }


    private void TurnOffHitbox()
    {
        // 
        _playerAbilityHitBox.size = _hitboxTemp.size;
        _playerAbilityHitBox.enabled = false;
        _secondAbilitySprite.SetActive(false);
    }

    
    private void LeftSlideKickAbility()
    {
        Debug.Log("LeftSlide");
    }


    private void RightSlideKickAbility()
    {
        Debug.Log("RightSide");
    }
}
