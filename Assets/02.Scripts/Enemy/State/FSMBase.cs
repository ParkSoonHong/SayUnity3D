using UnityEngine;


public interface IFSM
{
    public void Start();
    public EEnemyState Update();
    public void End();
}

public abstract class FSMBase : MonoBehaviour 
{
    // 공통 상태 전환 로직 ?
    public virtual void Attack() { /* 기본 공격 */ }
    public virtual void Idle() { /* 기본 대기 */ }
    public virtual void Move() { /* 기본 이동 */ }
    public virtual void Damage() { /* 기본 피격 처리 */ }
    
}