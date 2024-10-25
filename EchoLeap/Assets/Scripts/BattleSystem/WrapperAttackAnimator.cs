using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperAttackAnimator : MonoBehaviour
{
    public string nameAnimator;
    public Animator animator;

    [SerializeField] private PlayerAttackController playerAttackController;

    public void HitBasicAttack()
    {
        playerAttackController.HitBasicAttack();
    }

    public void HitHeavyAttack()
    {
        playerAttackController.HitHeavyAttack();
    }

    public void HitSkillAttack()
    {
        playerAttackController.HitSkillAttack();
    }
}
