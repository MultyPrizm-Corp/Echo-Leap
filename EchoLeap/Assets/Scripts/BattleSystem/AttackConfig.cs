using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackConfig : ScriptableObject, IAttack
{
    [SerializeField] private string animatorName = "null";
    [SerializeField] private float attackDelay = 0;

    public virtual void Attack(AttackControlPackage package)
    {

    }

    public virtual void Hit(HitControlPackage package)
    {

    }

    public string GetAttackAnimator()
    {
        return animatorName;
    }

    public float GetAttackDelay()
    {
        return attackDelay;
    }
}
