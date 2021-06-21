using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class playerSettings : ScriptableObject
{
    [Header("Player:", order = 0)]
    public float playerSpeed = 5f;
    public float smoothSpeed = 0.5f;
    public float turnSpeed = 100f;
    public int startingHealth = 100;
    [Space(10)]

    [Header("Effects:", order = 1)]
    public bool useBloodEffects = true;
    public GameObject bloodParticle;
}
