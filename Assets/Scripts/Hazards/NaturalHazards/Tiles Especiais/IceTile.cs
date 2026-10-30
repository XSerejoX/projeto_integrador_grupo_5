using UnityEngine;

public class IceTile : MonoBehaviour
{
    private Vector2Int cell;

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
        entity.TryMove(entity.LastMoveDirection, playMoveAnimation: false);
    }
}
