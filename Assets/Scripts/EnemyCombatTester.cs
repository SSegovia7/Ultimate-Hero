// Author: Nick Hwang
// For: Beat Em Up Style Tutorials
// youtube.com/c/nickhwang

using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatTester : MonoBehaviour
{
    [SerializeField] int attackDamage = 20;

    [SerializeField] private bool canAttack = true;
    
    [SerializeField] private Collider2D inLineCollider;
    
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private GameObject attackSprite;

    [SerializeField] private float punchCooldown = 2;
    private float punchCooldownTimer;

    [SerializeField] private float punchDuration = 1;
    private float punchDurationTimer;

    public bool isPunching = false;
    

    private ContactFilter2D contactFilter2D;
    public List<Collider2D> cols = new List<Collider2D>();

    [SerializeField] public int punch_damage = 20;

    // Audio stuff to add on every script
    AudioManager audioManager;
   

    private void Awake()
    {
        contactFilter2D.SetLayerMask(playerLayer);
        // Audio stuff to add on every script
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void BasicAttack()
    {
        Debug.Log("Enemy Punching");
        punchCooldownTimer = punchCooldown;
        punchDurationTimer = punchDuration;
        isPunching = true;
        attackSprite.SetActive(true);

        inLineCollider.OverlapCollider(contactFilter2D, cols);
        if (cols.Count > 0)
        {
            foreach (var col in cols)
            {
                print($"Enemy attacked: {col.transform.name}");
                // if (col.TryGetComponent(out SpriteRenderer sr))
                // {
                //     sr.color = Color.red;
                // }
                if (col.gameObject.tag == "Player")
                {
                    Health playerHealth = col.gameObject.GetComponentInParent<Health>();
                    if (playerHealth)
                    {
                        // Audio clip implementation
                        audioManager.PlaySFX(audioManager.playerDamaged);
                        Debug.Log("hit player");
                        playerHealth.TakeDamage(attackDamage);
                    }
                }
            }
        }

    }

    void ClearHitbox()
    {
        isPunching = false;
        attackSprite.SetActive(false);
    }

    // this should not be public - change later
    // public void AbilityAttack()
    // {
    //     _abilityInLineCollider.OverlapCollider(contactFilter2D,cols);
    //     if(cols.Count > 0)
    //     {
    //         foreach (var col in cols)
    //         {
    //             print(col.transform.name);
    //             if (col.TryGetComponent(out SpriteRenderer sr))
    //             {
    //                 sr.color = Color.red;
    //             }
    //         }
    //     }
    // }

}
