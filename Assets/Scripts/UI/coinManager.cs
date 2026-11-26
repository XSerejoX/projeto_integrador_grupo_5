using UnityEngine;

public class coinManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Animator coin1;
    [SerializeField] private Animator coin2;
    [SerializeField] private Animator coin3;
    [SerializeField] private Animator shadow1;
    [SerializeField] private Animator shadow2;
    [SerializeField] private Animator shadow3;


    void Update()
    {
        if(inventory.CoinCount == 1)
        {
            coin1.SetTrigger(name: "on");
            shadow1.SetTrigger(name: "on");
        }

        if (inventory.CoinCount == 2)
        {
            coin2.SetTrigger(name: "on");
            shadow2.SetTrigger(name: "on");
        }

        if (inventory.CoinCount == 3)
        {
            coin3.SetTrigger(name: "on");
            shadow3.SetTrigger(name: "on");
        }

    }
}
