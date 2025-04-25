using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class Drum : MonoBehaviour,IDamageAble
{
    public GameObject ExplosionPrefab;

    public float Health = 100;
    public int Damage = 10;
    public float DestaroyTime = 2f;
    public float ForcePower = 200;

    private Damage _damage;

    private Rigidbody _rigidbody;

    private bool _isExplosition = false;

    private void Awake()
    {
        _damage = new Damage(Damage, this.gameObject,300);
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        if(Health <= 0 && _isExplosition == false)
        {
            _isExplosition = true;
            StartCoroutine( Explosion());
        }
    }

    IEnumerator Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);

        foreach(Collider collider in colliders)
        {
            IDamageAble damaged = collider.GetComponent<IDamageAble>();
            if (damaged != null)
            {
                if(collider.TryGetComponent<Drum>(out Drum durm) && durm != this)
                {
                    collider.GetComponent<Drum>().StartCoroutine(Explosion());
                }
                else
                {
                    damaged.TakeDamage(_damage);
                }
            }

        }

        _rigidbody.AddForce(Vector3.up * ForcePower , ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.one);
        GameObject explosion = Instantiate(ExplosionPrefab);
        explosion.transform.position = transform.position;

        yield return new WaitForSeconds(DestaroyTime);
        Destroy(gameObject);
        yield break;
    }

}
