using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance = null;

    public List<GameObject> EnemyList;

    private List<GameObject> _spawnEnemyList;
    public List<GameObject> SpawnEnemyList => _spawnEnemyList;

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
        _spawnEnemyList = new List<GameObject>(EnemyList.Count * EnemySpawnCount);
       
    }

    private void Start()
    {
        EnemyPool();
    }

    private void EnemyPool()
    {
        for(int i=0; i< EnemyList.Count; i++)
        {
            for(int j=0; j< EnemySpawnCount; j++)
            {
                GameObject enemy = Instantiate(EnemyList[i],this.transform);
                enemy.gameObject.SetActive(false);
                _spawnEnemyList.Add(enemy);
            }
        }
       
    }
}
