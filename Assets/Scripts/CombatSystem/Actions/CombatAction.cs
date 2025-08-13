using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem.Enums;

public abstract class CombatAction
{
    public MoveDataSO MoveInfo { get; private set; }

    public CombatAction(MoveDataSO moveData)
    {
        this.MoveInfo = moveData;                   
    }

    public ActionType ActionType => MoveInfo.actionType;

    public abstract void Execute(CombatantBase user, CombatantBase target, int phase);
}
