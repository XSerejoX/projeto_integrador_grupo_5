using UnityEngine;

public class Idle : IState
{
    public PlayerController playerController;

    public Idle(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        Debug.Log("Entrou no estado de Idle");
    }

    public void Update()
    {
        if (playerController.moveAction.WasPressedThisFrame())
        {
            playerController.stateMachine.ChangeState(playerController.moveState);
        }
        
    }

    public void Exit()
    {
        Debug.Log("Saiu do estado de Idle");
    }


}
