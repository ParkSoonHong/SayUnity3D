using System.Collections;
using UnityEngine;

public class EnemyDamaged : IFSM
{
    private float _damagedTime = 0.5f;
    private float _damagedTimer = 0f;

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
        _enemy.Agent.isStopped = true;
        _enemy.Agent.ResetPath();
        _enemy.Animator.SetTrigger("Damage");
        _damagedTimer = 0;
    }
    public EEnemyState Update()
    {
        _damagedTimer += Time.deltaTime;

        if(_damagedTimer > _damagedTime)
        {
            _damagedTimer = 0;
            return EEnemyState.Idle;
        }

        return EEnemyState.Damaged;
    }

    public void End()  // 끝낼시 필요
    {
        _damagedTimer = 0;
        _enemy.Agent.isStopped = false;
    }

}
