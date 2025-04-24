using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSo", menuName = "Scriptable Objects/EnemyDataSo")]
public class EnemyDataSo : ScriptableObject
{
    public enum EnemyType
    {
        Nomal,
        Follow,
    }

    public int Health;
    public float MoveSpeed = 3.3f;
    public Damage Damage;

}
