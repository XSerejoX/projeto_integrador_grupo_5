using UnityEngine;
using System.Collections;

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

    public IEnumerator TakeTurn(GridEntity player)
    {
        Vector2Int dir = Directions[Random.Range(0, Directions.Length)];
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
            if (!TryMove(dir))  // se não conseguir mover
                TryMove(-dir);  // tenta a direção oposta
        }

        while (IsAnimating || player.IsAnimating) yield return null;
        OnMoveEnd();
    }

    private void TryKnockback(GridEntity player, Vector2Int dir)
    {
        if (player.TryMove(dir))
            TryMove(dir);
    }
}