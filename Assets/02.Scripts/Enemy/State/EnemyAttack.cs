using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAttack : IFSM
{
    private float _attackCoolTime = 2f;
    private float _attackAngleRange = 30f;
    private float _attackRadius = 3f;

    private Damage _damage;

    private Enemy _enemy;

    private bool _isAttack = false;
    public EnemyAttack(Enemy enemy)
    {
        _enemy = enemy;
        Initialize();
    }

    private void Initialize()
    {
        _damage = new Damage(_enemy.EnemyData.DamagePower, _enemy.gameObject, _enemy.EnemyData.KnockbackPower);
        _attackCoolTime = _enemy.EnemyData.AttackCoolTime;
        _attackAngleRange = _enemy.EnemyData.AttackAngleRange;
        _attackRadius = _enemy.EnemyData.AttackRadius;
    }

    public void Start()
    {
        _enemy.Agent.isStopped = true;
        _enemy.Agent.ResetPath();
        _isAttack = true;
        _enemy.Animator.SetTrigger("Attack");
        Debug.Log("Attack");

    }

    public EEnemyState Update()
    {
        if(_isAttack)
        {
            return EEnemyState.Attack;
        }

        if (_enemy.TryAttack() == false) // 상대를 못찾으면 
        {
            Debug.Log("Attack -> Trace");
            return EEnemyState.Trace;
        }

        return EEnemyState.Attack;
    }
    public void End()
    {
        _isAttack = false;
        _enemy.Agent.isStopped = false;
    }

    private bool AttackDirection() // 부채꼴 범위 공격 - 범위 공격 생각 중
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(_enemy.transform.position, _attackRadius * 2);

        foreach (Collider collider in colliders)
        {
            Vector3 interV = (collider.transform.position - _enemy.transform.position).normalized;

            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV, _enemy.transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= _attackAngleRange / 2f)
            {
                return true;
            }
        }

        return false;
    }
   
}
