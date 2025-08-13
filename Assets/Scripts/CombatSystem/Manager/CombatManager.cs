using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CombatSystem.Enums;
using System;


public enum CombatOutcome { Draw, PlayerLost, PlayerWon }

public class CombatManager
{
    private readonly CombatantBase player;
    private readonly CombatantBase enemy;

    public event Action<CombatOutcome> OnCombatEnded;
    private bool combatOver = false;

    public CombatManager(CombatantBase player, CombatantBase enemy)
    {
        this.player = player;
        this.enemy  = enemy;
    }

    /*summary
     Resolves exactly one turn:
     - Each side chooses actions
     - Apply step (and reset it so it doesn't persist)
     - Phase 1: half-turn actions only
     - Phase 2: half-turn actions only
     - Full-turn pass: full-turn actions only, exactly once
     - End checks + cleanup
     */
    public void StartTurn()
    {
        if (combatOver) return;

        // 1) Let actors choose their actions for THIS turn
        SafeChoose(player);
        SafeChoose(enemy);

        // 2) Apply steps (gameplay state: facing / distance); then reset
        ApplyStep(player, enemy);
        ApplyStep(enemy, player);

        // 3) Phase 1 (half-turn only), speed order
        ExecutePhase(phase: 1);
        if (CheckForCombatEnd()) { EndTurnCleanup(); return; }

        // 4) Phase 2 (half-turn only), speed order
        ExecutePhase(phase: 2);
        if (CheckForCombatEnd()) { EndTurnCleanup(); return; }

        // 5) Full-turn pass (full-turn only, once), speed order
        ExecuteFullTurn();
        if (CheckForCombatEnd()) { EndTurnCleanup(); return; }

        // 6) heat updates (later)

        // 7) Cleanup so next turn starts clean
        EndTurnCleanup();
    }

    // Choosing

    private void SafeChoose(CombatantBase c)
    {
        try { c?.ChooseActions(); }
        catch (Exception ex)
        {
            Debug.LogError($"ChooseActions() error on {c?.Name ?? "null"}: {ex}");
        }
    }

    // Step application (gameplay state only; visuals handled elsewhere) 

    private void ApplyStep(CombatantBase actor, CombatantBase target)
    {
        if (actor == null || target == null) return;

        switch (actor.StepChoice)
        {
            case StepType.Left:
                actor.Facing = TurnLeft(actor.Facing);
                Debug.Log("[Combat] " + $"{actor.Name} stepped Left. Now facing {actor.Facing}.");
                break;

            case StepType.Right:
                actor.Facing = TurnRight(actor.Facing);
                Debug.Log("[Combat] " + $"{actor.Name} stepped Right. Now facing {actor.Facing}.");
                break;

            case StepType.Forward:
                actor.Distance = StepForward(actor.Distance);
                Debug.Log("[Combat] " + $"{actor.Name} stepped Forward. Distance is now {actor.Distance}.");
                break;

            case StepType.Backward:
                actor.Distance = StepBackward(actor.Distance);
                Debug.Log("[Combat] " + $"{actor.Name} stepped Backward. Distance is now {actor.Distance}.");
                break;

            case StepType.None:
            default:
                break;
        }

        // Reset so steps don't go into the next turn
        actor.StepChoice = StepType.None;
        actor.HasSteppedThisTurn = false;
    }

    private ZoneDirection TurnLeft(ZoneDirection f)
    {
        // Assuming enum order: Front=0, Right=1, Back=2, Left=3
        return (ZoneDirection)(((int)f + 3) & 0x3); // -1 mod 4
    }

    private ZoneDirection TurnRight(ZoneDirection f)
    {
        return (ZoneDirection)(((int)f + 1) & 0x3); // +1 mod 4
    }

    private ZoneDistance StepForward(ZoneDistance d)
    {
        // Assuming enum order: Near=0, Mid=1, Far=2
        int v = Mathf.Clamp((int)d - 1, 0, 2);
        return (ZoneDistance)v;
    }

    private ZoneDistance StepBackward(ZoneDistance d)
    {
        int v = Mathf.Clamp((int)d + 1, 0, 2);
        return (ZoneDistance)v;
    }

    // Phases (half-turn actions only)

    private void ExecutePhase(int phase)
    {
        if (player == null || enemy == null) return;

        // Speed order (fast acts first). If equal, player first.
        CombatantBase first  = (player.Speed >= enemy.Speed) ? player : enemy;
        CombatantBase second = (first == player) ? enemy : player;

        TryExecHalf(first,  second, phase);
        TryExecHalf(second, first,  phase);
    }

    private void TryExecHalf(CombatantBase user, CombatantBase target, int phase)
    {
        if (user == null || target == null) return;

        // Only allow half-turn actions in phases
        if (user.Actions.Count >= phase && user.Actions[phase - 1] is HalfAttackAction)
        {
            try { user.Actions[phase - 1].Execute(user, target, phase); }
            catch (Exception ex)
            {
                Debug.LogError($"Phase {phase} action error on {user.Name}: {ex}");
            }
        }
    }

    //Full-turn (full actions only, exactly once)

    private void ExecuteFullTurn()
    {
        if (player == null || enemy == null) return;

        bool playerHasFull = (player.Actions.Count == 1 && player.Actions[0] is FullAttackAction);
        bool enemyHasFull  = (enemy.Actions.Count  == 1 && enemy.Actions[0]  is FullAttackAction);

        if (!playerHasFull && !enemyHasFull) return;

        // Speed order again
        CombatantBase first  = (player.Speed >= enemy.Speed) ? player : enemy;
        CombatantBase second = (first == player) ? enemy : player;

        if (first.Actions.Count == 1 && first.Actions[0] is FullAttackAction)
        {
            TryExecFull(first, second);
        }

        if (second.Actions.Count == 1 && second.Actions[0] is FullAttackAction)
        {
            TryExecFull(second, first);
        }
    }

    private void TryExecFull(CombatantBase user, CombatantBase target)
    {
        try { user.Actions[0].Execute(user, target, phase: 0); }
        catch (Exception ex)
        {
            Debug.LogError($"Full-turn action error on {user.Name}: {ex}");
        }
    }

    // End checks & cleanup

    private bool CheckForCombatEnd()
    {
        if (combatOver) return true;

        if (player.Health <= 0 && enemy.Health <= 0)
        {
            Debug.Log("[Combat] Draw!");
            combatOver = true;
            OnCombatEnded?.Invoke(CombatOutcome.Draw);
            return true;
        }
        if (player.Health <= 0)
        {
            Debug.Log("[Combat] You lost.");
            combatOver = true;
            OnCombatEnded?.Invoke(CombatOutcome.PlayerLost);
            return true;
        }
        if (enemy.Health <= 0)
        {
            Debug.Log("[Combat] You won!");
            combatOver = true;
            OnCombatEnded?.Invoke(CombatOutcome.PlayerWon);
            return true;
        }

        return false;
        }

    private void EndTurnCleanup()
    {
        CleanOne(player);
        CleanOne(enemy);
    }

    private void CleanOne(CombatantBase c)
    {
        if (c == null) return;
        c.Actions.Clear();
        c.StepChoice = StepType.None;
        c.HasSteppedThisTurn = false;
    }
}
