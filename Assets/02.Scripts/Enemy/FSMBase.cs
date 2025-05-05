using UnityEngine;

public interface IFSM
{
    public void Attack();
    public void Idle();
    public void Move();
    public void Damage();
}

public abstract class FSMBase : MonoBehaviour, IFSM
{
    // 공통 상태 전환 로직
    public virtual void Attack() { /* 기본 공격 */ }
    public virtual void Idle() { /* 기본 대기 */ }
    public virtual void Move() { /* 기본 이동 */ }
    public virtual void Damage() { /* 기본 피격 처리 */ }
}