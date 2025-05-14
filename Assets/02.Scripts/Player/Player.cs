using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum ECharacterType
{
    Nezuko,
    Tanjiro,
    Count,
}

public class Player : MonoBehaviour , IDamageAble
{
    public Transform FPSCamPOS;
    public Transform TPSCamPOS;

    private CharacterController _characterController;
    public CharacterController CharacterController => _characterController;

    private  List<Animator> _tpsAnimators;
    private  List<Animator> _fpsAnimators;
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

    public ECharacterType CharacterType;

    public PlayerSO PlayerData;

    public List<GameObject> TPSPlayerModels;
    public List<GameObject> FPSPlayerModels;

    public int MaxMonsterCount = 50;
    public List<Collider> AttackMonsters;

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

        _tpsAnimators = new List<Animator>(TPSPlayerModels.Count);
        _fpsAnimators = new List<Animator>(FPSPlayerModels.Count);

        AttackMonsters = new List<Collider>(MaxMonsterCount);


        foreach (GameObject playerModel in TPSPlayerModels)
        {
            _tpsAnimators.Add(playerModel.GetComponent<Animator>());
        }

        foreach (GameObject playerModel in FPSPlayerModels)
        {
            _fpsAnimators.Add(playerModel.GetComponent<Animator>());
        }
        CameraModeManager.Instance.ModeHendleEvent += CamerModeCheck;

        PlayerSwap(ECharacterType.Nezuko);
    }

    public bool UseStamina(float StaminaAmount) // 스테미나를 사용 할수 있는지
    {
        if (_stamina - StaminaAmount <= 0)
        {
            return false;
        }

        _stamina -= StaminaAmount;
        UI_HUDManager.Instance.UpdatePlayerStamina(_stamina / _maxStamina);
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
                UI_HUDManager.Instance.UpdatePlayerStamina(_stamina / _maxStamina);
                _isRecovery = false;
                break;
            }

            _stamina += StaminaRecoverySpeed;
            UI_HUDManager.Instance.UpdatePlayerStamina(_stamina /_maxStamina);
        }

        yield break;
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        UI_HUDManager.Instance.UpdateHealth(_health/_maxhealth);
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

    public void PlayerSwap(ECharacterType characterType)
    {
        switch (characterType)
        {
            case ECharacterType.Nezuko:
                UI_HUDManager.Instance.UpdateSwap(characterType);
                PlayerViewCheck(characterType);
                _characterController.radius = 0.4f;
                _characterController.height = 1.55f;
               
                break;
            case ECharacterType.Tanjiro:
                PlayerViewCheck(characterType);
                UI_HUDManager.Instance.UpdateSwap(characterType);
                _characterController.radius = 0.4f;
                _characterController.height = 1.7f;
                break;
        }
        CharacterType = characterType;
    }

    private void CamerModeCheck(CameraMode cameraMode)
    {
        PlayerViewCheck(CharacterType);
    }

    private void PlayerViewCheck(ECharacterType characterType)
    {
        if(CameraModeManager.Instance.CurrentMode == CameraMode.FPS)
        {
            for(int i=0; i < (int)ECharacterType.Count; i++)
            {
                if(i == (int)characterType)
                {
                    TPSPlayerModels[i].SetActive(false);
                    FPSPlayerModels[i].SetActive(true);
                    BaseAnimator = _fpsAnimators[(int)characterType];
                    Debug.Log("FPS");
                    continue;
                }
                TPSPlayerModels[i].SetActive(false);
                FPSPlayerModels[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < (int)ECharacterType.Count; i++)
            {
                if (i == (int)characterType)
                {
                    FPSPlayerModels[i].SetActive(false);
                    TPSPlayerModels[i].SetActive(true);
                    BaseAnimator = _tpsAnimators[(int)characterType];
                    Debug.Log("TPS");
                    continue;
                }
                TPSPlayerModels[i].SetActive(false);
                FPSPlayerModels[i].SetActive(false);
            }
        }
    }

    // 과제용 추후 제거
    private int currentIndex = 0; // 0 = 첫 번째 무기
    void Update()
    {
        // 마우스 휠 입력 감지 (앞으로 굴리면 양수, 뒤로 굴리면 음수)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            // 휠 올리기 → 이전 무기로
            currentIndex = (currentIndex - 1 + (int)ECharacterType.Count) % (int)ECharacterType.Count;
            PlayerSwap((ECharacterType)currentIndex);
        }
        else if (scroll < 0f)
        {
            // 휠 내리기 → 다음 무기로
            currentIndex = (currentIndex + 1) % (int)ECharacterType.Count;
            PlayerSwap((ECharacterType)currentIndex);
        }
    }
}
