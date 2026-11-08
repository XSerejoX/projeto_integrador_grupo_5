using System.Collections.Generic;
using UnityEngine;

// Coloque em qualquer GameObject da cena (ex.: o mesmo do EnemyTurnManager).
// Instancia um tile por célula do plano e faz todos piscarem juntos.
public class PathPreview : MonoBehaviour
{
    public static PathPreview Instance { get; private set; }

    [SerializeField] private PathTile tilePrefab;
    [SerializeField] private int blinkCount = 3;
    [SerializeField] private float blinkInterval = 0.12f; // tempo ligado e tempo desligado

    private readonly List<PathTile> active = new();

    // O manager espera esse tempo: o piscar manda no ritmo do turno.
    public float Duration => blinkCount * 2f * blinkInterval;

    private void Awake() => Instance = this;

    private void OnDisable() => Hide();

    public void Show(IReadOnlyList<Vector2Int> path)
    {
        Hide();

        if (tilePrefab == null)
        {
            Debug.LogError("PathPreview precisa de um tilePrefab.", this);
            return;
        }

        foreach (Vector2Int cell in path)
        {
            Vector3 pos = GridManager.Instance.CellToWorld(cell);
            PathTile tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
            tile.Blink(blinkCount, blinkInterval);
            active.Add(tile);
        }
    }

    public void Hide()
    {
        foreach (PathTile tile in active)
            if (tile != null) Destroy(tile.gameObject);
        active.Clear();
    }
}
