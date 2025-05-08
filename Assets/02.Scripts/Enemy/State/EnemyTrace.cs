using UnityEngine;
using UnityEngine.AI;

public class EnemyTrace : IFSM
{
    private float _moveSpeed = 3.3f;

    private Vector3 _startPosition;

    private Enemy _enemy;

    private GameObject _player;

    private bool _isStarted = false;

    public EnemyTrace(Enemy enemy)
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
        _isStarted = true;
    }
    public EEnemyState Update()
    {
        if (_isStarted == false)
        {
            Start();
        }

        if (_enemy.TryReturnPoint())
        {
            return EEnemyState.Return; // 리턴 포인트로 되돌아가기
        }

        if (_enemy.TryAttack())
        {
            return EEnemyState.Attack;
        }
;
        _enemy.Agent.SetDestination(_player.transform.position);
        return EEnemyState.Trace;
    }
    public void End()
    {

    }

}
