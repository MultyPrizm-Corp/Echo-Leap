using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComboAttack
{
    public (ComboState, bool, float) ActivateCombo(List<string> combo, AttackControlPackage package);

    public (ComboState, bool) ContinueCombo(ComboState state, HitControlPackage package);
}


public interface IAttack
{
    public void Attack(AttackControlPackage package);

    public void Hit(HitControlPackage package);

    public string GetAttackAnimator();
}

public abstract class IWeapon : ScriptableObject
{
    public abstract (int, float) Attack(AttackControlPackage package, int stamina);

    public abstract void Hit(HitControlPackage package);

    public abstract (ComboState, bool, float) ActivateCombo(List<string> combo, AttackControlPackage package);

    public abstract (ComboState, bool) ContinueCombo(ComboState state, HitControlPackage package);

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
    public Animator playerAnimator;
    public GameObject attackPoint;
    public Dictionary<string, GameObject> specialObjects;
    public AudioSource audioSource;
    public int damage;
    public string element;
    public string playerSide;
}


public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Animators")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private List<WrapperAttackAnimator> effectAnimatorsPull = new List<WrapperAttackAnimator>();
    
    [Header("Battle Config")]
    [SerializeField] private List<IWeapon> weapons = new List<IWeapon>();
    [SerializeField] private string weaponEquip;
    [SerializeField] private int comboBuffer;
    [SerializeField, Tooltip("Time range: value -- value*2")] private float comboResetTime;
    [SerializeField] private int maxStamina;
    [SerializeField] private int regenStaminaOfSeconds;

    [Header("Special")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private StaminaSliderUI staminaSlider;
    [SerializeField] private ComboView comboView;
    [SerializeField, Tooltip("Defalt: y=0 -> left; y=180 -> rigth")] private bool invertLeftAndRigth = false;
    [SerializeField] private List<GameObject> specialObjects = new List<GameObject>();
    [SerializeField] private bool debag;

    [Header("Permissions")]
    [SerializeField] private bool basicAttack = true;
    [SerializeField] private bool heavyAttack = true;
    [SerializeField] private bool skillAttack = true;
    [SerializeField] private bool dash = true;

    [Header("Debag")]
    [SerializeField] private int stamina;
    [SerializeField] private List<string> combo = new List<string>();

    private Dictionary<string, Animator> animatorPullList;
    private Dictionary<string, IWeapon> weaponsList;
    private Dictionary<string, GameObject> specialObjectsList;
    private bool readinessAttack = true;
    private ComboState comboState;
    private bool comboReset;

    private void Start()
    {
        animatorPullList = CompileAnimatorPullList(effectAnimatorsPull);
        weaponsList = CompileWeaponsList(weapons);
        specialObjectsList = CompileSpecialObjectsList(specialObjects);
        stamina = maxStamina;

        StartCoroutine(RegenStamina());
        StartCoroutine(ComboReseter());

        if (staminaSlider != null)
        {
            staminaSlider.SetMaxStamina(maxStamina);
        }

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

    private void Update()
    {
        if (staminaSlider != null)
        {
            staminaSlider.SetStamina(stamina);
        }

        if (comboView != null)
        {
            comboView.ViewCombo(combo);
        }
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

    private IEnumerator RegenStamina()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if(stamina + regenStaminaOfSeconds > maxStamina)
            {
                stamina = maxStamina;
            }
            else
            {
                stamina += regenStaminaOfSeconds;
            }        
        }
    }

    private IEnumerator ComboReseter()
    {
        while (true)
        {
            if (comboReset)
            {
                combo.Clear();
                comboReset = false;
            }
            else
            {
                comboReset = true;
            }

            yield return new WaitForSeconds(comboResetTime);
        }
    }

    private IEnumerator AttackDelay(float sec)
    {
        readinessAttack = false;
        yield return new WaitForSeconds(sec);
        readinessAttack = true;
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
        package.playerAnimator = playerAnimator;
        package.attackPoint = attackPoint;
        package.playerSide = CheckPlayerSide();
        package.audioSource = audioSource;

        return package;
    }

    private void StartAttack(string type)
    {
        (ComboState, bool, float) comboStatus;

        comboStatus = weaponsList[weaponEquip].ActivateCombo(combo, GetAttackControlPackage(type));

        if (comboStatus.Item2 && readinessAttack)
        {
            comboState = comboStatus.Item1;
            StartCoroutine(AttackDelay(comboStatus.Item3));
            combo.Clear();
        }
        else if (readinessAttack)
        {
            (int, float) status;
            status = weaponsList[weaponEquip].Attack(GetAttackControlPackage(type), stamina);

            if (status.Item1 > 0)
            {
                stamina -= status.Item1;
            }
            else if (staminaSlider != null)
            {
                staminaSlider.LowStaminaAlarm();
            }

            StartCoroutine(AttackDelay(status.Item2));

            comboReset = false;
            combo.Add(type);
        }

        if (combo.Count > comboBuffer)
        {
            combo.Clear();
        }
    }

    private void StartHit(string type)
    {
        if (comboState != null)
        {
            (ComboState, bool) comboStatus;

            comboStatus = weaponsList[weaponEquip].ContinueCombo(comboState, GetHitControlPackage(type));

            if (comboStatus.Item2)
            {
                comboState = comboStatus.Item1;
            }
            else
            {
                comboState = null;
                combo.Clear();
            }       
        }
        else
        {
            weaponsList[weaponEquip].Hit(GetHitControlPackage(type));
        }
    }

    public void BasicAttack()
    {
        StartAttack("basic");
    }

    public void HeavyAttack()
    {
        StartAttack("heavy");
    }

    public void SkillAttack()
    {
        StartAttack("skill");
    }

    public void HitBasicAttack()
    {
        StartHit("basic");
    }

    public void HitHeavyAttack()
    {
        StartHit("heavy");
    }

    public void HitSkillAttack()
    {
        StartHit("skill");
    }
}