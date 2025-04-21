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

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
}
