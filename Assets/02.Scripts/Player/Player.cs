using UnityEngine;

public struct PlayerState
{
    public float MoveSpeed;
    public float MaxRunSpeed;
    public float RunAcceleration;
    public float JumpPower;
}

public class Player : MonoBehaviour
{

    private CharacterController _characterController;
    public CharacterController CharacterController => _characterController;

    public float MoveSpeed = 7;
    public float MaxRunSpeed = 12;
    public float RunAcceleration = 5f;
    public float JumpPower = 5;
    public int MaxJumpCount = 2;
    public float MaxStamina = 10;
    public float StaminaRecoverySpeed = 1;

    // 사용하는 액션에 스테미나 감소량
    public float MoveActionStaminaAmount = 0.5f;
    public float RollStaminaAmout = 3f;

    // 파이어 관련
    public int MaxBombCount = 3;
    public int MaxBulletCount = 50;
    public float MaxThroPower = 30;

    [SerializeField] private PlayerSO _playerData;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Initialize();
    }

    private void Initialize()
    {
        MoveSpeed = _playerData.MoveSpeed;
        MaxRunSpeed = _playerData.MaxRunSpeed;
        RunAcceleration = _playerData.RunAcceleration;
        JumpPower = _playerData.JumpPower;
        MaxJumpCount = _playerData.MaxJumpCount;
        MaxStamina = _playerData.MaxStamina;
        StaminaRecoverySpeed = _playerData.StaminaRecoverySpeed;
        MoveActionStaminaAmount = _playerData.MoveActionStaminaAmount;
        RollStaminaAmout = _playerData.RollStaminaAmout;

        MaxBombCount = _playerData.BombCount;
        MaxBulletCount = _playerData.MaxBulletCount;
        MaxThroPower = _playerData.MaxThroPower;
    }
}
