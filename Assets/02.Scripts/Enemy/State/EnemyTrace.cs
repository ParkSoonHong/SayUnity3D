using UnityEngine;
using UnityEngine.AI;

public class EnemyTrace : IFSM
{

    private Enemy _enemy;


    public EnemyTrace(Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {
        _enemy.Agent.speed = _enemy.EnemyData.MoveSpeed;
    }

    public void Start()
    {
        _enemy.Agent.isStopped = false;
        _enemy.Agent.SetDestination(_enemy.Player.transform.position);
    }
    public EEnemyState Update()
    {

        if (_enemy.TryReturnPoint())
        {
            return EEnemyState.Return; // 리턴 포인트로 되돌아가기
        }

        if (_enemy.TryAttack())
        {
            return EEnemyState.Attack;
        }
;
       
        return EEnemyState.Trace;
    }
    public void End()
    {

    }

}
