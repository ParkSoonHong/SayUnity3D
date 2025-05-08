using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyReturn
{
    
    private float _moveSpeed = 3.3f;

    private Vector3 _startPosition;

    private Enemy _enemy;

    private bool _isStarted = false;

    public EnemyReturn(Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {
        _moveSpeed = _enemy.EnemyData.MoveSpeed;
        _enemy.Agent.speed = _moveSpeed;
    }

    public void Start()
    {
        _isStarted = true;
    }
    public EEnemyState Update()
    {
        if (_isStarted == false)
        {
            Start();
        }

        if (_enemy.TryEndPoint(_startPosition))
        {
            _enemy.Agent.ResetPath();
            _enemy.Agent.isStopped = true;
            return EEnemyState.Idle;
        }

        if (_enemy.TryFindTarget())
        {
            _enemy.Agent.ResetPath();
            _enemy.Agent.isStopped = true;
            return EEnemyState.Trace;
        }

        return EEnemyState.Return;
    }
    public void End()
    {
        
    }

}
