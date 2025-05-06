using UnityEditor.PackageManager;
using UnityEngine;

public class WeaponColider : MonoBehaviour
{

    public Damage Damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageAble>(out IDamageAble damageAble)) // true 받으면 out에 집어 넣어준다.
        {
            damageAble.TakeDamage(Damage);
        }
    }
}
