using UnityEngine;

public class Fire : MonoBehaviour
{
    private const string NaturalHazardTag = "NaturalHazard";
    private bool hasDamagedPlayer;

    private void Awake()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>(true);
        if (colliders.Length == 0)
        {
            Debug.LogError("Fire requires a Collider2D to identify its hazard area.", this);
            return;
        }

        foreach (Collider2D hazardCollider in colliders)
            hazardCollider.gameObject.tag = NaturalHazardTag;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamagedPlayer) return;

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health == null) return;

        hasDamagedPlayer = true;
        health.TakeDamage(1);
    }
}
