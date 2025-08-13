using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAction : CombatAction
{
    public BlockAction(MoveDataSO moveInfo) : base(moveInfo) { }


    public override void Execute(CombatantBase user, CombatantBase target, int phase)
    {
        Debug.Log("[Combat] " + $"{user.Name} is blocking this turn.");
    }
}
