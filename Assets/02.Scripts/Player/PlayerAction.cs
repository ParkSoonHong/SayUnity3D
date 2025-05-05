using System.Collections;
using UnityEngine;

public class PlayerAction 
{

    public float RollTimer = 1;
    private float _rollSpeed;

    private float _jumpPower = 5;
    private int _maxJumpCount = 2;
    private int _jumpCount;

    private float _timar = 0;

    private bool _isJumping = false;
    private bool _isRoll = false;
    private bool _isClimbing = false;

    // 사용하는 액션에 스테미나 감소량
    public float RollStaminaAmout = 3f;

    private Player _player;

    public PlayerAction(Player player)
    {
        _player = player;
        Initialize();
    }

    private void Initialize()
    {
        _timar = 0;
        _rollSpeed = _player.PlayerData.RollSpeed;
        _maxJumpCount = _player.PlayerData.MaxJumpCount;
        _jumpPower = _player.PlayerData.JumpPower;
    }


    public void Jump()
    {
        /*
        if (Input.GetButtonDown("Jump") && _isClimbing == true)
        {
            _isRecovery = true;
            _isClimbing = false;
            return;
        }
        */
        // 캐릭터가 땅 위에 있다면
        if (_player.CharacterController.isGrounded)
        {
            _isJumping = false;
            _jumpCount = _maxJumpCount;
        }

        // 3. 점프 적용
        if (Input.GetButtonDown("Jump") && _isJumping == false)
        {
            _player.YVelocity = _jumpPower;
            _jumpCount--;

            if (_jumpCount <= 0)
            {
                _isJumping = true;
            }
        }
    }

    public void Roll()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isRoll == false)
        {
            if (_player.UseStamina(RollStaminaAmout) == false)
            {
                return;
            }
            _isRoll = true;
           
        }

        if (_isRoll == false) return;

        _timar += Time.deltaTime;
       
        Vector3 dir = new Vector3(_player.transform.position.x * Time.deltaTime * _rollSpeed, _player.transform.position.y);
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
