using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CombatSystem.Enums;

public class FullAttackAction : CombatAction
{
    public FullAttackAction(MoveDataSO moveInfo) : base(moveInfo) { }

    public override void Execute(CombatantBase user, CombatantBase target, int phase)
    {
        bool doubleDodge = target.Actions.All(a => a is DodgeAction);

        if (doubleDodge && !MoveInfo.ignoreDodge)
        {
            Debug.Log("[Combat] " + $"{target.Name} avoided {MoveInfo.moveName} with two dodges!");
            return;
        }

        // Allow from Near and Mid
        bool validDistance = user.Distance == ZoneDistance.Near || user.Distance == ZoneDistance.Mid;
        if (!validDistance)
        {
            Debug.Log("[Combat] " + $"{MoveInfo.moveName} failed — invalid distance.");
            return;
        }

        // Special: Bonus damage if attacker is Far (for Lunge-type moves)
        int distanceBonus = user.Distance == ZoneDistance.Far ? 15 : 0;
        int totalDamage = MoveInfo.damage + distanceBonus;

        target.Health -= totalDamage;
        user.Heat += MoveInfo.heatGain;

        Debug.Log("[Combat] " + $"{user.Name} used {MoveInfo.moveName} from {user.Distance} and dealt {totalDamage} damage!");
    }
}


