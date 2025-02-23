using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Health player_health;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player"){
            player_health.TakeDamage(20);
        }else if (other.gameObject.tag == "Enemy"){
            
        }
        Debug.Log("You hit something");
    }
}
