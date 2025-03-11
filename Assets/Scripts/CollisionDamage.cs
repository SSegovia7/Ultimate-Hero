using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Health player_health;
    [SerializeField] int collision_damage = 20;

    // Audio stuff to add on every script
    AudioManager audioManager;
    // Audio stuff to add on every script
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player"){;
            // Audio clip implementation
            audioManager.PlaySFX(audioManager.playerDamaged);
            player_health.TakeDamage(collision_damage);
        }else if (other.gameObject.tag == "Enemy"){
            
        }
        Debug.Log("You hit something");
    }
}
