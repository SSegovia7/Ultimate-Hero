// Author: Nick Hwang
// For: Beat Em Up Style Tutorials
// youtube.com/c/nickhwang

using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatTester : MonoBehaviour
{
    [SerializeField] private bool canAttack = true;
    
    [SerializeField] private Collider2D inLineCollider;
    
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private GameObject attackSprite;

    [SerializeField] private float punchCooldown = 2;
    private float punchCooldownTimer;

    [SerializeField] private float punchDuration = 1;
    private float punchDurationTimer;

    public bool isPunching = false;
    

    PlayerInput input;
    Controls controls = new Controls();
    private ContactFilter2D contactFilter2D;
    public List<Collider2D> cols = new List<Collider2D>();

    [SerializeField] public int punch_damage = 20;
    
    private void Awake()
    {
        contactFilter2D.SetLayerMask(playerLayer);
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
                if (col.TryGetComponent(out SpriteRenderer sr))
                {
                    sr.color = Color.red;
                }
                Health enemy_health = col.GetComponent<Health>(); //damaging enemy and updating health
                enemy_health.TakeDamage(punch_damage);
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
