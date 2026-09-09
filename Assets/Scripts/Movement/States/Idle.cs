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
            //lidando com as diagonais. caso o jogador aperte uma diagonal, ele vai se mover na horizontal
        if (playerController.moveAction.WasPressedThisFrame())
        {
            if (playerController.directionVector.x > 0 && playerController.directionVector.y > 0)
            {
                playerController.directionVector = new Vector2(1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            
            }

            else if (playerController.directionVector.x < 0 && playerController.directionVector.y < 0)
            {
                playerController.directionVector = new Vector2(-1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            }

           else if (playerController.directionVector.x > 0 && playerController.directionVector.y < 0)
            {
                playerController.directionVector = new Vector2(1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            }

            else if (playerController.directionVector.x < 0 && playerController.directionVector.y > 0)
            {
                playerController.directionVector = new Vector2(-1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            }
           
            else
            {
                playerController.stateMachine.ChangeState(playerController.moveState);
            }  
        }
        
    }

    public void Exit()
    {
        Debug.Log("Saiu do estado de Idle");
    }


}
