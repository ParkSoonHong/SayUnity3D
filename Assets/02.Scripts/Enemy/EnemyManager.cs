using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance = null;

    public List<Enemy> _enemyList;

    private List<Enemy> _spawnEnemyList;
    public List<Enemy> SpawnEnemyList => _spawnEnemyList;

    public int EnemySpawnCount = 10;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _spawnEnemyList = new List<Enemy>(_enemyList.Count * EnemySpawnCount);
        EnemyPool();
    }

    private void EnemyPool()
    {
        for(int i=0; i< _enemyList.Count; i++)
        {
            for(int j=0; j< EnemySpawnCount; j++)
            {
                Enemy enemy = Instantiate(_enemyList[i],this.transform);
                enemy.gameObject.SetActive(false);
                // enemy.Initialize();
                _spawnEnemyList.Add(enemy);
            }
        }
       
    }
}
