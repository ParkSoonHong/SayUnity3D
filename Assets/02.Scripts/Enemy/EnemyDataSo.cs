using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSo", menuName = "Scriptable Objects/EnemyDataSo")]
public class EnemyDataSo : ScriptableObject
{
    public enum EnemyType
    {
        Nomal, // 일반몹
        Elite, // 엘리트 몬스터
        Boss   // 보스 몬스터
    }

    [Header("스테이터스")]
    public int Health = 100;
    public float MoveSpeed = 3.3f;
    public float DamagePower = 10;
    public float KnockbackPower = 200;


    [Header("거리")]
    public float MovePointDistance = 0.1f; // 목적지와의 거리
    public float FindDistance = 7f;
    public float ReturnDistance = 5f;
    public float AttackDistance = 2f;

    [Header("순찰")]
    public int TotalPatrolCount = 3; // 몇개의 장소를 다닐것인지
    public float PatrolTime = 2f;

    public Vector3 PatrolMaxRange = new Vector3(10, 10, 0); // 순찰 최대 범위
    public Vector3 PatrolMinRange = new Vector3(-10, -10, 0); // 순찰 최소 범위

    [Header("죽음")]
    public float DeathTime = 1f;
}
