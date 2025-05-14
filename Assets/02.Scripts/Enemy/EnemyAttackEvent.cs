using UnityEngine;

public class EnemyAttackEvent : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyController _enemyController;

    public void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
        _enemyController = GetComponentInParent<EnemyController>();
    }
    public void OnAttackHit()
    {
        // 실제 공격 판정
        Collider[] hits = Physics.OverlapSphere(_enemy.transform.position, _enemy.EnemyData.AttackDistance);
        foreach (var c in hits)
            if (c.TryGetComponent<IDamageAble>(out var dmg) && c.CompareTag("Player"))
            {
                dmg.TakeDamage(new Damage(_enemy.EnemyData.DamagePower, _enemy.gameObject, _enemy.EnemyData.KnockbackPower));
                break;
            }
    }

    public void OnAttackEnd()
    {
        // 애니메이션이 끝났다!
        _enemyController.EndAnim(EEnemyState.Trace);
    }
}
