using UnityEngine;

public class pizza : MonoBehaviour
{
 
   private bool collected;
   public int pizzaUnit = 1; 
    private void OnTriggerEnter2D(Collider2D other) // ao detectar colisao
    {
        if (collected) return; // uma vez o item coletado nao retorna nada

            //quando o player colide com a pizza, procura o componente do player-PlayerInventory
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>(); 
        if (inventory == null) return;   // caso nao tenha retorna nada

        collected = true;
        inventory.PizzasAquired += pizzaUnit; //incrementa 1 pizza no invetario

        GetComponent<Collider2D>().enabled = false; // desativa o colisor
        Invoke(nameof(HidePizza), 0.2f); //desativa a pizza num delay 
    }

    private void HidePizza()
    {
        gameObject.SetActive(false); //desativa a pizza
    }
  
}
