using UnityEngine;

public class DeliveryBag : GridEntity
{
    [SerializeField] private NewPlayerController player;

    private void OnEnable()
    {
        if (player == null)
            player = FindAnyObjectByType<NewPlayerController>();

        if (player != null)
            player.Moved += TryDeliver;
    }

    private void OnDisable()
    {
        if (player != null)
            player.Moved -= TryDeliver;
    }

    private void TryDeliver()
    {
        if (!IsAdjacent(player.Cell)) return;

        int delivered = player.Inventory.ConsumeAllBakedPizzas();
        if (delivered <= 0) return; // não tinha pizza assada nenhuma, nada acontece

        DeliveryManager.Instance.NotifyPizzaDelivered(delivered);
        Debug.Log($"[Delivery] {delivered} pizza(s) entregue(s) na sacola em {Cell}!");
    }

    private bool IsAdjacent(Vector2Int otherCell)
    {
        Vector2Int diff = otherCell - Cell;
        return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1;
    }
}