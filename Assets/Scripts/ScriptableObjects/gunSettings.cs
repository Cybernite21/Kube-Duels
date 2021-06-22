using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gunSettings", menuName = "ScriptableObjects/GunSettings")]
public class gunSettings : ScriptableObject
{
    [Header("Gun Settings:", order = 0)]
    public bool holdToRapidFire = false;
    public int damage = 5;
    public float gunTiltSpeed = 75f;
    public float bulletSpeed = 100f;
    public float shootCoolDownSecs = 0.25f;
    public float aimLazerDistance = 2.5f;
    public LayerMask aimLazerMask;
    [Space(10)]

    [Header("Colors", order = 1)]
    [ColorUsage(true, true)]
    public Color lazerNoEnemyColor;
    [ColorUsage(true, true)]
    public Color lazerEnemyColor;
}