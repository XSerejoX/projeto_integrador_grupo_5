using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int RawPizzaCount { get; private set; }
    public int BakedPizzaCount { get; private set; }

    public void AddRawPizza() => RawPizzaCount++;
    public void AddBakedPizza() => BakedPizzaCount++;

    public bool TryConsumeRawPizza()
    {
        if (RawPizzaCount <= 0) return false;
        RawPizzaCount--;
        return true;
    }

    public bool TryConsumeBakedPizza()
    {
        if (BakedPizzaCount <= 0) return false;
        BakedPizzaCount--;
        return true;
    }
}