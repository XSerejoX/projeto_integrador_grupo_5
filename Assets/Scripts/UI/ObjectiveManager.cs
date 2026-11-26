using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Animator raw1;
    [SerializeField] private Animator raw2;
    [SerializeField] private Animator baked1;
    [SerializeField] private Animator baked2;
 

    // Update is called once per frame
    void Update()
    {
        if(inventory.RawPizzaCount == 1 && inventory.BakedPizzaCount == 0 )
        {
            raw1.SetTrigger(name: "Collect");
        }
        if (inventory.RawPizzaCount == 2 && inventory.BakedPizzaCount == 0)
        {
            raw2.SetTrigger(name: "Collect");
        }
        if ((inventory.RawPizzaCount == 1 && inventory.BakedPizzaCount == 1) || (inventory.RawPizzaCount == 0 && inventory.BakedPizzaCount == 1))
        {
            raw1.SetTrigger(name: "Give");
            baked1.SetTrigger(name: "Collect");
        }
        if (inventory.RawPizzaCount == 0 && inventory.BakedPizzaCount == 2)
        {
            raw2.SetTrigger(name: "Give");
            baked2.SetTrigger(name: "Collect");
        }
    }
}
