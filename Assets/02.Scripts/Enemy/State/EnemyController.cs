using System.Collections.Generic;
using Unity.VisualScripting;
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
// 해야할것 정리
// 1. 모든 상태에 DIstance 확인가능 하도록 추상 클래스 작성
// 2. 보스 밑 엘리트 몬스터 밑 일반 모스터 공격 구현
// 3. 애니메이션 할당
// 4. 플레이어 공격 및 스킬 추가
// 5. 

public class EnemyController : MonoBehaviour, IDamageAble
{
    private EEnemyState _currentState = EEnemyState.Idle;
    private Dictionary<EEnemyState, IFSM> _stateMap;
    private Enemy _enemy;
    /*
    private EnemyIdle _enemyIdle;
    private EnemyReturn _enemyReturn;
    private EnemyPatrol _enemyPatrol;
    private EnemyAttack _enemyAttack;
    private EnemyDamaged _enemyDamaged;
    private EnemyTrace _enemyTrace;
    private EnemyDie _enemyDie;
    */
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();

        // 딕셔너리에 상태 객체 등록
        _stateMap = new Dictionary<EEnemyState, IFSM>
        {
            { EEnemyState.Idle,    new EnemyIdle(_enemy) },
            { EEnemyState.Trace,   new EnemyTrace(_enemy) },
            { EEnemyState.Return,  new EnemyReturn(_enemy) },
            { EEnemyState.Patrol,  new EnemyPatrol(_enemy) },
            { EEnemyState.Attack,  new EnemyAttack(_enemy) },
            { EEnemyState.Damaged, new EnemyDamaged(_enemy) },
            { EEnemyState.Die,     new EnemyDie(_enemy) }
        };

        _currentState = EEnemyState.Idle;
        _stateMap[_currentState].Start();
    }

    private void Update()
    {
        EEnemyState nextState = _stateMap[_currentState].Update();
        if (nextState != _currentState)
        {
            ChangeState(nextState);
        }
    }

    private void ChangeState(EEnemyState nextState)
    {
        // 현재 상태 종료
        _stateMap[_currentState].End();
        // 새 상태 진입
        _currentState = nextState;
        _stateMap[_currentState].Start();
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

        if (_enemy.Helath <= 0)
        {
            ChangeState(EEnemyState.Die);
            return;
        }

        ChangeState(EEnemyState.Damaged);
       
    }
   

}
