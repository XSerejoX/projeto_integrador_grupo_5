using UnityEngine;

public sealed class EnemyKnockback : MonoBehaviour
{
    private EnemyHazard entity;

    public bool IsAnimating => entity != null && entity.IsAnimating;

    private void Awake()
    {
        entity = GetComponent<EnemyHazard>();
        if (entity == null)
            Debug.LogError("EnemyKnockback precisa de EnemyHazard.", this);
    }

    public bool TryMove(Vector2Int direction)
    {
        if (entity == null || GridManager.Instance == null)
            return false;

        return entity.TryMove(direction);
    }
}
