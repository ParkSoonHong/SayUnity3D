using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun,
    melee,
}

public class PlayerFire : MonoBehaviour // 나중에 이름 변경 Attack으로
{
    private Player _player;
    private LineRenderer bulletLineRenderer;

    public GameObject FirePosition;

    public GameObject BombPrefab;
    public GameObject TracerPrefab;
    private List<GameObject> _bombs;

    // 파이어 관련
    private int _maxBombCount = 3;
    private int _maxBulletCount = 50;
    private float _maxThroPower = 30;

    private float _inithrowPower = 15f;
    private float _throwPower;
    private float _throwPoweAccretionr = 10;

    private Camera _mainCamera; // 캐싱을 해라

    private int _bulletCount;
    private int _bombCount;

    public int Damage = 10;
    public int WeaponKnockback = 200;

    public ParticleSystem BulletEffectPrefab;

    public float FireCoolTime = 0.2f;
    public float ReLoadCoolTime = 2f;
    private float _timar = 0;

    private bool _isReLoad = false;

    public float angleRange = 30f;
    public float radius = 3f;

    private WeaponType _weaponType = WeaponType.Gun;
    private Damage _damage;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;

        _player = GetComponent<Player>();
        _bombCount = _maxBombCount;
        _bulletCount = _maxBulletCount;
        _throwPower = _inithrowPower;
        BombPool();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _mainCamera = Camera.main;
        UI_Manager.Instance.UpdateBomb(_bombCount, _maxBombCount);
        UI_Manager.Instance.UpdateBullet(_bulletCount, _maxBulletCount);

        _damage = new Damage(Damage, this.gameObject, WeaponKnockback);
    }

    private void Update()
    {
        Bomb();

        WeaponSwap();

        if (Input.GetMouseButtonDown(0))
        {
            switch (_weaponType)
            {
                case WeaponType.Gun:
                    {
                        Fire();
                        break;
                    }
                case WeaponType.melee:
                    {
                        MeleeAttack();
                        break;
                    }
            }
        }

        if(Input.GetMouseButton(0))
        {
            switch (_weaponType)
            {
                case WeaponType.Gun:
                    {
                        _timar += Time.deltaTime;
                        if (FireCoolTime <= _timar)
                        {
                            Fire();
                        }
                        break;
                    }
                case WeaponType.melee:
                    {
                        _timar += Time.deltaTime;
                        if (FireCoolTime <= _timar)
                        {
                            MeleeAttack();
                        }
                        break;
                    }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R) && !_isReLoad)
        {
            _isReLoad = true;
            UI_Manager.Instance.ReLodingText();
        }

        if (_isReLoad)
        {
            _timar += Time.deltaTime;
        }

        if (ReLoadCoolTime <= _timar)
        {
            ReLoad();
        }
    }

    private void WeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _weaponType = WeaponType.Gun;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _weaponType = WeaponType.melee;
        }
    }

    public void ReLoad()
    {
        _bulletCount = 50;
        UI_Manager.Instance.UpdateBullet(_bulletCount, _maxBulletCount);
    }

    public void BombPool()
    {
        _bombs = new List<GameObject>(_maxBombCount);
        for (int i = 0; i < _maxBombCount; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);
            _bombs.Add(bomb);
            bomb.SetActive(false);
        }
    }
    // 게임 수학 : 선형대수학(스칼라, 벡터, 행렬) , 기하학(삼각함수 ..)

    public void Bomb()
    {

        if (Input.GetMouseButton(1))
        {
            if (_throwPower >= _maxThroPower) return;
            _throwPower += Time.deltaTime * _throwPoweAccretionr;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_bombCount <= 0) return;
            // 3. 발사 위치에 수류탄 생성하기
            GameObject Bomb = null;
            foreach (GameObject bomb in _bombs)
            {
                if (bomb.activeInHierarchy == false && Bomb == null)
                {
                    Bomb = bomb;
                    break;
                }
            }
            Bomb.SetActive(true);
            Bomb.transform.position = FirePosition.transform.position;

            // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
            Rigidbody bombRigidbody = Bomb.GetComponent<Rigidbody>();
            bombRigidbody.AddForce(_mainCamera.transform.forward * _throwPower, ForceMode.Impulse);
            bombRigidbody.AddTorque(Vector3.one);

            _throwPower = _inithrowPower;
            _bombCount--;
            UI_Manager.Instance.UpdateBomb(_bombCount, _maxBombCount);
        }
    }
    public void Fire()
    {
        if (BulletChek() == false) return;
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);

        RaycastHit hitInfo = new RaycastHit();

        bool isHit = Physics.Raycast(ray, out hitInfo);
        if (isHit)
        {

            BulletEffectPrefab.transform.position = hitInfo.point;
            BulletEffectPrefab.transform.forward = hitInfo.normal; // 법선 벡터: 직선에 대하여 수직인 벡터
            BulletEffectPrefab.Play();
            _bulletCount--;
            StartCoroutine(ShotEffect(hitInfo.point));

            UI_Manager.Instance.UpdateBullet(_bulletCount, _maxBulletCount);
            _timar = 0;


            if (hitInfo.collider.TryGetComponent<IDamageAble>(out IDamageAble damageAble)) // true 받으면 out에 집어 넣어준다.
            {
                damageAble.TakeDamage(_damage);
            }


        }
    }

    // 근접 무기 구현
    // 내 앞 백터 * offset 까지 부채꼴 모양으로 범위 체크
    // 라이트 + 앞 백터 에서 

    public void MeleeAttack()
    {
        Collider[]
        colliders = Physics.OverlapSphere(transform.position, radius * 2);

        foreach (Collider collider in colliders)
        {
            Vector3 interV = (collider.transform.position - transform.position).normalized;

            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV, transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= angleRange / 2f && collider.TryGetComponent<IDamageAble>(out IDamageAble damageAble))
            {
                damageAble.TakeDamage(_damage);
            }
        }
    }
    IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, FirePosition.transform.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
        yield break;
        // 파이어 포지션과 카메라 위치를 동시에 위로 조금씩 올리다가 일정 수 까지 올라가면 멈추고 양옆으로 발사

    }

    public bool BulletChek()
    {
        if (_bulletCount <= 0)
        {
            _isReLoad = false;
            _timar = 0;
            UI_Manager.Instance.UpdateBullet(_bulletCount, _maxBulletCount);
            return false;
        }
        return true;
    }

    public void recoil()
    {

    }
}
