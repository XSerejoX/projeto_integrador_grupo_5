using UnityEngine;

public class IdlePerseguidor : IState
{
	private readonly PerseguidorController perseguidorController;

	public IdlePerseguidor(PerseguidorController controller)
	{
		perseguidorController = controller;
	}

	public void Enter()
	{
		Debug.Log("Perseguidor entrou no estado idle.");
	}

	public void Update()
	{
		if (!perseguidorController.turnManager.isPlayerTurn)
			perseguidorController.stateMachine.ChangeState(perseguidorController.perseguindoState);
	}

	public void Exit()
	{
		Debug.Log("Perseguidor saiu do estado idle.");
	}
}
