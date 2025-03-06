using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] public float attackCooldown = 3f; // amount of time after an enemy attacks before selecting a new attacker
    public float attackCooldownTimer = 0f;

    [SerializeField] public float attackerRefreshCooldown = 6f; // amount of time an enemy attacks for before selecting a new attacker
    public float attackerRefreshCooldownTimer = 0f;

    public NodeController playerNodeController;

    public List<EnemyAI> enemies;
    public int attackingEnemyIndex = -1;

    private void Awake()
    {
        instance = this;
    }
    private void FixedUpdate()
    {
        UpdateAttacker();
    }

    private void UpdateAttacker()
    {
        attackerRefreshCooldownTimer += Time.deltaTime;
        if (attackingEnemyIndex == -1)
        {
            attackCooldownTimer += Time.deltaTime;
        }
        if (attackCooldownTimer > attackCooldown || attackerRefreshCooldownTimer > attackerRefreshCooldown)
        {
            SetNewAttacker();
        }
    }

    private void SetNewAttacker()
    {
        attackCooldownTimer = 0;
        attackerRefreshCooldownTimer = 0;
        if (attackingEnemyIndex != -1)
        {
            enemies[attackingEnemyIndex].isAttacking = false;
        }
        attackingEnemyIndex = Random.Range(0, enemies.Count);
        Debug.Log($"attacking enemy: {attackingEnemyIndex}");
        enemies[attackingEnemyIndex].isAttacking = true;
    }

    // called by EnemyAI script when enemy dies
    public void OnDeadEnemy(EnemyAI deadEnemy)
    {
        Debug.Log("removing enemy");
        int index = enemies.IndexOf(deadEnemy);
        if (index != -1)
        {
            RemoveEnemyAt(index);
        }
    }

    public void OnEnemyAttack()
    {
        attackingEnemyIndex = -1;
        attackCooldownTimer = 0;
    }

    private void RemoveEnemyAt(int index)
    {
        enemies.RemoveAt(index);
        if (index == attackingEnemyIndex)
        {
            attackingEnemyIndex = -1;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
