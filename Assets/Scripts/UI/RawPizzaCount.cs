using UnityEngine;
using TMPro;

public class RawPizzaCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerInventory inventory;

    void Update()
    {
        text.text = inventory.RawPizzaCount.ToString();
    }
}