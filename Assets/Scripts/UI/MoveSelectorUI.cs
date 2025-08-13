using UnityEngine;
using CombatSystem.Enums;

public class MoveSelectorUI : MonoBehaviour
{
    [Header("References")]
    public PlayerCombatant player;
    public CombatManagerMono combatManager;

    [Header("Moves")]
    public MoveDataSO quickStabMove;
    public MoveDataSO heavySwingMove;
    public MoveDataSO flankJabMove;
    public MoveDataSO blockMove;
    public MoveDataSO dodgeMove;
    public MoveDataSO stepLeftMove;
    public MoveDataSO stepRightMove;
    public MoveDataSO stepForwardMove;
    public MoveDataSO stepBackwardMove;

    [Header("Behaviour")]
    [SerializeField] bool autoRunOnTwoHalvesOrFull = true;

    int halfCountThisTurn = 0;

    // One method per button:
    public void SelectQuickStab() => SelectMove(quickStabMove);
    public void SelectHeavySwing() => SelectMove(heavySwingMove);
    public void SelectFlankJab() => SelectMove(flankJabMove);
    public void SelectBlock() => SelectMove(blockMove);
    public void SelectDodge() => SelectMove(dodgeMove);
    public void SelectStepLeft() => SelectMove(stepLeftMove);
    public void SelectStepRight() => SelectMove(stepRightMove);
    public void SelectStepForward() => SelectMove(stepForwardMove);
    public void SelectStepBackward() => SelectMove(stepBackwardMove);

    // Main move-handling logic
     private void SelectMove(MoveDataSO moveSO)
    {
        if (!player || !moveSO) { Debug.LogWarning("Missing player or move!"); return; }

        switch (moveSO.actionType)
        {
            case ActionType.Step:
                player.StepChoice = moveSO.stepDirection;     // step is stored on the combatant
                Debug.Log($"[Combat] Selected Step: {moveSO.stepDirection}");
                break;

            // Treat these as half-turn actions
            case ActionType.HalfAttack:
                AddHalf(new HalfAttackAction(moveSO), moveSO.moveName);
                break;
            case ActionType.Dodge:
                AddHalf(new DodgeAction(moveSO), moveSO.moveName);
                break;
            case ActionType.Block:
                AddHalf(new BlockAction(moveSO), moveSO.moveName);
                break;

            case ActionType.FullAttack:
                SetFull(new FullAttackAction(moveSO), moveSO.moveName);
                break;
        }
    }

    void AddHalf(CombatAction action, string label)
    {
        // If this is first pick of the turn, reset half counter
        if (player.preparedActions.Count == 0) halfCountThisTurn = 0;

        player.preparedActions.Add(action);
        halfCountThisTurn++;
        Debug.Log("[Combat] Selected: " + label);

        if (autoRunOnTwoHalvesOrFull && halfCountThisTurn >= 2)
            ConfirmTurn();
    }

    void SetFull(CombatAction action, string label)
    {
        player.preparedActions.Clear(); // can't mix with halves
        halfCountThisTurn = 0;
        player.preparedActions.Add(action);
        Debug.Log("[Combat] Selected (Full): " + label);

        if (autoRunOnTwoHalvesOrFull)
            ConfirmTurn();
    }

    // Call from a Confirm button, or rely on autoRun
    public void ConfirmTurn()
    {
        Debug.Log("[Combat] Player done selecting. Running combat...");
        combatManager?.RunNextTurn();

    }

    public void ClearSelection()
    {
        player.preparedActions.Clear();
        halfCountThisTurn = 0;
        player.StepChoice = StepType.None;
    }
}