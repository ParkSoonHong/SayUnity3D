using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyDie : IFSM
{
    private Enemy _enemy;

    private float _deathTime;

    private bool _isStarted = false;

    public EnemyDie (Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {
        _deathTime = _enemy.EnemyData.DeathTime;
    }

    public void Start()
    {
       _enemy.StartCoroutine(Die_Coroutine());
    }

    // Update is called once per frame
    public EEnemyState Update()
    {
        return EEnemyState.Die;
    }

    public void End()
    {

    }

    public IEnumerator Die_Coroutine()
    {
        float dieTimer = 0;
        while(dieTimer < _deathTime)
        {
            dieTimer += Time.deltaTime;
            Vector3 dir = _enemy.transform.position;
            dir.z -= Time.deltaTime * 1;
            _enemy.transform.position = dir;
        }
       
        _enemy.gameObject.SetActive(false);
        yield break;
    }
}
