using UnityEngine;

public class Damage 
{
    public float Value;
    public float KnockbackPower = 100;
    public GameObject From;

    public Damage(float value,  GameObject from , float knockbackPower = 0)
    {
        Value = value;
        KnockbackPower = knockbackPower;
        From = from;
    }
}

public interface IDamageAble
{
    public void TakeDamage(Damage damage);
}