// Author: Nick Hwang
// For: Beat Em Up Style Tutorials
// youtube.com/c/nickhwang

using System.Collections.Generic;
using UnityEngine;

public class CombatTester : MonoBehaviour
{
    [SerializeField] private bool canAttack = true;
    
    [SerializeField] private Collider2D inLineCollider;
    
    [SerializeField] private LayerMask enemyLayer;

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
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        contactFilter2D.SetLayerMask(enemyLayer);
    }

    // Update is called once per frame
    void Update()
    {
        controls = input.GetInput();
        if (controls.AttackState & punchCooldownTimer == 0)
        {
            Debug.Log("Punching");
            punchCooldownTimer = punchCooldown;
            punchDurationTimer = punchDuration;
            isPunching = true;
            attackSprite.SetActive(true);

            inLineCollider.OverlapCollider(contactFilter2D, cols);
            if (cols.Count > 0)
            {
                foreach (var col in cols)
                {
                    print(col.transform.name);
                    if (col.TryGetComponent(out SpriteRenderer sr))
                    {
                        sr.color = Color.red;
                    }
                }
            }
        }

        // This updates punchCooldownTimer typeshit
        if (punchCooldownTimer > 0)
        {
            punchCooldownTimer -= Time.deltaTime;
        }
        if (punchCooldownTimer < 0)
        {
            punchCooldownTimer = 0;
        }
        // This updates punchDurationTimer typeshit typeshit
        if (punchDurationTimer > 0)
        {
            punchDurationTimer -= Time.deltaTime;
        }
        if (punchDurationTimer < 0)
        {
            punchDurationTimer = 0;
            isPunching = false;
            attackSprite.SetActive(false);
        }

    }
}
