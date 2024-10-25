using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Battle System/WeaponConfig")]
public class WeaponConfig : IWeapon
{
    [Header("Config")]
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite sprite;

    [Header("Skills")]
    [SerializeField] private AttackConfig basicAttack;
    [SerializeField] private AttackConfig heavyAttack;
    [SerializeField] private AttackConfig skillAttack;

    [Header("Stats")]
    [SerializeField] private int basicDamage;
    [SerializeField] private int heavyDamage;
    [SerializeField] private int skillDamage;
    [SerializeField] private string element;
    
    public override string GetName()
    {
        return weaponName;
    }

    public override void Attack(AttackControlPackage package)
    {
        switch (package.type) 
        {
            case "right":
                StartAttack(basicAttack, basicDamage, package);
                break;

            case "left":
                StartAttack(heavyAttack, heavyDamage, package);
                break;

            case "skill":
                StartAttack(skillAttack, skillDamage, package);
                break;
        }
    }

    public override void Hit(HitControlPackage package)
    {
        switch (package.type)
        {
            case "right":
                StartHit(basicAttack, package, basicDamage);
                break;

            case "left":
                StartHit(heavyAttack, package, heavyDamage);
                break;

            case "skill":
                StartHit(skillAttack, package, skillDamage);
                break;
        }
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
