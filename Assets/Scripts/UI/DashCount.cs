using UnityEngine;
using TMPro;

public class DashCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private NewPlayerController player;

    void Update()
    {
        text.text = player.DashCharges.ToString();
    }
}