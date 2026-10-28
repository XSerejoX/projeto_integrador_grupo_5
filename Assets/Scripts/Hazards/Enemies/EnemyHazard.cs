using UnityEngine;
public abstract class EnemyHazard : GridEntity
{
    private EnemyKnockback knockback;

    protected virtual void Awake()
    {
        knockback = GetComponent<EnemyKnockback>();
        if (knockback == null)
            knockback = gameObject.AddComponent<EnemyKnockback>();
    }

    protected bool TryMoveWithKnockback(Vector2Int direction) =>
        knockback.TryMove(direction);

    protected bool IsKnockbackAnimating =>
        knockback != null && knockback.IsAnimating;
}