using System;
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

    public EEnemyState CurrentState => _currentState;
    private Dictionary<EEnemyState, IFSM> _stateMap;
    private Enemy _enemy;
  
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        Initialize();
    }

    private void Update()
    {
        EEnemyState nextState = _stateMap[_currentState].Update();
        Debug.Log(nextState);
        if (nextState != _currentState)
        {
            ChangeState(nextState);
        }
    }

    private void Initialize()
    {
        _stateMap = new Dictionary<EEnemyState, IFSM>();
        // 딕셔너리에 상태 객체 등록
        foreach (EEnemyState state in _enemy.EnemyData.AvailableStates)
        {
            _stateMap[state] = CreateStateInstance(state);
        }
        // 최소한 Idle, trace 상태는 항상 있어야 함을 보장
        if (!_stateMap.ContainsKey(EEnemyState.Idle))
        {
            _stateMap.Add(EEnemyState.Idle, new EnemyIdle(_enemy));
        }

        if (!_stateMap.ContainsKey(EEnemyState.Trace))
        {
            _stateMap.Add(EEnemyState.Trace, new EnemyTrace(_enemy));
        }

        _currentState = EEnemyState.Idle;
        _stateMap[_currentState].Start();
    }

    private IFSM CreateStateInstance(EEnemyState state) 
    {
        switch (state)
        {
            case EEnemyState.Idle:
                {
                    return  new EnemyIdle(_enemy);
                }
            case EEnemyState.Trace:
                {
                    return new EnemyTrace(_enemy);
                }
            case EEnemyState.Return:
                {
                    return new EnemyReturn(_enemy);
                }
            case EEnemyState.Attack:
                {
                 return new EnemyAttack(_enemy);
                }
            case EEnemyState.Damaged:
                {
                    return new EnemyDamaged(_enemy);
                }
            case EEnemyState.Patrol:
                {
                    return new EnemyPatrol(_enemy);
                }
            case EEnemyState.Die:
                {
                    return new EnemyDie(_enemy);
                }
        }
        return null;
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
