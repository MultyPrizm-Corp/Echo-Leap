using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicCombo", menuName = "Battle System/Combos/BasicCombo")]
public class BasicCombo : ComboAttack
{
    [SerializeField] private int attackRadius;
    [SerializeField] private string animationAttackName;
    [SerializeField] private string animationPlayerName;
    [SerializeField] private string attackFloatStateName;
    [SerializeField] private AudioClip audioClip;
    
    protected override ComboState StartCombo(AttackControlPackage package)
    {
        package.playerAnimator.SetFloat(attackFloatStateName, 1f);
        package.playerAnimator.SetBool(animationPlayerName, true);
        package.attackAnimator.SetBool(animationAttackName, true);

        return new ComboState();
    }

    public override (ComboState, bool) ContinueCombo(ComboState state, HitControlPackage package)
    {
        package.playerAnimator.SetFloat(attackFloatStateName, 0f);
        Debug.Log("Combo!!!");
        return (new ComboState(), false);
    }
}
