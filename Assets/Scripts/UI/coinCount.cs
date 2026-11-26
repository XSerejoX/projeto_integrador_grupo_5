using TMPro;
using UnityEngine;

public class coinCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerInventory inventory;

    void Update()
    {
        text.text = inventory.CoinCount.ToString();
    }
}
