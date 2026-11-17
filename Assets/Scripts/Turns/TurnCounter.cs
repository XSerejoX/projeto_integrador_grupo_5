using TMPro;
using UnityEngine;

public class TurnCounter : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private string format = "Turnos: {0}";

    public int Turns { get; private set; } //tornando a variavel acessivel

    private void OnEnable()
    {
        if (turnManager == null) // se nulo, procura por turn manager
            turnManager = FindAnyObjectByType<TurnManager>();

        if (turnManager != null)//inscreve este metodo no evento de fim de turno
            turnManager.OnPlayerTurnEnded += HandleTurnEnded;// <- este metodo

        UpdateUI();
    }

    private void OnDisable()
    {
        if (turnManager != null) //desincreve quando o objeto for desativado
            turnManager.OnPlayerTurnEnded -= HandleTurnEnded;
    }

    private void HandleTurnEnded()
    {
        Turns++; //incrementa +1 no contador de turnos
        UpdateUI();// atualiza o texto na UI
    }

    public void ResetCounter()
    {
        Turns = 0; //reseta o contador de turnos
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (turnText != null)
            turnText.text = string.Format(format, Turns);// coloca o texto na UI
    }
}