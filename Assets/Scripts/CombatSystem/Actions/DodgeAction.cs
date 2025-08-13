using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAction : CombatAction
{
    public DodgeAction(MoveDataSO moveInfo) : base(moveInfo) { }

    public override void Execute(CombatantBase user, CombatantBase target, int phase)
    {
        Debug.Log("[Combat] " + $"{user.Name} is dodging on phase {phase}");
    }

}
