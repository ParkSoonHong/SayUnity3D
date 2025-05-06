using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public float RollSpeed = 7;

    public float MoveSpeed = 7;
    public float MaxRunSpeed = 12;
    public float RunAcceleration = 5f;

    public float JumpPower = 5;
    public int MaxJumpCount = 2;

    public float MaxStamina = 10;
    public float StaminaRecoverySpeed = 1;
    public float Maxhealth = 100;
 

    // 스테미나 감소량
    public float MoveActionStaminaAmount = 0.5f;
    public float RollStaminaAmout = 3f;

    // 기본 공격
    public float Damage = 10;
    public float KnockBackpower = 200;
}
