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
		
	}

	public void Update()
	{
		if (!perseguidorController.turnManager.isPlayerTurn)
			perseguidorController.stateMachine.ChangeState(perseguidorController.perseguindoState);
	}

	public void Exit()
	{
		
	}
}
