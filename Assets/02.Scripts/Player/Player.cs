using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public struct PlayerState
{
    public float MoveSpeed;
    public float MaxRunSpeed;
    public float RunAcceleration;
    public float JumpPower;
}

public class Player : MonoBehaviour , IDamageAble
{
    public Transform FPSCamPOS;
    public Transform TPSCamPOS;

    private CharacterController _characterController;
    public CharacterController CharacterController => _characterController;

    private  List<Animator> _animators;
    public Animator BaseAnimator;

    private float _maxStamina = 10;
    private float _stamina;
    public float Stamina => _stamina;
    public float StaminaRecoverySpeed = 1;

    public float YVelocity = 0f;  // 중력 가속도

    private float _maxhealth = 100;
    private float _health;
    public float Health => _health;

    private bool _isRecovery = false;

    public CharacterType CharacterType;

    public PlayerSO PlayerData;

    public List<GameObject> PlayerModels;
    private int _curruntModels = 0;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Initialize();
    }

    private void Initialize()
    {
        _maxhealth = PlayerData.Maxhealth;
        _health = _maxhealth;

        _maxStamina = PlayerData.MaxStamina;
        _stamina = _maxStamina;

        _curruntModels = (int)CharacterType;

        _animators = new List<Animator>(PlayerModels.Count);

        foreach(GameObject playerModel in PlayerModels)
        {
            _animators.Add(playerModel.GetComponent<Animator>());
        }

        PlayerSwap(CharacterType.Nezuko);
    }

    public bool UseStamina(float StaminaAmount) // 스테미나를 사용 할수 있는지
    {
        if (_stamina - StaminaAmount <= 0)
        {
            return false;
        }

        _stamina -= StaminaAmount;
        UI_Manager.Instance.UpdatePlayerStamina(_stamina / _maxStamina);
        _isRecovery = false;
        return true;
    }

    private IEnumerator RecoveryStamina(float waitTime)//스테미너 회복
    {
     

        yield return new WaitForSeconds(waitTime); // 1초뒤 회복 시작

        while (_isRecovery) // 현재 회복 중이라면
        {
            if (_stamina >= _maxStamina) // 현재가 최대를 넘어가면 
            {
                _stamina = _maxStamina;
                UI_Manager.Instance.UpdatePlayerStamina(_stamina / _maxStamina);
                _isRecovery = false;
                break;
            }

            _stamina += StaminaRecoverySpeed;
            UI_Manager.Instance.UpdatePlayerStamina(_stamina /_maxStamina);
        }

        yield break;
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        UI_Manager.Instance.UpdateHealth(_health/_maxhealth);
        if(_health <= 0)
        {
            // 죽음
        }
    }

    public void StartRecoveryStamina()
    {
        _isRecovery = true;
        StartCoroutine(RecoveryStamina(1));
    }

    public void PlayerSwap(CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterType.Nezuko:
                PlayerModels[_curruntModels].SetActive(false);
                PlayerModels[(int)characterType].SetActive(true);
                _characterController.radius = 0.4f;
                _characterController.height = 1.55f;
                BaseAnimator = _animators[(int)characterType];
                break;
            case CharacterType.Tanjiro:
                PlayerModels[_curruntModels].SetActive(false);
                PlayerModels[(int)characterType].SetActive(true);
                _characterController.radius = 0.4f;
                _characterController.height = 1.7f;
                BaseAnimator = _animators[(int)characterType];
                break;
        }
        _curruntModels = (int)characterType;
    }
}
