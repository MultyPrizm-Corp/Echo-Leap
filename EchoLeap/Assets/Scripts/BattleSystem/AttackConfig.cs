using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackConfig : ScriptableObject, IAttack
{
    [SerializeField] private string animatorName = "null";

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
}
