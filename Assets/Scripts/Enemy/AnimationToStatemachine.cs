using UnityEngine;

public class AnimationToStatemachine : MonoBehaviour
{
    public AttackState attackState;
    public DamagedState damageState;

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
}
