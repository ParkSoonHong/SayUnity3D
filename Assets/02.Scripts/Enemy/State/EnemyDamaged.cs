using System.Collections;
using UnityEngine;

public class EnemyDamaged : IFSM
{
    private float _damagedTime = 0.5f;
    private float _deathTime = 2f;

    private bool _isStarted = false;

    private Enemy _enemy;
    public EnemyDamaged(Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {

    }

 

    public void Start() // 시작시 필요
    {
        _enemy.Agent.isStopped = false;
        _isStarted = false;
    }
    public EEnemyState Update()
    {
        return EEnemyState.Damaged;
    }

    public void End()  // 끝낼시 필요
    {

    }

    public IEnumerator Damaged_Coroutine()
    {

        yield return new WaitForSeconds(_damagedTime);
        _enemy.Agent.isStopped = false;
        yield break;
    }
}
