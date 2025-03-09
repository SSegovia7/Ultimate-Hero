using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;

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
        EnemyAI enemyAI = spawnedEnemy.GetComponent<EnemyAI>();
        enemyAI.enemyManager = enemyManager;
        return enemyAI;

    }
}
