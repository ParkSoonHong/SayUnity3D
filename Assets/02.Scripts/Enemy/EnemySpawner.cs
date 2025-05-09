using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour
{

    public float MaxSpawnRange = 4;  
    public float MinSpawnRange = 1;

    private List<GameObject> _enemyLists;

    public float WatiTime;

    private WaitForSeconds _baseWait;

    public bool spawnStart = true;

    void Awake()
    {
      
    }

    private void Start()
    {
        _enemyLists = EnemyManager.Instance.SpawnEnemyList;
        _baseWait = new WaitForSeconds(WatiTime);
        StartSpawning();

    }

    IEnumerator Spawn()
    {
        while(spawnStart)
        {
            yield return _baseWait;
            foreach (GameObject enemy in _enemyLists)
            {
                if (enemy != null && enemy.gameObject.activeInHierarchy == false)
                {
                    enemy.transform.position = transform.position * UnityEngine.Random.Range(MinSpawnRange, MaxSpawnRange);
                    enemy.gameObject.SetActive(true);
                    break;
                }
            }

          //  _baseWait = new WaitForSeconds(UnityEngine.Random.Range(MinSpawnRange, MaxSpawnRange));
        }
    }

    public void StartSpawning()
    {
        spawnStart = true;
        StartCoroutine(Spawn());
    }

    public void StopSpawning()
    {
        spawnStart = false;
        StopCoroutine(Spawn());
    }
}
