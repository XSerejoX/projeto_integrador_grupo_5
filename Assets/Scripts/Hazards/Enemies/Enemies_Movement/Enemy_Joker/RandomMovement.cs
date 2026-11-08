using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomMovement : EnemyHazard, IEnemyTurn
{
    [SerializeField] private Animator animator;

    [Header("Ordem de turno (menor joga primeiro)")]
    [SerializeField] private int turnOrder = 2;
    public int TurnOrder => turnOrder;

    private static readonly int JokerDirX = Animator.StringToHash("JokerDirX");
    private static readonly int JokerDirY = Animator.StringToHash("JokerDirY");
    private static readonly int IsJokerMoving = Animator.StringToHash("IsJokerMoving");
    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    private void OnEnable() => EnemyTurnManager.Register(this);
    private void OnDisable() => EnemyTurnManager.Unregister(this);

    protected override void OnMoveStart(Vector2Int dir)
    {
        if (animator == null) return;
        animator.SetFloat(JokerDirX, dir.x);
        animator.SetFloat(JokerDirY, dir.y);
        animator.SetBool(IsJokerMoving, true);
    }

    protected override void OnMoveEnd()
    {
        if (animator == null) return;
        animator.SetBool(IsJokerMoving, false);
    }

    // Sorteia a direção agora (a prévia e a execução usam a mesma decisão).
    public EnemyPlan PlanTurn(GridEntity player)
    {
        var plan = new EnemyPlan { Path = new List<Vector2Int>() };
        Vector2Int dir = Directions[Random.Range(0, Directions.Length)];

        if (Cell + dir == player.Cell || GridManager.Instance.IsFree(Cell + dir))
            plan.Path.Add(Cell + dir);
        else if (GridManager.Instance.IsFree(Cell - dir))
            plan.Path.Add(Cell - dir);

        plan.HitsPlayer = plan.Path.Count > 0 && plan.Path[0] == player.Cell;
        return plan;
    }

    public IEnumerator ExecuteTurn(GridEntity player, EnemyPlan plan)
    {
        yield return FollowPlan(player, plan);
        OnMoveEnd();
    }
}
