using System;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event Action RawPizzaCollected;
    public event Action BakedPizzaAcquired;
    public event Action PizzaDelivered;

    public int Score { get; private set; }

    void Awake() => Instance = this;

    public void NotifyRawPizzaCollected() => RawPizzaCollected?.Invoke();
    public void NotifyBakedPizzaAcquired() => BakedPizzaAcquired?.Invoke();

    public void NotifyPizzaDelivered()
    {
        Score++;
        PizzaDelivered?.Invoke();
    }
}