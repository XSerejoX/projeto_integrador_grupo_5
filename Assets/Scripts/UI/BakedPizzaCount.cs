using UnityEngine;
using TMPro;

public class BakedPizzaCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerInventory inventory;

    void Update()
    {
        text.text = inventory.BakedPizzaCount.ToString();
    }
}