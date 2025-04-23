using System.Collections.Generic;
using System.Collections;
using UnityEngine;



public class Enemy : MonoBehaviour
{
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

    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;
    private CharacterController _characterController;
    private Vector3 _startPosition;

    public float FindDistance = 7f;
    public float AttackDistance = 2f;
    public float ReturnDistance = 5f;
    public float MoveSpeed = 3.3f;
    public float DeathTime = 2f;
    public float AttackCoolTime = 2f;
    private float _attackTimer = 0f;

    public int Health = 100;
    public float DamagedTime = 0.5f;
    private float _damagedTimer = 0f;

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
        Vector3 dir = (damage.From.transform.position - transform.position) * -1;
        dir.Normalize();
        _patrolTimer = 0;
        _characterController.Move(dir * damage.KnockbackPower * Time.deltaTime);
      
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;
        if(Health <= 0)
        {
            SetState ( EnemyState.Die);
            StartCoroutine(Die_coruotin());
            return;
        }

        SetState(EnemyState.Damaged);
        StartCoroutine(Damaged_Coroutine());
    }

    private void Idle()
    {

        if (FindTarget()) return;

        _patrolTimer += Time.deltaTime;
        if(_patrolTimer >= PatrolTime)
        {
            SetState (EnemyState.Patrol);
            _patrolTimer = 0;
        }
    }

    private void Trace()
    {

        if(ReturnPoint()) return;

        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            SetState( EnemyState.Attack);
            return;
        }

        Vector3 dir = (_player.transform.position - transform.position).normalized;
        transform.LookAt(_player.transform);
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

    }

    private void Patrol()
    {
        if (FindTarget()) return;

        if (Vector3.Distance(transform.position, _currentPatrolPoint) <= MovePointDistance)
        {
            Debug.Log("SetPatrolPoint");
            transform.position = _currentPatrolPoint;
            SetState(CurrentState);
            return;
        }
        transform.LookAt(_currentPatrolPoint);
        Vector3 dir = (_currentPatrolPoint - transform.position).normalized;

        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

     
    }

    private void Return()
    {
        if (Vector3.Distance(transform.position, _startPosition) <= MovePointDistance)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = _startPosition;
            SetState(EnemyState.Idle);
            return;
        }

        if(FindTarget()) return;

        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            SetState(EnemyState.Trace);
            _attackTimer = 0;
            return;
        }

        _attackTimer += Time.deltaTime;
        if(_attackTimer > AttackCoolTime)
        {
            Debug.Log("플레이어 공격!");
            _attackTimer = 0;
        }

    }

    private IEnumerator Damaged_Coroutine()
    {
        yield return new WaitForSeconds(DamagedTime);
        SetState(EnemyState.Trace);
    }

    private IEnumerator Die_coruotin()
    {
        yield return new WaitForSeconds(DeathTime);
        gameObject.SetActive(false);
    }

    private bool FindTarget()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            SetState(EnemyState.Trace);
            return true;
        }
        return false;
    }

    private bool ReturnPoint()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            SetState(EnemyState.Return);
            return true;
        }
        return false;
    }

    private void SetState(EnemyState enemyState)
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
