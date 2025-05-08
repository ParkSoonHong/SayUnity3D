using System;
using System.Collections;
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
        _isStarted = true;
    }

    // Update is called once per frame
    public EEnemyState Update()
    {
        if (_isStarted == false)
        {
            Start();
        }
        return EEnemyState.Die;
    }

    public void End()
    {

    }

    public IEnumerator Die_coruotin()
    {
        Vector3 dir = _enemy.transform.position;
        dir.z -= Time.deltaTime * 1;
        _enemy.transform.position = dir;

        yield return new WaitForSeconds(_deathTime);
        _enemy.gameObject.SetActive(false);
        yield break;
    }
}
