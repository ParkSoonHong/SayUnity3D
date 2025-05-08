using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum EEnemyState
{
    Idle,
    Trace,
    Return,
    Attack,
    Damaged,
    Patrol,
    Die,
}


public class EnemyController : MonoBehaviour, IDamageAble
{
    private EEnemyState _currentState = EEnemyState.Idle;

    private Enemy _enemy;
    private EnemyIdle _enemyIdle;
    private EnemyReturn _enemyReturn;
    private EnemyPatrol _enemyPatrol;
    private EnemyAttack _enemyAttack;
    private EnemyDamaged _enemyDamaged;
    private EnemyTrace _enemyTrace;
    private EnemyDie _enemyDie;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();

        _enemyIdle = new EnemyIdle(_enemy);
        _enemyReturn = new EnemyReturn(_enemy);
        _enemyTrace = new EnemyTrace(_enemy);
        _enemyPatrol = new EnemyPatrol(_enemy);
        _enemyAttack = new EnemyAttack(_enemy);
        _enemyDamaged = new EnemyDamaged(_enemy);
        _enemyDie = new EnemyDie(_enemy);
    }

    private void Update()
    {
        
        switch (_currentState)
        {
            case EEnemyState.Idle:
                {
                    SetState(_enemyIdle.Update());
                    break;
                }
            case EEnemyState.Trace:
                {
                    SetState(_enemyTrace.Update());
                    break;
                }
            case EEnemyState.Return:
                {
                    SetState(_enemyReturn.Update());
                    break;
                }
            case EEnemyState.Attack:
                {
                    SetState(_enemyAttack.Update());
                    break;
                }
            case EEnemyState.Patrol:
                {
                    SetState(_enemyPatrol.Update());
                    break;
                }
            case EEnemyState.Die:
                {
                    SetState(_enemyIdle.Update());
                    break;
                }
        }
        
    }

    private void SetState(EEnemyState enemyState)
    {
        _currentState = enemyState;
        // 애니메이션 추가
    }

    public void TakeDamage(Damage damage)
    {
        if (_currentState == EEnemyState.Damaged || _currentState == EEnemyState.Die)
        {
            return;
        }
        _enemy.Agent.isStopped = true;
        _enemy.Agent.ResetPath();

        // 넉백
        Vector3 dir = (damage.From.transform.position - transform.position) * -1;
        dir.Normalize();
        transform.position = transform.position + (dir * damage.KnockbackPower * Time.deltaTime);

        _enemy.TakeDamage(damage);

        StateInitialize();
        if (_enemy.Helath <= 0)
        {
            SetState(EEnemyState.Die);
            StartCoroutine(_enemyDie.Die_coruotin());
            return;
        }

        SetState(EEnemyState.Damaged);
        StartCoroutine(_enemyDamaged.Damaged_Coroutine());
    }
    private void StateInitialize()
    {
        _enemyIdle.End();
        _enemyTrace.End();
        _enemyReturn.End();
        _enemyAttack.End();
        _enemyPatrol.End();
    }

}
