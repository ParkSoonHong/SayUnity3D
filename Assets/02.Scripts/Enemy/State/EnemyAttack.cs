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
        // 애니메이션 실행
    }

    public EEnemyState Update()
    {
         // 공격 애니메이션 실행 중에는 이동하면 안된다. -> 공격 실행중이라면 return 

        if(_isAttack)
        {
            return EEnemyState.Attack;
        }

        if (_enemy.TryAttack() == false || AttackDirection() == false) // 상대를 못찾으면 
        {
            Debug.Log("Trace");
            return EEnemyState.Trace;
        }

        return EEnemyState.Attack;
    }
    public void End()
    {
        _isAttack = false;
        _enemy.Agent.isStopped = false;
        _enemy.StopCoroutine(Attack_Coroutine());
    }

    // 시간 만큼 기다렸다가 공격 체크
    // 공격 애니메이션이 끝나면 이벤트 발생
    // 발생된 이벤트를 통해 어택 종료 -> 스테이트 변경
    // Start에서 애니메이션 실행 하고 End에만 반환
    public IEnumerator Attack_Coroutine()
    {
        _isAttack = true;

        Collider[] Colliders = Physics.OverlapSphere(_enemy.transform.position, _enemy.AttackDistance);
        foreach (Collider collider in Colliders)
        {
            if (collider.TryGetComponent<IDamageAble>(out IDamageAble damageAble) && collider.CompareTag("Player"))
            {
                damageAble.TakeDamage(_damage);
                break;
            }
        }
        yield return new WaitForSeconds(_attackCoolTime);

        _isAttack = false;
        yield break;
    }

    private bool AttackDirection()
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

    public void OnEnterAnimation()
    {
        // 애니메이션이 실제로 재생되기 시작할 때
        _isAttack = true;
    }

    // StateMachineBehaviour 종료 시 호출
    public void OnExitAnimation()
    {
        // 애니메이션이 끝났을 때
        _isAttack = false;
    }
}
