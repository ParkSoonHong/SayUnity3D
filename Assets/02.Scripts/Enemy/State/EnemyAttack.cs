using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAttack : IFSM
{
    private float _attackTimer = 0f;
    public float AttackCoolTime = 2f;

    private Damage _damage;
    private bool _isStarted = false;

    private Enemy _enemy;
    public EnemyAttack(Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {
        _damage = new Damage(_enemy.EnemyData.DamagePower, _enemy.gameObject, _enemy.EnemyData.KnockbackPower);

    }

    public void Start()
    {
        _isStarted = true;
        _enemy.StartCoroutine(Attack_Coroutine());
    }
    public EEnemyState Update()
    {
        if (_isStarted == false)
        {
            Start();
        }

        if (_enemy.TryFindTarget() == false) // 상대를 못찾으면
        {
            _attackTimer = 0;
            return EEnemyState.Trace;
        }
        // 일단 공격

        _attackTimer += Time.deltaTime;
        if (_attackTimer > AttackCoolTime)
        {

           
            _attackTimer = 0;
        }

        return EEnemyState.Attack;
    }
    public void End()
    {
        _isStarted = false;
    }
    public IEnumerator Attack_Coroutine()
    {
        Collider[] Colliders = Physics.OverlapSphere(_enemy.transform.position, _enemy.AttackDistance);

        foreach (Collider collider in Colliders)
        {
            if (collider.TryGetComponent<IDamageAble>(out IDamageAble damageAble) && collider.CompareTag("Player"))
            {
                damageAble.TakeDamage(_damage);
                break;
            }
        }
        yield return new WaitForSeconds(_attackTimer);

        yield return null;
    }
}
