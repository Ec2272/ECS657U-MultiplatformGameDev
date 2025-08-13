using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CombatSystem.Enums;

public class HalfAttackAction : CombatAction
{
    public HalfAttackAction(MoveDataSO moveData) : base(moveData) { }

    public override void Execute(CombatantBase user, CombatantBase target, int phase)
    {
        // Allow distance variety: QuickJab can work at Near or Mid
        bool validDistance = user.Distance == ZoneDistance.Near || user.Distance == ZoneDistance.Mid;

        if (!validDistance)
        {
            Debug.Log("[Combat] " + $"{MoveInfo.moveName} failed — invalid distance.");
            return;
        }

        // Direction check
        ZoneDirection relative = target.GetRelativeDirectionTo(user);
        if (MoveInfo.requiredDirection != ZoneDirection.Front && relative != MoveInfo.requiredDirection)
        {
            Debug.Log("[Combat] " + $"{MoveInfo.moveName} failed — wrong direction (required {MoveInfo.requiredDirection}, got {relative}).");
            return;
        }

        // Dodge logic
        bool dodged = (phase == 1 && target.Actions[0] is DodgeAction)
                   || (phase == 2 && target.Actions.Count > 1 && target.Actions[1] is DodgeAction);

        if (dodged && !MoveInfo.ignoreDodge)
        {
            Debug.Log("[Combat] " + $"{target.Name} dodged {MoveInfo.moveName}!");
            return;
        }

        // Block logic
        bool isBlocked = target.Actions.Any(a => a is BlockAction) && !MoveInfo.ignoreBlock;

        if (isBlocked)
        {
            Debug.Log("[Combat] " + $"{target.Name} blocked {MoveInfo.moveName}.");
            return;
        }

        // Flanking bonus (if coming from Left or Right)
        int bonus = (relative == ZoneDirection.Left || relative == ZoneDirection.Right) ? 5 : 0;
        int totalDamage = MoveInfo.damage + bonus;

        target.Health -= totalDamage;
        user.Heat += MoveInfo.heatGain;

        Debug.Log("[Combat] " + $"{user.Name} used {MoveInfo.moveName} from {relative} for {totalDamage} damage!");
    }
}



