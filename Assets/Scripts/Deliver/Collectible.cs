using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Vector2Int cell;
    public Vector2Int Cell => cell;

    void Start()
    {
        cell = GridManager.Instance.WorldToCell(transform.position);
        transform.position = GridManager.Instance.CellToWorld(cell);
    }

    // Chamado pelo PlayerController depois de um TryMove bem sucedido
    
        //checa se o collectible o player tao na mesma celula
    public bool TryCollect(PlayerInventory inventory, Vector2Int playerCell)
    {
        if (playerCell != cell) return false;

        inventory.AddRawPizza(); //se sim, add a raw pizza
        DeliveryManager.Instance.NotifyRawPizzaCollected();
        Debug.Log($"[Delivery] Pizza crua coletada em {cell}. Total cru no inventário: {inventory.RawPizzaCount}");
        Destroy(gameObject);
        return true;
    }
}