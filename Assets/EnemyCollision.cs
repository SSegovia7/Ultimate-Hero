using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private int playerDamage = 20;
 
    private void OnCollisionEnter2D(Collision2D collision) {

        Debug.Log("Ouch you hit something");
        if(collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(playerDamage);
        }else if (collision.gameObject.tag == "Enemy")
        {
            playerHealth.TakeDamage(playerDamage);
        }
    }
    
}
