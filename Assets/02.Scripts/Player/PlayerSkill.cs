using UnityEngine;

public class PlayerSkill 
{
    private Player _player;

    private Damage _damage;

    public PlayerSkill(Player player)
    {
        _player = player;
        Initialize();
    }

    private void Initialize()
    {
        _damage = new Damage(_player.PlayerData.Damage, _player.gameObject, _player.PlayerData.KnockBackpower);
    }


    public void Skill(ECharacterType characterType)
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

    private void NezukoAttack()
    {
        _player.BaseAnimator.SetTrigger("Skill");
        foreach (Collider collider in _player.AttackMonsters)
        {
            if (collider.TryGetComponent<IDamageAble>(out IDamageAble damageAble))
            {
                damageAble.TakeDamage(_damage);
            }
        }
    }

    private void TanjiroAttack()
    {

    }
}
