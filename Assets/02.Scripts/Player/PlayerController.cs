using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    private PlayerMove _playerMove;

    private PlayerAction _playerAction;

    private PlayerAttack _playerAttack;

    private PlayerSkill _playerSkill;

    private PlayerRotate _playerRotate;

    private bool _isClimbing = false;

    private bool _isEvent = false;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerMove = new PlayerMove(_player);
        _playerAction = new PlayerAction(_player);
        _playerAttack = new PlayerAttack(_player);
        _playerSkill = new PlayerSkill(_player);
        _playerRotate = new PlayerRotate(_player);
    }

    private void Update()
    {
        if (_isEvent) return;

        _playerRotate.Rotate();

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(EventHandler(1));
            _playerAttack.Attack(_player.CharacterType);
            return;
        }
        // move 를 하다가 벽과 부닿으면 ClimbingMove로 전환
        // 스테미나를 체크해서 달리기
        // 스테미나를 체크해서 구르기
        // 스테미나를 체크해서 벽타기

        _playerAction.Jump();
        Climbing();

        if (!_isClimbing)
        {
            _playerMove.Move();
            _playerAction.Roll();
        }

        CharacterSwap();

    }

    public void Climbing()
    {
        if ((_player.CharacterController.collisionFlags & CollisionFlags.Sides) != 0 && _player.UseStamina(_playerMove.MoveActionStaminaAmount))
        {
            if (!_isClimbing)
            {
                _isClimbing = true;
                _player.YVelocity = 0;
            }

            // 클라이밍 중에는 일정 간격으로 스태미나 소비
            if (_isClimbing && _player.UseStamina(_playerMove.MoveActionStaminaAmount))
            {
                _playerMove.ClimbingMove();
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
            _player.StartRecoveryStamina();
        }
    }

    private void CharacterSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _player.PlayerSwap(CharacterType.Tanjiro);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _player.PlayerSwap(CharacterType.Nezuko);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
    }
    private IEnumerator EventHandler(float waitTime)
    {
        _isEvent = true;
        CameraFollow.Instance.isEvent = _isEvent;

        yield return new WaitForSeconds(waitTime);
 
        _isEvent = false;
        CameraFollow.Instance.isEvent = _isEvent;
        yield break;
    }

}
