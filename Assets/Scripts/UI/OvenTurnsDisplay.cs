using TMPro;
using UnityEngine;

// Mostra só um número num texto do Canvas (ex.: 3, depois 2, depois 1).
// Quem manda no valor é o forno: ele chama SetTurns() a cada Tick.
public class OvenTurnsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [Tooltip("Esconde o texto quando não há pizza assando (turnos <= 0)")]
    [SerializeField] private bool hideWhenZero = true;

    private void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
        if (label == null)
            Debug.LogError("OvenTurnsDisplay precisa de um TMP_Text.", this);
    }

    public void SetTurns(int turns)
    {
        if (label == null) return;

        label.enabled = !(hideWhenZero && turns <= 0);
        label.text = turns.ToString();
    }
}
