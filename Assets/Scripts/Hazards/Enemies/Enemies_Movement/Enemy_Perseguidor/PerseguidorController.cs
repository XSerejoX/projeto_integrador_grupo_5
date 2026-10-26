using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerseguidorController : EnemyHazard, IEnemyTurn
{
    [SerializeField] private Animator animator;

    [Header("Ordem de turno (menor joga primeiro)")]
    [SerializeField] private int turnOrder = 1;
    public int TurnOrder => turnOrder;

    private static readonly int PersDirX = Animator.StringToHash("PersDirX");
    private static readonly int PersDirY = Animator.StringToHash("PersDirY");
    private static readonly int IsPersMoving = Animator.StringToHash("IsPersMoving");

    private void OnEnable() => EnemyTurnManager.Register(this);
    private void OnDisable() => EnemyTurnManager.Unregister(this);

    public IEnumerator TakeTurn(GridEntity player)
    {
        if (FindNextStep(player.Cell, out Vector2Int dir))
        {
            Vector2Int target = Cell + dir;

            if (target == player.Cell) // chegou do lado do player: ataca + knockback
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                    health.TakeDamage(dir, this);

                TryKnockback(player, dir);
            }
            else
            {
                TryMove(dir);
            }
        }
        // se não há caminho, ele simplesmente espera o próximo turno

        while (IsAnimating || player.IsAnimating) yield return null;
        OnMoveEnd();
    }

    private void TryKnockback(GridEntity player, Vector2Int dir)
    {
        if (player.TryMove(dir))
            TryMove(dir);
    }

    protected override void OnMoveStart(Vector2Int dir)
    {
        if (animator == null) return;
        animator.SetFloat(PersDirX, dir.x);
        animator.SetFloat(PersDirY, dir.y);
        animator.SetBool(IsPersMoving, true);
    }

    protected override void OnMoveEnd()
    {
        if (animator == null) return;
        animator.SetBool(IsPersMoving, false);
    }

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    // BFS: devolve a direção do PRIMEIRO passo do menor caminho até o goal.
    private bool FindNextStep(Vector2Int goal, out Vector2Int firstStep)
    {
        firstStep = Vector2Int.zero;
        Vector2Int start = Cell;
        if (start == goal) return false;

        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var queue = new Queue<Vector2Int>();

        cameFrom[start] = start;
        queue.Enqueue(start);

        int offset = Random.Range(0, Directions.Length);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == goal) break;

            for (int i = 0; i < Directions.Length; i++)
            {
                Vector2Int next = current + Directions[(i + offset) % Directions.Length];

                if (cameFrom.ContainsKey(next)) continue;

                bool isGoal = next == goal;
                if (!isGoal && !GridManager.Instance.IsFree(next)) continue;

                cameFrom[next] = current;
                queue.Enqueue(next);
            }
        }

        if (!cameFrom.ContainsKey(goal)) return false;

        Vector2Int step = goal;
        while (cameFrom[step] != start)
            step = cameFrom[step];

        firstStep = step - start;
        return true;
    }
}