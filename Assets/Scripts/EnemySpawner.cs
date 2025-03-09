using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private HealthBar enemyHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // returns the EnemyAI component of the spawned enemy, for use with the EnemyManager that calls this function
    public EnemyAI SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefab, transform);
        // link the necessary components
        EnemyAI enemyAI = spawnedEnemy.GetComponent<EnemyAI>();
        enemyAI.enemyManager = enemyManager;
        Health enemyHealth = spawnedEnemy.GetComponent<Health>();
        enemyHealth.healthBar = enemyHealthBar;
        return enemyAI;

    }
}
