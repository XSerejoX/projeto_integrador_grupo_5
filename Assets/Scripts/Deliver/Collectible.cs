using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType
    {
        RawPizza,
        Coin
    }

    [SerializeField] private Vector2Int cell;
    [SerializeField] private CollectibleType type;
    public Vector2Int Cell => cell;
    public CollectibleType Type => type;

    void Start()
    {
        cell = GridManager.Instance.WorldToCell(transform.position);
        transform.position = GridManager.Instance.CellToWorld(cell);
    }

    public bool TryCollect(PlayerInventory inventory, Vector2Int playerCell)
    {
        if (playerCell != cell) return false;

        if (type == CollectibleType.RawPizza)
        {
            inventory.AddRawPizza();
            DeliveryManager.Instance?.NotifyRawPizzaCollected();
            Debug.Log($"[Delivery] Pizza crua coletada em {cell}. Total cru no inventário: {inventory.RawPizzaCount}");
        }
        else
        {
            inventory.AddCoin();
            Debug.Log($"Moeda coletada em {cell}. Total de moedas no inventário: {inventory.CoinCount}");
        }

        Destroy(gameObject);
        return true;
    }
}