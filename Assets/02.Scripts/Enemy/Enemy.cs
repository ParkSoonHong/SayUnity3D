using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour
{

    private GameObject _player;
    public GameObject Player => _player;

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    private CharacterController _characterController;

    public EnemyDataSo EnemyData;
    public UI_Enemy UI_Enemy;

    public float FindDistance = 7f;
    public float ReturnDistance = 5f;
    public float AttackDistance = 2f;

    private float _maxHealth = 100;
    private float _health = 0;
    public float Helath => _health;

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        
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
