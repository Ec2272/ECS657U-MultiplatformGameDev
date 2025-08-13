using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem.Enums;

[CreateAssetMenu(fileName = "NewMove", menuName = "Combat/Move")]

public class MoveDataSO : ScriptableObject
{
    [Header("General Info")]
    public string moveName;
    public ActionType actionType;

    [Header("Combat Stats")]
    public int damage = 0;
    public int priority = 0;
    public int heatCost = 0;
    public int heatGain = 0;

    [Header("Positional Requirements")]
    public ZoneDistance requiredDistance = ZoneDistance.Mid;
    public ZoneDirection requiredDirection = ZoneDirection.Front;

    [Header("Defense Interactions")]
    public bool ignoreDodge = false;
    public bool ignoreBlock = false;

    [Header("Stepping (Only used for Step actions)")]
    public StepType stepDirection = StepType.None;
}


