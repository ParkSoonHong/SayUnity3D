using UnityEngine;

public class EnemyIdle : IFSM
{
    private float _patrolTime = 2f;
    private float _patrolTimer = 0f;

    private Enemy _enemy;

    public EnemyIdle(Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {
        _patrolTime = _enemy.EnemyData.PatrolTime;
    }

    public void Start() // 시작시 필요
    {
        
    }
    public EEnemyState Update()
    {

        if (_enemy.TryFindTarget()) // 발견
        {
            _patrolTimer = 0;
            return EEnemyState.Trace;
        }
        // 미 발견

        _patrolTimer += Time.deltaTime;
        if (_patrolTimer < _patrolTime)
        {
            _patrolTimer = 0;
            return EEnemyState.Patrol;
        }
        return EEnemyState.Idle;
    }
    public void End()  // 끝낼시 필요
    {

    }

}
