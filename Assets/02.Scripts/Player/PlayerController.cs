using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    private PlayerMove _playerMove;

    private PlayerAction _playerAction;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerMove = new PlayerMove(_player);
        _playerAction = new PlayerAction(_player);
    }

    private void Update()
    {
        // move 를 하다가 벽과 부닿으면 ClimbingMove로 전환
        // 스테미나를 체크해서 달리기
        // 스테미나를 체크해서 구르기
        // 스테미나를 체크해서 벽타기
        /*
        Jump();
        Climbing();

        if (!_isClimbing)
        {
            Move();
            Roll();
        }
        */
    }
}
