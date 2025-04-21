using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAction : MonoBehaviour
{
    //  목표 : wasd를 누르면 캐릭터를 카메라 방향에 맞게 이동시키고 싶다.

    public float MoveSpeed = 7;
    public float MaxRunSpeed = 12;
    public float RunAcceleration = 5f;
    public float JumpPower = 5;

    public float RollTimer = 1;

    public float MaxStamina = 10;
    public float RecoverySpeed = 1;
    public float RunStaminaAmount = 0.5f;

    public int MaxJumpCount = 2;

    private float _stamina;

    private float _timar = 0;

    private float _currentMoveSpeed;
    private int _jumpCount;

    private const float GRAVITY = -9.8f; // 중력
    private float _yVelocity = 0f;  // 중력 가속도

    private bool _isJumping = false;
    private bool _isRun = false;
    private bool _isRecovery = false;
    private bool _isRoll = false;

    private Player _player; 

    private void Awake()
    {
        _player = GetComponent<Player>();
         _currentMoveSpeed = MoveSpeed;
        _stamina = MaxStamina;
        _timar = 0;
    }

    // 구현 순서:
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    private void Update()
    {

        if (_isRecovery)
        {
            recoveryStamina();
        }
        if (_player.CharacterController.collisionFlags == CollisionFlags.Sides)
        {
            ClimbingMove();
            return;
        }

        Roll();
        Jump();
        Run();
        Move();

    }

    public void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isRun = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRun = false;
        }

        if (_isRun)
        {
            if (_currentMoveSpeed < MaxRunSpeed)
            {
                _currentMoveSpeed += RunAcceleration * Time.deltaTime;
                _isRecovery = false;
                _stamina -= RunStaminaAmount;
                UI_Manager.Instance.UpdatePlayerStamina(_stamina / MaxStamina);
            }
        }
        else
        {
            if (_currentMoveSpeed > MoveSpeed)
            {
                _isRecovery = true;
                _currentMoveSpeed -= RunAcceleration * Time.deltaTime;
            }
        }
    }

    public void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        // 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        // TransformDirection : 로컬 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수.
        // 3. 중력적용
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;


        _player.CharacterController.Move(dir * _currentMoveSpeed * Time.deltaTime);
    }

    public void ClimbingMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0);
        dir = dir.normalized;
        dir.z = 0;
        // 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        // TransformDirection : 로컬 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수.
        _player.CharacterController.Move(dir * _currentMoveSpeed * Time.deltaTime);
    }
    public void Jump()
    {
        // 캐릭터가 땅 위에 있다면
        if (_player.CharacterController.isGrounded)
        {
            _isJumping = false;
            _jumpCount = MaxJumpCount;
        }

        // 3. 점프 적용
        if (Input.GetButtonDown("Jump") && _isJumping == false)
        {
            _yVelocity = JumpPower;
            _jumpCount--;

            if (_jumpCount <= 0)
            {
                _isJumping = true;
            }
        }
    }
    public void recoveryStamina()
    {
        if (_stamina >= MaxStamina)
        {
            _stamina = MaxStamina;
            return;
        }
        _stamina += RecoverySpeed;
        UI_Manager.Instance.UpdatePlayerStamina(_stamina / MaxStamina);
    }

    public void Roll()
    {

        if (Input.GetKeyDown(KeyCode.E) && _isRoll == false)
        {
            _isRoll = true;
        }

        if (_isRoll == false) return;
        _timar += Time.deltaTime;
        Debug.Log("Roll");
        Vector3 dir = new Vector3(transform.position.x * Time.deltaTime * MoveSpeed, transform.position.y);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        _player.CharacterController.Move(dir * 10 * Time.deltaTime);
        if (_timar >= RollTimer)
        {
            _timar = 0;
            _isRoll = false;
        }
    }


}
