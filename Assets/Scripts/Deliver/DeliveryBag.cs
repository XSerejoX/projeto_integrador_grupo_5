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
        //se ta adjacente e consumiu a baked pizza - pizza delivered
        if (!IsAdjacent(player.Cell)) return;
        if (!player.Inventory.TryConsumeBakedPizza()) return;

        DeliveryManager.Instance.NotifyPizzaDelivered();
        Debug.Log($"[Delivery] Pizza entregue na sacola em {Cell}! Placar atual: {DeliveryManager.Instance.Score}");

    }

    private bool IsAdjacent(Vector2Int otherCell)
    {
        //se a distancia da celula x/y do player com a atual for 1: true
        
        Vector2Int diff = otherCell - Cell;
        return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1;
    }
}