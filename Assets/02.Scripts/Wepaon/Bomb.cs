using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표 : 마우스의 오른쪽 버튼을 누르면 카메라가 바라보는 방향으로 수류탄을 던지고 싶다.
    // 1. 수류탄 오브젝트 만들기

    private ParticleSystem _explosionEffect; 
    public GameObject ExplosionEffectPrefab;

    private void Awake()
    {
        GameObject Effect = Instantiate(ExplosionEffectPrefab);
        Effect.gameObject.SetActive(false);
        _explosionEffect = Effect.GetComponent<ParticleSystem>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        _explosionEffect.gameObject.SetActive(true);
        _explosionEffect.transform.position = transform.position;
        _explosionEffect.transform.forward = collision.transform.forward * -1;
        _explosionEffect.Play();
        gameObject.SetActive(false);
    }
}
