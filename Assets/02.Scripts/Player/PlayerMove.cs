using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMove 
{
    public float MoveSpeed = 7;
    public float MaxRunSpeed = 12;
    public float RunAcceleration = 5f;
    private float _currentMoveSpeed;

    private Player _player;

    public float MoveActionStaminaAmount = 0.5f;

    private const float GRAVITY = -9.8f; // 중력

    public PlayerMove (Player player )
    {
        _player = player;
        Initialize();
    }

    private void Initialize()
    {
        MoveSpeed = _player.PlayerData.MoveSpeed;
        _currentMoveSpeed = MoveSpeed;

        MaxRunSpeed = _player.PlayerData.MaxRunSpeed;
        RunAcceleration = _player.PlayerData.RunAcceleration;

        MoveActionStaminaAmount = _player.PlayerData.MoveActionStaminaAmount;
    }

    public void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_player.UseStamina(MoveActionStaminaAmount)) // 스테미나가 있으면 달리고 없으면 멈춘다.
            {
                if (MoveSpeedChek(_currentMoveSpeed))
                {
                    _currentMoveSpeed += RunAcceleration * Time.deltaTime;
                }
            }
            else // 스테미나가 없으면 멈춘다.
            {
                if (MoveSpeedChek(_currentMoveSpeed))
                {
                    _currentMoveSpeed -= RunAcceleration * Time.deltaTime;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) // 달리기 멈추기
        {
            _currentMoveSpeed = MoveSpeed;
            _player.StartRecoveryStamina();
        }
    }

    public bool MoveSpeedChek(float currentMoveSpeed)
    {
        if (currentMoveSpeed >= MoveSpeed && MaxRunSpeed >= currentMoveSpeed)
        {
            return true;
        }
        return false;
    }

    public void Move()
    {
    
        // 1) 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // 2) 카메라 기준 이동 방향 변환
        Vector3 moveDir = Camera.main.transform.TransformDirection(inputDir);
        moveDir.y = 0;

        Run();

        // 4) 중력 처리
        if (_player.CharacterController.isGrounded && _player.YVelocity < 0)
        {
            _player.YVelocity = -2f;
        }
        _player.YVelocity += GRAVITY * Time.deltaTime;

        // 5) 최종 이동 벡터
        Vector3 velocity = moveDir * _currentMoveSpeed;
        velocity.y = _player.YVelocity;
        _player.CharacterController.Move(velocity * Time.deltaTime);

        // 6) 애니메이터 MoveSpeed 파라미터 세팅
        //    이동 입력이 없으면 0, 있으면 실제 속도의 크기 (Run 기준 11)
        float horizontalSpeed = new Vector3(_player.CharacterController.velocity.x, 0, _player.CharacterController.velocity.z).magnitude;
     
        _player.BaseAnimator.SetFloat("MoveSpeed", horizontalSpeed);
    }
    /*
    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");

    Vector3 dir = new Vector3(h, 0, v);
    dir = dir.normalized;
    // 메인 카메라를 기준으로 방향을 변환한다.
    dir = Camera.main.transform.TransformDirection(dir);
    // TransformDirection : 로컬 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수.
    // 3. 중력적용
    _player.YVelocity += GRAVITY * Time.deltaTime;
    dir.y = _player.YVelocity;
    if (h > 0 || v > 0 )
    {
        _player.BaseAnimator.SetFloat("MoveSpeed", _currentMoveSpeed);
        Debug.Log(_currentMoveSpeed);
    }
    else
    {
        _player.BaseAnimator.SetFloat("MoveSpeed", 0);

    }
    _player.CharacterController.Move(dir * _currentMoveSpeed * Time.deltaTime);
    */


    public void ClimbingMove()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0);
        dir = dir.normalized;
        dir.z = 0;
        _player.YVelocity = 0;
        _player.CharacterController.Move(dir * _currentMoveSpeed * Time.deltaTime);
    }


}

