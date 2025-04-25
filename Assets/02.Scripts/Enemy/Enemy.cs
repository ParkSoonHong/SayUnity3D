using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Trace,
    Return,
    Attack,
    Damaged,
    Patrol,
    Die,
}


public class Enemy : MonoBehaviour, IDamageAble
{
    public EnemyState CurrentState = EnemyState.Idle;

    protected GameObject _player;
    private CharacterController _characterController;
    private Vector3 _startPosition;
    protected NavMeshAgent _agent;

    public UI_Enemy UI_Enemy;

    public float FindDistance = 7f;
    public float AttackDistance = 2f;
    public float ReturnDistance = 5f;
    public float MoveSpeed = 3.3f;
    public float DeathTime = 2f;
    public float AttackCoolTime = 2f;
    private float _attackTimer = 0f;

    private float _maxHealth = 100;
    private float _health = 0;


    public float DamagedTime = 0.5f;

    public float PatrolTime = 2f;
    private float _patrolTimer = 0f;

    public List<Vector3> PatrolPoints;
    private Vector3 _currentPatrolPoint;
    public float MovePointDistance = 0.1f;

    public void Start()
    {
        _startPosition = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
        _health = _maxHealth;
    }


    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
            {
                 Idle();
                 break;
            }
            case EnemyState.Trace:
            {
                Trace();
                break;
            }
            case EnemyState.Return:
            {
                Return();
                break;
            }
            case EnemyState.Attack:
            {
                Attack();
                break;
            }
            case EnemyState.Patrol:
            {
                Patrol();
                break;
            }
        }
    }

    public void TakeDamage(Damage damage)
    {
      
      
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }
        _agent.isStopped = true;
        _agent.ResetPath();
        Vector3 dir = (damage.From.transform.position - transform.position) * -1;
        dir.Normalize();
        _patrolTimer = 0;
        transform.position = transform.position + (dir * damage.KnockbackPower * Time.deltaTime);

        _health -= damage.Value;
        UI_Enemy.UpdateHealth(_health/ _maxHealth);
        if (_health <= 0)
        {
            SetState ( EnemyState.Die);
            StartCoroutine(Die_coruotin());
            return;
        }

        SetState(EnemyState.Damaged);
        StartCoroutine(Damaged_Coroutine());
    }

    protected void Idle()
    {

        if (TryFindTarget()) return;

        _patrolTimer += Time.deltaTime;
        if(_patrolTimer >= PatrolTime)
        {
            SetState (EnemyState.Patrol);
            _patrolTimer = 0;
        }
    }

    protected void Trace()
    {

        if(TryReturnPoint()) return;

        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            
            SetState( EnemyState.Attack);
            return;
        }

        //Vector3 dir = (_player.transform.position - transform.position).normalized;
        //transform.LookAt(_player.transform);
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position);

    }

    protected void Patrol()
    {
        if (TryFindTarget()) return;

        if (Vector3.Distance(transform.position, _currentPatrolPoint) <= MovePointDistance)
        {
            transform.position = _currentPatrolPoint;
            SetState(CurrentState);
            return;
        }
        transform.LookAt(_currentPatrolPoint);
        _agent.SetDestination(_currentPatrolPoint);
       
        //Vector3 dir = (_currentPatrolPoint - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);

     
    }

    protected void Return()
    {
        if (Vector3.Distance(transform.position, _startPosition) <= MovePointDistance)
        {
            transform.position = _startPosition;
            SetState(EnemyState.Idle);
            return;
        }

        if(TryFindTarget()) return;

        //Vector3 dir = (_startPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_startPosition);
    }

    protected void Attack()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            SetState(EnemyState.Trace);
            _attackTimer = 0;
            return;
        }
        // 일단 공격

        _attackTimer += Time.deltaTime;
        if(_attackTimer > AttackCoolTime)
        {
           

            _attackTimer = 0;
        }

    }

    private IEnumerator Attack_Coroutine()
    {
        Physics.OverlapSphere(transform.forward, AttackDistance); 

        // 공격 상태 아니면 return
        yield return new WaitForSeconds(_attackTimer);
    }

    private IEnumerator Damaged_Coroutine()
    {
 
        yield return new WaitForSeconds(DamagedTime);
        _agent.isStopped = false;
        SetState(EnemyState.Trace);
        yield break;
    }

    private IEnumerator Die_coruotin()
    {
        Vector3 dir = transform.position;
        dir.z -= Time.deltaTime * MoveSpeed;
        transform.position = dir;

        yield return new WaitForSeconds(DeathTime);
        gameObject.SetActive(false);
        yield break;
    }

    private bool TryFindTarget()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            SetState(EnemyState.Trace);
            return true;
        }
        return false;
    }

    private bool TryReturnPoint()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > ReturnDistance)
        {
            SetState(EnemyState.Return);
            return true;
        }
        return false;
    }

    protected void SetState(EnemyState enemyState)
    {
        CurrentState = enemyState;

        if(enemyState == EnemyState.Patrol)
        {
            SetPatrolPoint();
        }
    }

    private void SetPatrolPoint()
    {
       
       while(true)
       {
            int range = Random.Range(0, PatrolPoints.Count);
            if (_currentPatrolPoint != PatrolPoints[range])
            {
                _currentPatrolPoint = PatrolPoints[range];
                break;
            }
       }
   
    }
}
