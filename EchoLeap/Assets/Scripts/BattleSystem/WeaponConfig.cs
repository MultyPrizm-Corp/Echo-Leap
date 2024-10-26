using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Battle System/WeaponConfig")]
public class WeaponConfig : IWeapon, IComboAttack
{
    [Header("Config")]
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite sprite;

    [Header("Basic Attack")]
    [SerializeField] private AttackConfig basicAttack;
    [SerializeField] private int basicDamage;
    [SerializeField] private int basicAttackStaminaCost;

    [Header("Heavy Attack")]
    [SerializeField] private AttackConfig heavyAttack;
    [SerializeField] private int heavyDamage;
    [SerializeField] private int heavyAttackStaminaCost;

    [Header("Skill Attack")]
    [SerializeField] private AttackConfig skillAttack;
    [SerializeField] private int skillDamage;
    [SerializeField] private int skillAttackStaminaCost;

    [Header("Combos")]
    [SerializeField] private List<ComboAttack> comboList;

    [Header("Stats")]
    [SerializeField] private string element;
    
    public override string GetName()
    {
        return weaponName;
    }

    public override (int, float) Attack(AttackControlPackage package, int stamina)
    {
        switch (package.type) 
        {
            case "basic":
                if (stamina >= basicAttackStaminaCost)
                {
                    StartAttack(basicAttack, basicDamage, package);
                    return (basicAttackStaminaCost, basicAttack.GetAttackDelay());
                }
                break;

            case "heavy":
                if (stamina >= heavyAttackStaminaCost)
                {
                    StartAttack(heavyAttack, heavyDamage, package);
                    return (heavyAttackStaminaCost, heavyAttack.GetAttackDelay());
                }
                break;

            case "skill":
                if (stamina >= skillAttackStaminaCost)
                {
                    StartAttack(skillAttack, skillDamage, package);
                    return (skillAttackStaminaCost, skillAttack.GetAttackDelay());
                }
                break;
        }

        return (0, 0f);
    }

    public override void Hit(HitControlPackage package)
    {
        switch (package.type)
        {
            case "basic":
                StartHit(basicAttack, package, basicDamage);
                break;

            case "heavy":
                StartHit(heavyAttack, package, heavyDamage);
                break;

            case "skill":
                StartHit(skillAttack, package, skillDamage);
                break;
        }
    }

    public override (ComboState, bool, float) ActivateCombo(List<string> combo, AttackControlPackage package)
    {
        foreach (ComboAttack i in comboList)
        {
            if (package.animatorPull.ContainsKey(i.GetAttackAnimatorName()))
            {
                package.attackAnimator = package.animatorPull[i.GetAttackAnimatorName()];
            }
            else
            {
                Debug.LogWarning($"Weapon [ERROR]: Animator[{i.GetAttackAnimatorName()}] don`t found.");
            }

            (ComboState, bool, float) status;

            status = i.ActivateCombo(combo, package);

            if (status.Item2)
            {
                return (status.Item1, true, status.Item3);
            }
        }

        return (new ComboState(), false, 0f);
    }

    public override (ComboState, bool) ContinueCombo(ComboState state, HitControlPackage package)
    {
        foreach(ComboAttack i in comboList)
        {
            if(i.GetName() == state.comboName)
            {
                return i.ContinueCombo(state, package);
            }
        }

        return (new ComboState(), false);
    }

    private void StartAttack(AttackConfig attack, int damage, AttackControlPackage package)
    {
        string animatorName = attack.GetAttackAnimator();

        package.element = element;
        package.damage = damage;

        if (attack == null)
        {
            return;
        }

        if (package.animatorPull.ContainsKey(animatorName))
        {
            package.attackAnimator = package.animatorPull[animatorName];
            attack.Attack(package);
        }
        else if (animatorName == "null")
        {
            attack.Attack(package);
        }
        else
        {
            Debug.LogError($"Weapon [ERROR]: Animator[{animatorName}] don`t found.");
        }
    }

    private void StartHit(AttackConfig attack, HitControlPackage package, int damage)
    {
        if (attack != null)
        {
            package.element = element;
            package.damage = damage;
            attack.Hit(package);
        }
    }
}
