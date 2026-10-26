using System.Collections;
using UnityEngine;

public class ImparavelController : EnemyHazard, IEnemyTurn
{
    [SerializeField] private Animator animator;

    [Header("Ordem de turno (menor joga primeiro)")]
    [SerializeField] private int turnOrder = 0;
    public int TurnOrder => turnOrder;

    private static readonly int ImpDirX = Animator.StringToHash("JokerDirX");
    private static readonly int ImpDirY = Animator.StringToHash("JokerDirY");
    private static readonly int IsImpMoving = Animator.StringToHash("IsJokerMoving");

    [Tooltip("Quantas células ele percorre por turno")]
    public int tilesToMove = 3;

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    private void OnEnable() => EnemyTurnManager.Register(this);
    private void OnDisable() => EnemyTurnManager.Unregister(this);

    public IEnumerator TakeTurn(GridEntity player)
    {
        Vector2Int dir = Directions[Random.Range(0, Directions.Length)];

        if (!CanCharge(player, dir) && CanCharge(player, -dir))
            dir = -dir;

        yield return Charge(player, dir);
        while (IsAnimating || player.IsAnimating) yield return null;
    }

    private bool CanCharge(GridEntity player, Vector2Int dir)
    {
        Vector2Int nextCell = Cell + dir;
        return nextCell == player.Cell || GridManager.Instance.IsFree(nextCell);
    }

    private IEnumerator Charge(GridEntity player, Vector2Int dir)
    {
        for (int i = 0; i < tilesToMove; i++)
        {
            Vector2Int target = Cell + dir;

            if (target == player.Cell)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                    health.TakeDamage(dir, this);

                TryKnockback(player, dir);
                yield break;
            }

            if (!TryMove(dir)) break;
            while (IsAnimating) yield return null;
        }
    }

    private void TryKnockback(GridEntity player, Vector2Int dir)
    {
        if (player.TryMove(dir))
            TryMove(dir);
    }

    protected override void OnMoveStart(Vector2Int dir)
    {
        if (animator == null) return;
        animator.SetFloat(ImpDirX, dir.x);
        animator.SetFloat(ImpDirY, dir.y);
        animator.SetBool(IsImpMoving, true);
    }

    protected override void OnMoveEnd()
    {
        if (animator == null) return;
        animator.SetBool(IsImpMoving, false);
    }
}