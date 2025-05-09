using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : IFSM
{
    private List<Vector3> _patrolPoints;
    private Vector3 _patrolMaxRange;
    private Vector3 _patrolMinRange;
    private Vector3 _currentPatrolPoint;

    private int _totalPatrolCount;

    private Enemy _enemy;
  
    public EnemyPatrol(Enemy enemy)
    {
        _enemy = enemy;
        _totalPatrolCount = _enemy.EnemyData.TotalPatrolCount;
        _patrolMaxRange = _enemy.EnemyData.PatrolMaxRange;
        _patrolMinRange = _enemy.EnemyData.PatrolMinRange;
        Initialize();
    }

    private void Initialize()
    {
        _patrolPoints = new List<Vector3>();


        SetInitializePoint();
    }

    public void Start()
    {
        SetNextPatrolPoint();
    }

    public EEnemyState Update()
    {
        if (_enemy.TryFindTarget())
        {
            return EEnemyState.Trace;
        }

        if (_enemy.TryEndPoint(_currentPatrolPoint)) // 목표 도달
        {
            SetNextPatrolPoint();
        }

        _enemy.transform.LookAt(_currentPatrolPoint);
        return EEnemyState.Patrol;
    
    }
    public void End()
    {

    }

    private void SetInitializePoint()
    {
        for (int i = 0; i < _totalPatrolCount; i++)
        {
            while (true)
            {
                float X = UnityEngine.Random.Range(_patrolMinRange.x, _patrolMaxRange.x);
                float Y = UnityEngine.Random.Range(_patrolMinRange.y, _patrolMaxRange.y);
                Vector3 point = new Vector3(X, Y);
                if (_patrolPoints.Contains(point) == false)
                {
                    _patrolPoints.Add(point);
                    break;
                }
            }
        }
    }

    public void SetNextPatrolPoint()
    {
        while (true)
        {
            int range = Random.Range(0, _patrolPoints.Count);
            if (_currentPatrolPoint != _patrolPoints[range])
            {
                _currentPatrolPoint = _patrolPoints[range];
                break;
            }
        }
        _enemy.Agent.isStopped = false;
        _enemy.Agent.SetDestination(_currentPatrolPoint);
    }
}
