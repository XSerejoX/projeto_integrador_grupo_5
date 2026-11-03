using UnityEngine;

public class MineTile : MonoBehaviour
{
    public const int TurnsPerMode = 3;
    private static readonly int IsArmedParameter = Animator.StringToHash("IsArmed");

    [SerializeField] private MineMode currentMode = MineMode.Armed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionLifetime = 1f;
    [SerializeField] private TurnManager turnManager;
    private Vector2Int cell;
    private int turnsInCurrentMode;

    public bool IsArmed => currentMode == MineMode.Armed;

    private void Start()
    {
        GridManager gridManager = GridManager.Instance;
        if (gridManager == null)
        {
            Debug.LogError("MineTile requires a GridManager.", this);
            enabled = false;
            return;
        }

        
        if (turnManager == null)
        {
            Debug.LogError("MineTile requires a TurnManager.", this);
            enabled = false;
            return;
        }

        cell = gridManager.WorldToCell(transform.position);
        gridManager.RegisterMineTile(this, cell);
        turnManager.OnPlayerTurnEnded += AdvanceMode;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogError("MineTile requires an Animator with an Isarmed bool parameter.", this);
        else
            animator.SetBool(IsArmedParameter, IsArmed);
    }

    private void OnDestroy()
    {
        if (turnManager != null)
            turnManager.OnPlayerTurnEnded -= AdvanceMode;

        if (GridManager.Instance != null)
            GridManager.Instance.UnregisterMineTile(this, cell);
    }

    public void OnEntityEntered(GridEntity entity)
    {
        if (!IsArmed || entity == null) return;

        PlayerHealth health = entity.GetComponent<PlayerHealth>();
        if (health == null) return;

        if (explosionPrefab == null)
        {
            Debug.LogError("MineTile requires an explosion prefab", this);
        }
        else
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionLifetime);
        }

        health.TakeDamage(1);
    }

    private void AdvanceMode()
    {
        turnsInCurrentMode++;
        if (turnsInCurrentMode < TurnsPerMode) return;
            //da switch nos modos da mina
        currentMode = IsArmed ? MineMode.Disarmed : MineMode.Armed; 
        turnsInCurrentMode = 0;
        if (animator != null)
            animator.SetBool(IsArmedParameter, IsArmed);

        if (IsArmed && GridManager.Instance != null)
            OnEntityEntered(GridManager.Instance.GetAt(cell));
    }

    private enum MineMode
    {
        Armed,
        Disarmed
    }
}
