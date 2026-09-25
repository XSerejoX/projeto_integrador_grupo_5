using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public bool isPizzaDelivered { get; private set; }
    public bool isPizzaReady { get; private set; } // pizza terminou de assar e pode ser recolhida

    public void SetPizzaDelivered(bool value)
    {
        isPizzaDelivered = value;
    }

    public void SetPizzaReady(bool value)
    {
        isPizzaReady = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // mesmo padrão do pizza.cs pra achar o inventário do player
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null) return;

        if (inventory.PizzasAquired > 0 && !isPizzaDelivered)
        {
            // player chegou com pizza crua -> entrega pro forno
            inventory.PizzasAquired--;
            SetPizzaDelivered(true);
            Debug.Log("[DeliveryManager] Pizza crua entregue ao forno!", this);
            return;
        }

        if (isPizzaReady)
        {
            // player chegou depois da pizza pronta -> recolhe
            isPizzaReady = false;
            inventory.BakedPizzasAquired++;
            Debug.Log("[DeliveryManager] Pizza pronta recolhida!", this);
        }
    }
}


