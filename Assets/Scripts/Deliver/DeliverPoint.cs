using UnityEngine;

public class DeliverPoint : MonoBehaviour
{
    public bool allPizzasDelivered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null || inventory.BakedPizzasAquired <= 0) return;

        inventory.BakedPizzasAquired--;
        Debug.Log("[DeliverPoint] Pizza assada entregue!", this);

        if(inventory.BakedPizzasAquired <= 0)
        {
            allPizzasDelivered = true;        
        }
    
    }
}
