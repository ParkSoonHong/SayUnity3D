using System.Collections;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{

    public float RollTimer = 1;

    

    private float _stamina;

    private float _timar = 0;
    private float _currentMoveSpeed;
    private int _jumpCount;

    private const float GRAVITY = -9.8f; // 중력
    private float _yVelocity = 0f;  // 중력 가속도

    private bool _isJumping = false;
  
    private bool _isRecovery = false;
    private bool _isRoll = false;
    private bool _isClimbing = false;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
        Initialize();
    }

    private void Initialize()
    {
        _currentMoveSpeed = _player.MoveSpeed;
        _stamina = _player.MaxStamina;
        _timar = 0;
    }

    private void Update()
    {
      
        if (_isRecovery)
        {
            recoveryStamina();
        }

        Jump();
        Run();
        Climbing();

        if (!_isClimbing)
        {
            Move();
            Roll();
        }

    }
    public void Climbing()
    {
        if ((_player.CharacterController.collisionFlags & CollisionFlags.Sides) != 0 && UseStamina(_player.MoveActionStaminaAmount))
        {
            if (!_isClimbing)
            {
                _isClimbing = true;
                _yVelocity = 0; 
            }

            // 클라이밍 중에는 일정 간격으로 스태미나 소비
            if (_isClimbing && UseStamina(_player.MoveActionStaminaAmount))
            {
                ClimbingMove();
                return;
            }
            else
            {
                // 스태미나가 부족하면 클라이밍 중지
                _isClimbing = false;
            }
        }
        else
        {
            // 벽에서 떨어졌을 때 클라이밍 상태 해제
            _isClimbing = false;
            _isRecovery = true;
        }
    }
    public void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (UseStamina(_player.MoveActionStaminaAmount)) // 스테미나가 있으면 달리고 없으면 멈춘다.
            {
                if (MoveSpeedChek(_currentMoveSpeed))
                {
                    _currentMoveSpeed += _player.RunAcceleration * Time.deltaTime;
                }
            }
            else
            {
                if (MoveSpeedChek(_currentMoveSpeed))
                {
                    _currentMoveSpeed -= _player.RunAcceleration * Time.deltaTime;
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRecovery = true;
           
            _currentMoveSpeed = _player.MoveSpeed;
            
        }


    }

    public bool MoveSpeedChek(float currentMoveSpeed)
    {
        if(currentMoveSpeed >= _player.MoveSpeed && _player.MaxRunSpeed >= currentMoveSpeed)
        {
            return true;
        }

        return false;
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
        _yVelocity = 0;
        _player.CharacterController.Move(dir * _currentMoveSpeed * Time.deltaTime);
    }
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isClimbing == true)
        {
            _isRecovery = true;
            _isClimbing = false;
            return;
        }

        // 캐릭터가 땅 위에 있다면
        if (_player.CharacterController.isGrounded)
        {
            _isJumping = false;
            _jumpCount = _player.MaxJumpCount;
        }

        // 3. 점프 적용
        if (Input.GetButtonDown("Jump") && _isJumping == false)
        {
            _yVelocity = _player.JumpPower;
            _jumpCount--;

            if (_jumpCount <= 0)
            {
                _isJumping = true;
            }
        }
    }
    public void recoveryStamina()
    {
        if (_stamina >= _player.MaxStamina)
        {
            _stamina = _player.MaxStamina;
            return;
        }
        _stamina += _player.StaminaRecoverySpeed;
        UI_Manager.Instance.UpdatePlayerStamina(_stamina / _player.MaxStamina);
    }

    public void Roll()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isRoll == false)
        {
            if (!UseStamina(_player.RollStaminaAmout))
            {
                return;
            }
            _isRoll = true;
        }

        if (_isRoll == false) return;
        _timar += Time.deltaTime;
       
        Vector3 dir = new Vector3(transform.position.x * Time.deltaTime * _player.MoveSpeed, transform.position.y);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);

        _player.CharacterController.Move(dir * 10 * Time.deltaTime);
        
        if (_timar >= RollTimer)
        {
            _timar = 0;
            _isRecovery = true;
            _isRoll = false;
        }
    }

    public bool UseStamina(float StaminaAmount)
    {
        if(_stamina - StaminaAmount <= 0)
        {
            StartCoroutine(EndAction());
            return false;
        }

        _stamina -= StaminaAmount;
        UI_Manager.Instance.UpdatePlayerStamina(_stamina / _player.MaxStamina);
        _isRecovery = false;
        return true;
    }

    IEnumerator EndAction()
    {
        yield return new WaitForSeconds(1);
        _isRecovery = true;
    }
}
