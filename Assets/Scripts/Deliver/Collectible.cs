using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private static readonly HashSet<int> CollectedIds = new HashSet<int>();

    [SerializeField] private Vector2Int cell;
    public Vector2Int Cell => cell;
    public bool IsRawPizza => CompareTag("RawPizza");

    private void Start()
    {
        cell = GridManager.Instance.WorldToCell(transform.position);
        transform.position = GridManager.Instance.CellToWorld(cell);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;


        var inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null)
        {
            return;
        }

        if (CompareTag("RawPizza"))
        {
            inventory.AddRawPizza();
            DeliveryManager.Instance?.NotifyRawPizzaCollected();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.pickPizzaEnlatada);
            }

            Debug.Log($"[Delivery] Pizza crua coletada em {cell}. Total cru no inventário: {inventory.RawPizzaCount}");
        }
        else if (CompareTag("Coin"))
        {
            inventory.AddCoin();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.pickMoeda);
            }

            Debug.Log($"Moeda coletada em {cell}. Total de moedas no inventário: {inventory.CoinCount}");
        }
        else
        {
            Debug.LogWarning($"Collectible on {name} has unsupported tag '{tag}'.");
            return;
        }

        Destroy(gameObject);
    }
}