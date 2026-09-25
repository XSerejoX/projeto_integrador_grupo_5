public class FornoIdle : IState
{
    private Forno forno;

    public FornoIdle(Forno forno)
    {
        this.forno = forno;
    }

    public void Enter()
    {
        // entra a animação de forno parado/apagado, se tiver
    }

    public void Update()
    {
        if (forno.deliveryManager.isPizzaDelivered)
        {
            forno.stateMachine.ChangeState(forno.bakingState);
        }
    }

    public void Exit()
    {
    }
}
