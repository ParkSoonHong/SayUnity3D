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
        Debug.Log("StartTrace");
        _enemy.Animator.SetBool("Move", true);
        _enemy.Agent.isStopped = false;
    }
    public EEnemyState Update()
    {

        if (_enemy.TryReturnPoint())
        {
            Debug.Log("Move -> Retrun");
            return EEnemyState.Return; // 리턴 포인트로 되돌아가기
        }

        if (_enemy.TryAttack())
        {
            Debug.Log("Move -> Attack");
            return EEnemyState.Attack;
        }
;
        _enemy.Agent.SetDestination(_enemy.Player.transform.position);

        return EEnemyState.Trace;
    }
    public void End()
    {

    }

}
