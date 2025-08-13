using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem.Enums;
using System.Linq;


public abstract class CombatantBase: MonoBehaviour
{
    public bool HasActionAtPhase(int phase) => Actions.Count >= phase;
    public bool IsDodgingOnPhase(int phase) => HasActionAtPhase(phase) && Actions[phase - 1] is DodgeAction;

    public string Name;
    public int Health;
    public int Speed;
    public int Heat;

    public ZoneDirection Facing = ZoneDirection.Front;
    public ZoneDistance Distance = ZoneDistance.Mid;
    public StepType StepChoice = StepType.None;
    public bool HasSteppedThisTurn = false;


    public List<CombatAction> Actions = new();

    public abstract void ChooseActions();

    public ZoneDirection GetRelativeDirectionTo(CombatantBase other)
    {
        int diff = ((int)other.Facing - (int)this.Facing + 4) % 4;

        return diff switch
        {
            0 => ZoneDirection.Front,
            1 => ZoneDirection.Right,
            2 => ZoneDirection.Back,
            3 => ZoneDirection.Left,
            _ => ZoneDirection.Front,
        };
    }
}



