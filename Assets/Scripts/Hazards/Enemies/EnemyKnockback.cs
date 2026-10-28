using UnityEngine;

public sealed class EnemyKnockback : MonoBehaviour
{
    private EnemyHazard giver;
    private EnemyHazard pushedEntity;

    public bool IsAnimating =>
        (giver != null && giver.IsAnimating) ||
        (pushedEntity != null && pushedEntity.IsAnimating);

    private void Awake()
    {
        giver = GetComponent<EnemyHazard>();
        if (giver == null)
            Debug.LogError("EnemyKnockback precisa de EnemyHazard.", this);
    }

    public bool TryMove(Vector2Int direction)
    {
        pushedEntity = null;

        if (giver == null)
            return false;

        if (GridManager.Instance == null)
        {
            Debug.LogError("EnemyKnockback precisa de GridManager.", this);
            return false;
        }

        GridEntity occupant = GridManager.Instance.GetAt(giver.Cell + direction);
        if (occupant == null)
            return giver.TryMove(direction);

        EnemyHazard receptor = occupant as EnemyHazard;
        if (receptor == null)
            return false;

        if (receptor == giver || !receptor.TryMove(direction))
            return false;

        pushedEntity = receptor;
        return giver.TryMove(direction);
    }
}
