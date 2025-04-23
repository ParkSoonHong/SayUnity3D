using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;


public class PlayerFire : MonoBehaviour
{
    private Player _player;
    private LineRenderer bulletLineRenderer;

    public GameObject FirePosition;

    public GameObject BombPrefab;
    public GameObject TracerPrefab;
    private List<GameObject> _bombs;

    private float _inithrowPower = 15f;
    private float _throwPower;
    private float _throwPoweAccretionr = 10;

    private Camera _mainCamera; // 캐싱을 해라

    private int _bulletCount;
    private int _bombCount;

    public int Damage = 10;

    public ParticleSystem BulletEffectPrefab;

    public float FireCoolTime = 0.2f;
    public float ReLoadCoolTime = 2f;
    private float _timar = 0;

    private bool _isReLoad = false;
    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;

        _player = GetComponent<Player>();
        _bombCount = _player.MaxBombCount;
        _bulletCount = _player.MaxBulletCount;
        _throwPower = _inithrowPower;
        BombPool();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _mainCamera = Camera.main;
        UI_Manager.Instance.UpdateBomb(_bombCount, _player.MaxBombCount);
        UI_Manager.Instance.UpdateBullet(_bulletCount, _player.MaxBulletCount);
    }

    private void Update()
    {
        Bomb();

        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if(Input.GetMouseButton(0))
        {
            _timar += Time.deltaTime;
            if (FireCoolTime <= _timar)
            {
                Fire();
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && !_isReLoad)
        {
            _isReLoad = true;
            UI_Manager.Instance.ReLodingText();
        }

        if(_isReLoad)
        {
            _timar += Time.deltaTime;
        }

        if (ReLoadCoolTime <= _timar)
        {
            ReLoad();
        }

        // 목표 : 마우스의 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총알을 발사하고 싶다.
        //1. 마우스 왼쪽 버튼 입력 받기
        //3. 레이와 부딛힌 물체의 정보를 저장할 변수를 생성
        //4. 레이를 발사한 다음,                   ㄴ에 데잍터가 있다면(부딫혀있으면) 피격 이펙트 생성(표시) 물체가 충돌하면 피격 이펙트 생성하기.
        //2. 레이를 생성하고 발사 위치와 진행 방향을 설정

            // Ray : 레이저(시작위치,방향)
            // RayCast : 레이저를 발사
            // RayCastHit : 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체

    }

    public void ReLoad()
    {
        _bulletCount = 50;
        UI_Manager.Instance.UpdateBullet(_bulletCount, _player.MaxBulletCount);
    }

    public void BombPool()
    {
        _bombs = new List<GameObject>(_player.MaxBombCount);
        for (int i = 0; i < _player.MaxBombCount; i++)
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
            if (_throwPower >= _player.MaxThroPower) return;
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
            UI_Manager.Instance.UpdateBomb(_bombCount, _player.MaxBombCount);
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

            UI_Manager.Instance.UpdateBullet(_bulletCount, _player.MaxBulletCount);
            _timar = 0;
            if(hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();

                Damage damage = new Damage();
                damage.Value = Damage;
                damage.From = this.gameObject;

                enemy.TakeDamage(damage);

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

        // 파이어 포지션과 카메라 위치를 동시에 위로 조금씩 올리다가 일정 수 까지 올라가면 멈추고 양옆으로 발사

    }

    public bool BulletChek() 
    {
        if(_bulletCount <= 0)
        {
            _isReLoad = false;
            _timar = 0;
            UI_Manager.Instance.UpdateBullet(_bulletCount, _player.MaxBulletCount);
            return false;
        }
        return true;
    }

    public void recoil()
    {

    }
}
