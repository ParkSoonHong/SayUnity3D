using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyReturn : IFSM
{
    
    private Vector3 _startPosition;

    private Enemy _enemy;


    public EnemyReturn(Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {
        _startPosition = _enemy.transform.position;
    }

    public void Start()
    {
        _enemy.Agent.isStopped = false;
        _enemy.Agent.SetDestination(_enemy.Player.transform.position);
    }
    public EEnemyState Update()
    {
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
