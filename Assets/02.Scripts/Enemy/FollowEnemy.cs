using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FollowEnemy : Enemy
{
  
    void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Trace:
                {
                    Trace();
                    break;
                }
           
            case EnemyState.Attack:
                {
                    Attack();
                    break;
                }
        }
    }

    private new void Trace()
    {
    
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {

            SetState(EnemyState.Attack);
            return;
        }

        _agent.SetDestination(_player.transform.position);

    }
}
