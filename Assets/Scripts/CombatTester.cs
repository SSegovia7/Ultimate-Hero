// Author: Nick Hwang
// For: Beat Em Up Style Tutorials
// youtube.com/c/nickhwang

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CombatTester : MonoBehaviour
{
    [SerializeField] private bool canAttack = true;
    
    [SerializeField] private Collider2D inLineCollider;
    [SerializeField] private BoxCollider2D[] _abilityInLineCollider;
    
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private GameObject attackSprite;

    [SerializeField] private float punchCooldown = 2;
    [SerializeField] private float _secondAbilityDamage;
    [SerializeField] private float _firstAbilityDamage;
    private float punchCooldownTimer;
    private IEnumerator colorChangeCoroutine;

    [SerializeField] private float punchDuration = 1;
    [SerializeField] private List<int> _abilitiesDamage;
    private float punchDurationTimer;

    public bool isPunching = false;
    Pose playerEnergy;

    PlayerInput input;
    Controls controls = new Controls();
    private ContactFilter2D contactFilter2D;
    public List<Collider2D> cols = new List<Collider2D>();

    [SerializeField] public int punch_damage = 20;
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        contactFilter2D.SetLayerMask(enemyLayer);
    }
    void Start()
    {   
        playerEnergy = this.GetComponent<Pose>();
    }
    // Update is called once per frame
    void Update()
    {
        BasicAttack();
    }

    void BasicAttack()
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
                    print($"You attacked: {col.transform.name}");
                    if (col.TryGetComponent(out SpriteRenderer sr))
                    {
                        sr.color = Color.red;
                        colorChangeCoroutine = ColorChange(sr);
                        StartCoroutine(colorChangeCoroutine);
                    }
                    Health enemy_health = col.GetComponent<Health>(); //damaging enemy and updating health
                    
                    enemy_health.TakeDamage(punch_damage);
                    playerEnergy.IncreasePose(5);

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

    // this should not be public - change later
    public void AbilityAttack(int abilityNumber)
    {
        _abilityInLineCollider[abilityNumber].OverlapCollider(contactFilter2D,cols);
        if(cols.Count > 0)
        {
            foreach (var col in cols)
            {
                print(col.transform.name);
                if(abilityNumber == 0)
                {
                    col.gameObject.TryGetComponent<Health>(out Health enemyHealth);
                    if(enemyHealth != null)
                        enemyHealth.EnemyDamaged(_secondAbilityDamage);
                }
                if(abilityNumber == 1)
                {
                    col.gameObject.TryGetComponent<Health>(out Health enemyHealth);
                    if(enemyHealth != null)
                        enemyHealth.EnemyDamaged(_firstAbilityDamage);
                }
                if (col.TryGetComponent(out SpriteRenderer sr))
                {
                    sr.color = Color.red;
                    colorChangeCoroutine = ColorChange(sr);
                    StartCoroutine(colorChangeCoroutine);
                }
            }
        }
    }

    private IEnumerator ColorChange(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(.2f);
        sr.color = Color.white;
    }

}
