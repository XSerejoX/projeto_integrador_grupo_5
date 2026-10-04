using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int RawPizzaCount { get; private set; }
    public int BakedPizzaCount { get; private set; }
    public int CoinCount { get; private set; }

    public void AddRawPizza() => RawPizzaCount++;
    public void AddBakedPizza() => BakedPizzaCount++;
    public void AddCoin() => CoinCount++;

    public bool TryConsumeRawPizza()
    {
        if (RawPizzaCount <= 0) return false;
        RawPizzaCount--;
        return true;
    }

    public int ConsumeAllBakedPizzas()
    {
        int amount = BakedPizzaCount;
        BakedPizzaCount = 0;
        return amount;
    }
}