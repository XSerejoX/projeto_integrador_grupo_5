using UnityEngine;

public class Baking : IState
{
    private Forno forno;
    private const int maxBakingTurns = 4;
    private int turnsToBake;

    public Baking(Forno forno)
    {
        this.forno = forno;
    }

    public void Enter()
    {
        turnsToBake = maxBakingTurns;

        // entra a animação de forno assando

        
        forno.deliveryManager.SetPizzaDelivered(false);

        forno.turnManager.OnPlayerTurnEnded += HandleTurnEnded;
        Debug.Log($"[Baking] Começou a assar! Faltam {turnsToBake} turnos"); // LOG TEMPORARIO
    }

    public void Update()
    {
        
    }

    private void HandleTurnEnded()
    {
        if (turnsToBake <= 0) return;

        turnsToBake--;
        Debug.Log($"[Baking] Turno passou. Faltam {turnsToBake} turnos pra pizza ficar pronta"); // LOG TEMPORARIO
        if (turnsToBake == 0)
        {
            forno.stateMachine.ChangeState(forno.idleState);
        }
    }

    public void Exit()
    {
        forno.turnManager.OnPlayerTurnEnded -= HandleTurnEnded; 

        // entra o fim da animação de assar

        forno.deliveryManager.SetPizzaReady(true); // avisa que a pizza terminou de assar
        Debug.Log("[Baking] Pizza pronta! Pode recolher"); // LOG TEMPORARIO
    }
}
