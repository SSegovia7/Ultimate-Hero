using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PosingController : MonoBehaviour
{
    public delegate bool CheckCombo(List<int> listOfInputs);
    private List<int> abilityCombo;
    CharacterMovement playerMovement;
    // this crap will need to be replaced with actual animator later, too busy
    SpriteRenderer playerSprite;
    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        abilityCombo = new List<int>();
        playerMovement = this.GetComponent<CharacterMovement>();
        playerSprite = this.GetComponentInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        // player press E to hold pose
        
        if(Input.GetKey(KeyCode.E))
        {
            PlayerStates.StateInstance.GetSetPlayerState = PlayerStates.StatesOfPlayer.Posing;
            playerMovement.enabled = false;
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                // store button press in list
                abilityCombo.Add((int)KeyCode.RightArrow);
                // change pose to right pose
                playerSprite.sprite = sprites[1];
            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                // store button press in list
                abilityCombo.Add((int)KeyCode.UpArrow);
                // change pose to up pose
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // store button press in list
                abilityCombo.Add((int)KeyCode.LeftArrow);
                // change pose to left pose
            }
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                // store button press in list
                abilityCombo.Add((int)KeyCode.DownArrow);
                // change pos to down pose
            }
        }
        
        if(Input.GetKeyUp(KeyCode.E))
        {
            PlayerStates.StateInstance.GetSetPlayerState = PlayerStates.StatesOfPlayer.Moving;
            playerMovement.enabled = true;
            // call method from ability class that checks if list has valid "combo input"
            // CheckCombo(abilityCombo);

            // reset list after to not have old combination data
            abilityCombo = new List<int>();
            playerSprite.sprite = sprites[0];
        }

    }
}
