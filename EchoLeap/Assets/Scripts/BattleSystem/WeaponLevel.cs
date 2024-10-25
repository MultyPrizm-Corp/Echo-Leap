using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponLevel", menuName = "Battle System/WeaponLevel")]
public class WeaponLevel : ScriptableObject
{
    public int level { get; private set; }
    public int damage { get; private set; }
    public int element { get; private set; }
}
