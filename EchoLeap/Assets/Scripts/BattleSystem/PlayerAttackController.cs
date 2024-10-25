using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComboAttack
{
    public ComboState Activate(Animator playerAnimator, GameObject player, Dictionary<string, Animator> animatorPull, List<string> combo);

    public ComboState ContinueAttack(ComboState state);
}

public interface IAttack
{
    public void Attack(AttackControlPackage package);

    public void Hit(HitControlPackage package);

    public string GetAttackAnimator();
}

public abstract class IWeapon : ScriptableObject
{
    public abstract void Attack(AttackControlPackage package);

    public abstract void Hit(HitControlPackage package);

    public abstract string GetName();
}

public struct AttackControlPackage
{
    public string type;
    public Animator playerAnimator;
    public GameObject player;
    public Dictionary<string, Animator> animatorPull;
    public Animator attackAnimator;
    public Dictionary<string, GameObject> specialObjects;
    public AudioSource audioSource;
    public int damage;
    public string element;
}

public struct HitControlPackage
{
    public string type;
    public GameObject player;
    public GameObject attackPoint;
    public Dictionary<string, GameObject> specialObjects;
    public AudioSource audioSource;
    public int damage;
    public string element;
    public string playerSide;
}

public struct ComboState
{
    public int attackPhase;
}

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Animators")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private List<WrapperAttackAnimator> effectAnimatorsPull = new List<WrapperAttackAnimator>();
    
    [Header("Battle Config")]
    [SerializeField, Space(5)] private List<IWeapon> weapons = new List<IWeapon>();
    [SerializeField] private string weaponEquip;

    [Header("Special")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject attackPoint;
    [SerializeField, Tooltip("Defalt: y=0 -> left; y=180 -> rigth")] private bool invertLeftAndRigth = false;
    [SerializeField] private List<GameObject> specialObjects = new List<GameObject>();
    [SerializeField] private bool debag;

    [Header("Permissions")]
    [SerializeField, Space(5)] private bool rigthAttack = true;
    [SerializeField] private bool leftAttack = true;
    [SerializeField] private bool skillAttack = true;
    [SerializeField] private bool dash = true;

    private Dictionary<string, Animator> animatorPullList;
    private Dictionary<string, IWeapon> weaponsList;
    private Dictionary<string, GameObject> specialObjectsList;
    private List<string> combo;

    private void Start()
    {
        animatorPullList = CompileAnimatorPullList(effectAnimatorsPull);
        weaponsList = CompileWeaponsList(weapons);
        specialObjectsList = CompileSpecialObjectsList(specialObjects);

#if DEBUG
        if(playerAnimator == null)
        {
            Debug.LogWarning("PlayerAttackController[Warning]: Player animator don`t found");
        }
        if (audioSource == null)
        {
            Debug.LogWarning("PlayerAttackController[Warning]: Audio source don`t found");
        }
        if (attackPoint == null)
        {
            Debug.LogWarning("PlayerAttackController[Warning]: Attack point don`t found");
        }
#endif
    }

#if DEBUG
    private void OnDrawGizmos()
    {
        if (attackPoint != null && debag)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(attackPoint.transform.position, attackPoint.transform.position + Vector3.right);
            Gizmos.DrawLine(attackPoint.transform.position, attackPoint.transform.position + Vector3.left);
        }
    }
#endif
    private Dictionary<string, GameObject> CompileSpecialObjectsList(List<GameObject> _specialObjects)
    {
        Dictionary<string, GameObject> weapons = new Dictionary<string, GameObject>();

        foreach (GameObject i in _specialObjects)
        {
            weapons.Add(i.name, i);
        }

        return weapons;
    }

    private Dictionary<string, IWeapon> CompileWeaponsList(List<IWeapon> _weapons)
    {
        Dictionary<string, IWeapon> weapons = new Dictionary<string, IWeapon>();

        foreach (IWeapon i in _weapons)
        {
            weapons.Add(i.GetName(), i);
        }

        return weapons;
    }

    private Dictionary<string, Animator> CompileAnimatorPullList(List<WrapperAttackAnimator> animatorWrappers)
    {
        Dictionary<string, Animator> animators = new Dictionary<string, Animator>();

        foreach(WrapperAttackAnimator i in animatorWrappers)
        {
            animators.Add(i.nameAnimator, i.animator);
        }

        return animators;
    }

    private string CheckPlayerSide()
    {
        if (player.transform.rotation.y == 0 || player.transform.rotation.y == 1 && invertLeftAndRigth)
        {
            return "left";
        }
        else if (player.transform.rotation.y == 1 || player.transform.rotation.y == 0 && invertLeftAndRigth)
        {
            return "right";
        }
        else
        {
            Debug.LogWarning("PlayerAttackController[Warning]: unforeseen rotate.");
        }

        return "";
    }

    private AttackControlPackage GetAttackControlPackage(string type)
    {
        AttackControlPackage package = new AttackControlPackage();

        package.type = type;
        package.playerAnimator = playerAnimator;
        package.animatorPull = animatorPullList;
        package.audioSource = audioSource;
        package.specialObjects = specialObjectsList;
        package.player = player;

        return package;
    }

    private HitControlPackage GetHitControlPackage(string type)
    {
        HitControlPackage package = new HitControlPackage();

        package.type = type;
        package.specialObjects = specialObjectsList;
        package.player = player;
        package.attackPoint = attackPoint;
        package.playerSide = CheckPlayerSide();
        package.audioSource = audioSource;

        return package;
    }

    public void BasicAttack()
    {
        weaponsList[weaponEquip].Attack(GetAttackControlPackage("right"));
    }

    public void HeavyAttack()
    {
        weaponsList[weaponEquip].Attack(GetAttackControlPackage("left"));
    }

    public void SkillAttack()
    {
        weaponsList[weaponEquip].Attack(GetAttackControlPackage("skill"));
    }

    public void HitBasicAttack()
    {
        weaponsList[weaponEquip].Hit(GetHitControlPackage("right"));
    }

    public void HitHeavyAttack()
    {
        weaponsList[weaponEquip].Hit(GetHitControlPackage("left"));
    }

    public void HitSkillAttack()
    {
        weaponsList[weaponEquip].Hit(GetHitControlPackage("skill"));
    }
}
