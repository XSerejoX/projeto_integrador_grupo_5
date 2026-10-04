using System;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event Action RawPizzaCollected;
    public event Action BakedPizzaAcquired;
    public event Action PizzaDelivered;

    private int totalPizzasToCollect;
    private int pizzasCollected;
    private int pizzasDelivered;

    void Awake()
    {
        Instance = this;
        Debug.Log($"[Delivery] DeliveryManager.Awake rodou. InstanceID: {GetEntityId()}");
    }

    public void SetPizzaCollectionGoal(Collectible[] collectibles)
    {
        totalPizzasToCollect = 0;
        if (collectibles == null) return;

        foreach (var collectible in collectibles)
            if (collectible != null && collectible.Type == Collectible.CollectibleType.RawPizza)
                totalPizzasToCollect++;

        Debug.Log($"[Delivery] Meta definida: {totalPizzasToCollect} pizzas.");
    }

    public void NotifyRawPizzaCollected()
    {
        pizzasCollected++;
        RawPizzaCollected?.Invoke();
        Debug.Log($"[Delivery] Coletadas: {pizzasCollected}/{totalPizzasToCollect}");
    }

    public void NotifyBakedPizzaAcquired() => BakedPizzaAcquired?.Invoke();

    public void NotifyPizzaDelivered(int amount)
    {
        Debug.Log($"[Delivery] NotifyPizzaDelivered chamado com amount = {amount} (InstanceID: {GetEntityId()})");

        pizzasDelivered += amount;
        PizzaDelivered?.Invoke();
        Debug.Log($"[Delivery] Entregues: {pizzasDelivered}/{totalPizzasToCollect}");

        if (totalPizzasToCollect > 0 && pizzasDelivered >= totalPizzasToCollect)
            GameManager.Instance?.TriggerVictory();
    }
}