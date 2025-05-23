using System.Collections.Generic;
using UnityEngine;



public class PlayerAttack
{
    private Player _player;


    private Camera _mainCamera; // 캐싱을 해라

    public int Damage = 10;
    public int WeaponKnockback = 200;

    public float angleRange = 60f;
    public float radius = 3f;

    private Damage _damage;

    private Collider[] _hitBuffer = new Collider[50];
    private float _radiusSq;
    private float _cosThreshold;

    public PlayerAttack(Player player)
    {
        _player = player;
        Initialize();
    }
    private void Initialize()
    {
        _damage = new Damage(_player.PlayerData.Damage, _player.gameObject, _player.PlayerData.KnockBackpower);
    }


    public void Attack(ECharacterType characterType)
    {
        switch (characterType)
        {
            case ECharacterType.Nezuko:
                NezukoAttack();
                break;
            case ECharacterType.Tanjiro:
                TanjiroAttack();
                break;
        }
    }

    public void NezukoAttack()
    {
        // 애니메이션 실행 풀링 사용,
        _player.BaseAnimator.SetTrigger("Attack");


        Collider[] colliders;
        colliders = Physics.OverlapSphere(_player.transform.position, radius * 2);

        foreach (Collider collider in colliders)
        {
            Vector3 interV = (collider.transform.position - _player.transform.position).normalized;

            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV, _player.transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= angleRange / 2f && collider.TryGetComponent<IDamageAble>(out IDamageAble damageAble) && _player.AttackMonsters.Contains(collider) == false)
            {
                _player.AttackMonsters.Add(collider);
                damageAble.TakeDamage(_damage);
            }
        }
    }

    public void TanjiroAttack()
    {
        _player.BaseAnimator.SetTrigger("Attack");

        int count = Physics.OverlapSphereNonAlloc(
        _player.transform.position,
        radius * 2,
        _hitBuffer
    );

        for (int i = 0; i < count; i++)
        {
            var col = _hitBuffer[i];
            Vector3 toTarget = col.transform.position - _player.transform.position;
            if (toTarget.sqrMagnitude > _radiusSq) continue;

            float dot = Vector3.Dot(
                toTarget.normalized,
                _player.transform.forward
            );
            if (dot < _cosThreshold) continue;

            if (col.TryGetComponent<IDamageAble>(out var dmg))
                dmg.TakeDamage(_damage);
        }
        // 애니메이션 실행,

        /*
        Collider[] colliders;
        colliders = Physics.OverlapSphere(_player.transform.position, radius * 2);

        foreach (Collider collider in colliders)
        {
            Vector3 interV = (collider.transform.position - _player.transform.position).normalized;

            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV, _player.transform.forward);
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
        */
        //  _isAttack = false;
    }

    /*
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

    
    public void ReLoad()
    {
        _bulletCount = 50;
        UI_Manager.Instance.UpdateBullet(_bulletCount, _maxBulletCount);
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
    }
    */
}