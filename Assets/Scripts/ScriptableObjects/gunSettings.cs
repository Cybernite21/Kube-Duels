using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gunSettings", menuName = "ScriptableObjects/GunSettings")]
public class gunSettings : ScriptableObject
{
    [Header("Gun Settings:", order = 0)]
    public bool holdToRapidFire = false;
    public int damage = 5;
    public float bulletDeathSecs = 10f;
    public float gunTiltSpeed = 75f;
    public float bulletSpeed = 100f;
    [Space(5)]

    [Header("Cooldowns:", order = 1)]
    public float shootCoolDownSecs = 0.25f;
    [Tooltip("After how many bullets should the gun start gunCoolDown, 0 means no gunCoolDown")]
    public int gunCoolDown = 0;
    public float gunCoolDownSecs = 3f;
    [Space(5)]

    [Header("Aim Lazer:", order = 2)]
    public float aimLazerDistance = 2.5f;
    public LayerMask aimLazerMask;
    [Space(10)]

    [Header("Colors:", order = 3)]
    [ColorUsage(true, true)]
    public Color lazerNoEnemyColor;
    [ColorUsage(true, true)]
    public Color lazerEnemyColor;
}