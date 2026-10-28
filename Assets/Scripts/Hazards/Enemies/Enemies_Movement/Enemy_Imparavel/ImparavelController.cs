using System;

using System.Collections;
using UnityEngine;

public class ImparavelController : EnemyHazard, IEnemyTurn
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerType player;
    [Header("Ordem de turno (menor joga primeiro)")]
    [SerializeField] private int turnOrder = 0;
    [SerializeField] private GameObject firePrefab;

    public int TurnOrder => turnOrder;

    private static readonly int ImpDirX = Animator.StringToHash("ImpDirX");
    private static readonly int ImpDirY = Animator.StringToHash("ImpDirY");
    private static readonly int IsImpMoving = Animator.StringToHash("IsImpMoving");

    [Tooltip("Quantas células ele percorre por turno")]
    public int tilesToMove = 4;

    public Vector2Int cell;
    public event Action HitWall;

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    private void OnEnable()
    {
        EnemyTurnManager.Register(this);
        HitWall += SpawnFiresAroundImparavel;
    }

    private void OnDisable()
    {
        HitWall -= SpawnFiresAroundImparavel;
        EnemyTurnManager.Unregister(this);
    }

    public IEnumerator TakeTurn(GridEntity player)
    {
        Vector2Int dir = Directions[UnityEngine.Random.Range(0, Directions.Length)];

        if (!CanCharge(player, dir) && CanCharge(player, -dir))
            dir = -dir;

        yield return Charge(player, dir);
        while (IsAnimating || player.IsAnimating || IsKnockbackAnimating) yield return null;
    }

    private bool CanCharge(GridEntity player, Vector2Int dir)
    {
        Vector2Int nextCell = Cell + dir;
        GridEntity receptor = GridManager.Instance.GetAt(nextCell);
        return receptor == null
            ? GridManager.Instance.IsFree(nextCell)
            : receptor == player || GridManager.Instance.IsFree(nextCell + dir);
    }

    private IEnumerator Charge(GridEntity player, Vector2Int dir)
    {
        for (int i = 0; i < tilesToMove; i++)
        {
            Vector2Int target = Cell + dir;
            GridEntity targetEntity = GridManager.Instance.GetAt(target);

            if (targetEntity is PlayerType)
            {
                PlayerHealth health = targetEntity.GetComponent<PlayerHealth>();
                if (health == null)
                    Debug.LogError("missing", targetEntity);
                else
                    health.TakeDamage(dir, this);

                break;
            }

            if (!GridManager.Instance.IsFree(target) &&
                targetEntity is not EnemyHazard)
            {
                HitWall?.Invoke();
                break;
            }

            if (!TryMoveWithKnockback(dir)) break;
            while (IsAnimating || IsKnockbackAnimating) yield return null;
        }
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

private static readonly Vector2Int[] AdjacentOffsets =
{
    new(-1, -1), new(0, -1), new(1, -1),
    new(-1,  0),             new(1,  0),
    new(-1,  1), new(0,  1), new(1,  1)
};

    private void SpawnFiresAroundImparavel()
    {
        if (firePrefab == null)
        {
            Debug.LogError("Imparavel needs a fire effect prefab.", this);
            return;
        }

        foreach (Vector2Int offset in AdjacentOffsets)
        {
            Vector2Int fireCell = Cell + offset;

            GridEntity occupant = GridManager.Instance.GetAt(fireCell);

            if (GridManager.Instance.OutBounds(fireCell) ||
                (occupant != null &&
                occupant is not PlayerType &&
                (occupant is not EnemyHazard)))
            {
                continue;
            }

            Vector3 worldPosition = GridManager.Instance.CellToWorld(fireCell);
            GameObject fire = Instantiate(firePrefab, worldPosition, Quaternion.identity);
            foreach (GridEntity entity in fire.GetComponentsInChildren<GridEntity>(true))
                entity.enabled = false;
            Destroy(fire, 1.5f);

        }
    }

}