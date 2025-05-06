using Unity.Android.Gradle.Manifest;
using UnityEngine;
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
        Run();
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


        _player.CharacterController.Move(dir * _currentMoveSpeed * Time.deltaTime);
    }

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

