using UnityEngine;

public class AnimationToStatemachine : MonoBehaviour
{
    public IdleState idleState;
    public AttackState attackState;
    public DamagedState damageState;
    public ChargeState chargeState;
    public DeadState deadState;

    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }

    private void DamagedAnimationDone()
    {
        damageState.DamagedAnimationDone();
    }

    private void FinishCharge()
    {
        chargeState.FinishCharge();
    }

    private void FinishTransition()
    {
        idleState.FinishTransition();
    }

    private void FinishDead()
    {
        deadState.FinishDeadAnimation();
    }
}
