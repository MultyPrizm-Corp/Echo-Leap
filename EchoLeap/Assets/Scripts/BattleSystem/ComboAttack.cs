using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum AttackType
{
    basic,
    heavy,
    skill
}

public class ComboState
{
    public string comboName;
    public int attackPhase;
}

public class ComboAttack : ScriptableObject, IComboAttack
{
    [Header("General")]
    [SerializeField] private string comboName;
    [SerializeField] private List<AttackType> combinationRequired;
    [SerializeField] private float attackDelay = 0;

    [Header("Animator")]
    [SerializeField] private string attackAnimatorName;

    [Header("Stats")]
    [SerializeField] protected int damage;

    public string GetName()
    {
        return comboName;
    }

    public string GetAttackAnimatorName()
    {
        return attackAnimatorName;
    }

    public float GetAttackDelay()
    {
        return attackDelay;
    }

    public (ComboState, bool, float) ActivateCombo(List<string> combo, AttackControlPackage package)
    {
        List<string> _combinationRequired = new List<string>();

        foreach (AttackType i in combinationRequired)
        {
            _combinationRequired.Add(i.ToString());
        }

        if (_combinationRequired.SequenceEqual(combo))
        {
            ComboState comboState = StartCombo(package);

            comboState.comboName = comboName;

            return (comboState, true, attackDelay);
        }
        else
        {
            return (new ComboState(), false, 0f);
        }
    }

    protected virtual ComboState StartCombo(AttackControlPackage package)
    {
        return new ComboState();
    }

    public virtual (ComboState, bool) ContinueCombo(ComboState state, HitControlPackage package)
    {
        return (new ComboState(), false);
    }
}
