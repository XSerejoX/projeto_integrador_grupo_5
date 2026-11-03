using UnityEngine;

public class IceTile : MonoBehaviour
{
    private Vector2Int cell;
    [SerializeField, Min(1)] private int slideDistance = 2;

    private void Start()
    {
        GridManager gridManager = GridManager.Instance;
        if (gridManager == null)
        {
            Debug.LogError("", this);
            enabled = false;
            return;
        }

        cell = gridManager.WorldToCell(transform.position);
        gridManager.RegisterIceTile(this, cell);
    }

    private void OnDestroy()
    {
        if (GridManager.Instance != null)
        {
            GridManager.Instance.UnregisterIceTile(this, cell);
        }
    }

    public void SlideEntity(GridEntity entity)
    {
        if (entity == null || entity.LastMoveDirection == Vector2Int.zero) return;

        for (int step = 0; step < slideDistance; step++)
        {
            if (entity is EnemyHazard attacker)
            {
                Vector2Int nextCell = entity.Cell + entity.LastMoveDirection;
                GridEntity occupant = GridManager.Instance.GetAt(nextCell);
                if (occupant is PlayerType player)
                {
                    PlayerHealth health = player.GetComponent<PlayerHealth>();
                    if (health == null)
                        Debug.LogError("PlayerHealth is missing.", player);
                    else
                        health.TakeDamage(entity.LastMoveDirection, attacker);

                    break;
                }
            }

            if (!entity.TryMove(entity.LastMoveDirection, playMoveAnimation: false))
                break;
        }
    }
}
