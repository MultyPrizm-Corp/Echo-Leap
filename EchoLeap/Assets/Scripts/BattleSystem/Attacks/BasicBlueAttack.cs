using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicBlueAttack", menuName = "Battle System/Attacks/BasicBlueAttack")]
public class BasicBlueAttack : AttackConfig
{
    [SerializeField] private int attackRadius;
    [SerializeField] private string animationAttackName;
    [SerializeField] private string animationPlayerName;
    [SerializeField] private AudioClip audioClip;

    public override void Attack(AttackControlPackage package)
    {
        package.playerAnimator.SetBool(animationPlayerName, true);
        package.attackAnimator.SetBool(animationAttackName, true);
    }

    public override void Hit(HitControlPackage package)
    {
        package.audioSource.PlayOneShot(audioClip);
        int playerLayer = LayerMask.GetMask("Player");
#if DEBUG
        Debug.DrawLine(package.attackPoint.transform.position, package.attackPoint.transform.position + (Vector3.right * attackRadius), Color.blue, 2f);
#endif
        RaycastHit2D hit;
        Vector2 target = new Vector2();

        if(package.playerSide == "left")
        {
            target = Vector2.left;
        }
        else if (package.playerSide == "right")
        {
            target = Vector2.right;
        }

        hit = Physics2D.Raycast(package.attackPoint.transform.position, target, attackRadius, ~playerLayer);

        if (hit.collider)
        {
            if (hit.collider.tag == "Enemy")
            {
                IHealth enemy = hit.collider.gameObject.GetComponent<IHealth>();

                if (enemy != null)
                {
                    enemy.Damage(package.damage, package.element);
                }
            }
        }
    }
}
