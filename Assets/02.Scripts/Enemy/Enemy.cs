using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Nomal, // 일반몹
    Elite, // 엘리트 몬스터
    Boss   // 보스 몬스터
}

public class Enemy : MonoBehaviour
{

    public GameObject Player;

    public NavMeshAgent Agent;

    private CharacterController _characterController;

    public EnemyDataSo EnemyData;
    public UI_Enemy UI_Enemy;
    // 애니메이션

    public float FindDistance = 7f;
    public float ReturnDistance = 5f;
    public float AttackDistance = 2f;

    private float _maxHealth = 100;
    private float _health = 0;
    public float Helath => _health;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        Agent = GetComponent<NavMeshAgent>();
      
        _health = _maxHealth;
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        UI_Enemy.UpdateHealth(_health/ _maxHealth);
    }

    public bool TryFindTarget()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < FindDistance)
        {
            return true;
        }
        return false;
    }

    public bool TryReturnPoint()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) > ReturnDistance)
        {
            return true;
        }
        return false;
    }

    public bool TryAttack()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < AttackDistance)
        {
            return true;
        }
        return false;
    }
    
    public bool TryEndPoint(Vector3 placePosition) // 도착 여부
    {
         if (Vector3.Distance(transform.position, placePosition) <= EnemyData.MovePointDistance)
        {
            transform.position = placePosition;
            
            return true;
        }
        return false;
    }
}
